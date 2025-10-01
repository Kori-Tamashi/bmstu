#include "connection_handler.h"
#include "request_parser.h"
#include "response_builder.h"
#include "file_reader.h"
#include "logger.h"

#define CONNECTION_HANDLER_CONTEXT__ connection_handler_context_init()

static inline conn_ctxt_t connection_handler_context_init()
{
    conn_ctxt_t conn_ctxt = (conn_ctxt_t) {
        .read_fds = {0},
        .max_fd = 0,
        .conn_sockets = {0},
        .conn_sockets_count = 0
    };
    
    FD_ZERO(&conn_ctxt.read_fds);
    memset(conn_ctxt.conn_sockets, 0, MAX_CONNECTIONS);

    return conn_ctxt;
}

static inline error_t connection_handler_new_connection_handle(worker_info_t* w, conn_ctxt_t* conn_ctxt)
{
    if (IS_NULL(w))
        return EXIT_FAILURE;

    if (IS_NULL(conn_ctxt))
        return EXIT_FAILURE;

    int conn_sock_fd = recv_fd(w->to_worker_channel[0]);
    if (IS_ERROR(conn_sock_fd)) {
        LOG_ERROR(w->logger, "connection_handler_new_connection_handle: failed to receive file descriptor");
        return EXIT_FAILURE;
    }

    if (conn_ctxt->conn_sockets_count >= MAX_CONNECTIONS) {
        close(conn_sock_fd);
        LOG_WARNING(w->logger, "connection_handler_new_connection_handle: max clients reached, connection socket_fd %d rejected", conn_sock_fd);
        return EXIT_FAILURE;
    }

    FD_SET(conn_sock_fd, &conn_ctxt->read_fds);
    conn_ctxt->max_fd = MAX(conn_ctxt->max_fd, conn_sock_fd);
    conn_ctxt->conn_sockets[conn_ctxt->conn_sockets_count++] = conn_sock_fd;

    LOG_INFO(w->logger, "connection_handler_new_connection_handle: new client connected, socket_fd %d", conn_sock_fd);

    return EXIT_SUCCESS;
}

static inline void connection_handler_cleanup(conn_ctxt_t* conn_ctxt)
{
    if (IS_NULL(conn_ctxt))
        return EXIT_FAILURE;

    for (int i = 0; i < conn_ctxt->conn_sockets_count; i++)
        close(conn_ctxt->conn_sockets[i]);
}

static inline char* connection_handler_recieve_request(worker_info_t* w, int conn_sock_fd)
{
    if (IS_NULL(w))
        return NULL;

    ssize_t bytes_read;
    char request[DEFAULT_REQUEST_LENGTH];
    while ((bytes_read = recv(conn_sock_fd, request, sizeof(request), MSG_DONTWAIT)) > 0) {
        LOG_DEBUG(w->logger, "connection_handler_recieve_request: request text - %.*s", (int)bytes_read, request);
    }

    if (bytes_read == 0)
        return NULL;
    
    if (IS_ERROR(bytes_read) && errno != EAGAIN && errno != EWOULDBLOCK) {
        LOG_ERROR(w->logger, "connection_handler_recieve_request error: %s", strerror(errno));
        return NULL;
    }

    return dup_string(request);
}

static inline error_t connection_handler_send_response_by_status(worker_info_t* w, http_status_code_t status, 
    file_read_result_t* fr_result, int conn_sock_fd)
{
    if (IS_NULL(w))
        return EXIT_FAILURE;

    http_response_t* http_response = http_response_create_by_status(status, fr_result);
    if (IS_NULL(http_response)) {
        LOG_ERROR(w->logger, "connection_handler_send_response_by_status error: http response create failed");
        return EXIT_FAILURE;
    }

    char* http_response_text = http_response_build(http_response);
    if (IS_NULL(http_response_text)) {
        LOG_ERROR(w->logger, "connection_handler_send_response_by_status error: http response build failed");
        http_response_free(http_response);
        return EXIT_FAILURE;
    }

    ssize_t bytes_sent;
    size_t headers_length = strlen(http_response_text);
    size_t headers_sent = 0;
    while (headers_sent < headers_length) {
        bytes_sent = send(conn_sock_fd, http_response_text + headers_sent, 
                         headers_length - headers_sent, MSG_NOSIGNAL);
        if (IS_ERROR(bytes_sent)) {
            LOG_ERROR(w->logger, "connection_handler_send_response_by_status error: http response send failed");
            free(http_response_text);
            return EXIT_FAILURE;
        }
        headers_sent += bytes_sent;
    }

    if (!IS_NULL(http_response->body) && http_response->body_length > 0) {
        size_t body_sent = 0;
        while (body_sent < http_response->body_length) {
            bytes_sent = send(conn_sock_fd, http_response->body + body_sent, MIN(2 * 8128, http_response->body_length - body_sent), MSG_NOSIGNAL);
            if (IS_ERROR(bytes_sent)) {
                LOG_ERROR(w->logger, "connection_handler_send_response_by_status error: http response body send failed");
                free(http_response_text);
                return EXIT_FAILURE;
            }
            body_sent += bytes_sent;
        }
        LOG_DEBUG(w->logger, "connection_handler_send_response: sent %zu/%zu bytes of body", body_sent, http_response->body_length);
    }
    
    http_response_free(http_response);
    free(http_response_text);

    return EXIT_SUCCESS;
}

static inline error_t connection_handler_send_response(worker_info_t* w, http_response_t* http_response, int conn_sock_fd)
{
    if (IS_NULL(w))
        return EXIT_FAILURE;

    if (IS_NULL(http_response))
        return EXIT_FAILURE;

    char* http_response_text = http_response_build(http_response);
    if (IS_NULL(http_response_text)) {
        LOG_ERROR(w->logger, "connection_handler_send_response error: http response build failed");
        return EXIT_FAILURE;
    }

    LOG_DEBUG(w->logger, "connection_handler_send_response: http_response_text - %s", http_response_text);

    ssize_t bytes_sent;
    size_t headers_length = strlen(http_response_text);
    size_t headers_sent = 0;
    while (headers_sent < headers_length) {
        bytes_sent = send(conn_sock_fd, http_response_text + headers_sent, 
                         headers_length - headers_sent, MSG_NOSIGNAL);
        if (IS_ERROR(bytes_sent) && errno != EAGAIN && errno != EWOULDBLOCK) {
            LOG_ERROR(w->logger, "connection_handler_send_response error: http response send failed");
            free(http_response_text);
            return EXIT_FAILURE;
        }
        headers_sent += bytes_sent;
    }

    LOG_DEBUG(w->logger, "connection_handler_send_response: body length - %d", http_response->body_length);

    if (!IS_NULL(http_response->body) && http_response->body_length > 0) {
        size_t body_sent = 0;
        while (body_sent < http_response->body_length) {
            bytes_sent = send(conn_sock_fd, http_response->body + body_sent, MIN(2 * 8128, http_response->body_length - body_sent), MSG_NOSIGNAL);
            if (IS_ERROR(bytes_sent) && errno != EAGAIN && errno != EWOULDBLOCK) {
                LOG_ERROR(w->logger, "connection_handler_send_response error: http response body send failed");
                free(http_response_text);
                return EXIT_FAILURE;
            }
            body_sent += bytes_sent;
        }
        LOG_DEBUG(w->logger, "connection_handler_send_response: sent %zu/%zu bytes of body", body_sent, http_response->body_length);
    }

    free(http_response_text);
    return EXIT_SUCCESS;
}

static inline error_t connection_handler_http_request_handle(worker_info_t* w, int conn_sock_fd)
{
    if (IS_NULL(w))
        return NULL;
    
    char* request = connection_handler_recieve_request(w, conn_sock_fd);
    if (IS_NULL(request)) {
        LOG_ERROR(w->logger, "connection_handler_http_request_handle error: recieve request failed");
        return EXIT_FAILURE;
    }

    LOG_INFO(w->logger, "connection_handler_http_request_handle: request from sock_fd %d succesfully recieved", conn_sock_fd);

    http_request_t* http_request = http_request_parse(w, request);
    if (IS_NULL(http_request)) {
        LOG_ERROR(w->logger, "connection_handler_http_request_handle error: parse request failed");
        if (IS_ERROR(connection_handler_send_response_by_status(w, HTTP_BAD_REQUEST, NULL, conn_sock_fd))) 
            LOG_ERROR(w->logger, "connection_handler_http_request_handle error: response send failed");
        
        free(request);
        return EXIT_FAILURE;
    }

    LOG_INFO(w->logger, "connection_handler_http_request_handle: request from sock_fd %d succesfully parsed", conn_sock_fd);
    LOG_DEBUG(w->logger, "connection_handler_http_request_handle: request method:  \"%s\"", http_request->method);
    LOG_DEBUG(w->logger, "connection_handler_http_request_handle: request path:    \"%s\"", http_request->path);
    LOG_DEBUG(w->logger, "connection_handler_http_request_handle: request version: \"%s\"", http_request->version);
    for (headers_list_t* cur = http_request->headers; !IS_NULL(cur); cur = cur->next)
        LOG_DEBUG(w->logger, "connection_handler_http_request_handle: request header:  \"%s: %s\"", cur->header, cur->value);

    http_response_t* http_response = http_response_create_by_request(http_request);
    if (IS_NULL(http_response)) {
        LOG_ERROR(w->logger, "connection_handler_http_request_handle error: create response failed");
        if (IS_ERROR(connection_handler_send_response_by_status(w, HTTP_INTERNAL_ERROR, NULL, conn_sock_fd))) 
            LOG_ERROR(w->logger, "connection_handler_http_request_handle error: response send failed");
        
        free(request);
        http_request_free(http_request);
        return EXIT_FAILURE;
    }

    LOG_INFO(w->logger, "connection_handler_http_request_handle: response for sock_fd %d succesfully created", conn_sock_fd);

    if (IS_ERROR(connection_handler_send_response(w, http_response, conn_sock_fd))) {
        LOG_ERROR(w->logger, "connection_handler_http_request_handle error: response send failed");
        free(request);
        http_request_free(http_request);
        http_response_free(http_response);
        return EXIT_FAILURE;
    }

    LOG_INFO(w->logger, "connection_handler_http_request_handle: response for sock_fd %d succesfully has been sent", conn_sock_fd);

    free(request);
    http_request_free(http_request);
    http_response_free(http_response);
    return EXIT_SUCCESS;
}

static inline bool is_connection_alive(int sockfd)
{
    char buf[1];
    ssize_t result = recv(sockfd, buf, sizeof(buf), MSG_PEEK | MSG_DONTWAIT);
    
    if (result == 0) {
        return false; // Соединение закрыто
    } else if (result < 0) {
        if (errno == EAGAIN || errno == EWOULDBLOCK) {
            return true; // Данных нет, но соединение активно
        }
        return false; // Ошибка соединения
    }
    
    return true; // Есть данные для чтения, соединение активно
}

static inline error_t connection_handler_connection_delete(conn_ctxt_t* conn_ctxt, int conn_sock_fd)
{
    if (IS_NULL(conn_ctxt))
        return EXIT_FAILURE;

    FD_CLR(conn_sock_fd, &conn_ctxt->read_fds);
    for (int i = 0; i < conn_ctxt->conn_sockets_count; i++) {
        if (conn_ctxt->conn_sockets[i] == conn_sock_fd) {
            for (int j = i; j < conn_ctxt->conn_sockets_count - 1; j++) {
                conn_ctxt->conn_sockets[j] = conn_ctxt->conn_sockets[j + 1];
            }
            conn_ctxt->conn_sockets_count--;
            break;
        }
    }

    return EXIT_SUCCESS;
}

static inline error_t connection_handler_connection_activity_handle(worker_info_t* w, conn_ctxt_t* conn_ctxt, int conn_sock_fd, fd_set* curr_fds)
{
    if (IS_NULL(w))
        return EXIT_FAILURE;

    if (IS_NULL(conn_ctxt))
        return EXIT_FAILURE;

    if (!is_connection_alive(conn_sock_fd)) {
        LOG_INFO(w->logger, "connection_handler_connection_activity_handle: connection closed by client: fd %d", conn_sock_fd);
        close(conn_sock_fd);

        if (IS_ERROR(connection_handler_connection_delete(conn_ctxt, conn_sock_fd))) {
            LOG_ERROR(w->logger, "connection_handler_connection_activity_handle: failed to delete connection");
            return EXIT_FAILURE;
        }

        return EXIT_SUCCESS;
    }

    if (FD_ISSET(conn_sock_fd, curr_fds)) {
        if (IS_ERROR(connection_handler_http_request_handle(w, conn_sock_fd))) {
            LOG_ERROR(w->logger, "connection_handler_connection_activity_handle: failed to handle http request from socket_fd %d", conn_sock_fd);
            
            close(conn_sock_fd);
            if (IS_ERROR(connection_handler_connection_delete(conn_ctxt, conn_sock_fd))) {
                LOG_ERROR(w->logger, "connection_handler_connection_activity_handle: failed to delete connection");
                return EXIT_FAILURE;
            }
            
            return EXIT_FAILURE;
        }

        close(conn_sock_fd);
        if (IS_ERROR(connection_handler_connection_delete(conn_ctxt, conn_sock_fd))) {
            LOG_ERROR(w->logger, "connection_handler_connection_activity_handle: failed to delete connection");
            return EXIT_FAILURE;
        }

        return EXIT_SUCCESS;
    }

    return EXIT_SUCCESS;
}

static inline error_t connection_handler_connections_activity_handle(worker_info_t* w, conn_ctxt_t* conn_ctxt, fd_set* curr_fds)
{
    if (IS_NULL(w))
        return EXIT_FAILURE;

    if (IS_NULL(conn_ctxt))
        return EXIT_FAILURE;

    for (int i = 0; i < conn_ctxt->conn_sockets_count; i++) {
        if (IS_ERROR(connection_handler_connection_activity_handle(w, conn_ctxt, conn_ctxt->conn_sockets[i], curr_fds))) {
            LOG_ERROR(w->logger, "connection_handler_connections_activity_handle: failed to handle connection activity from socket_fd %d", conn_ctxt->conn_sockets[i]);
            continue;
        }
    }

    return EXIT_SUCCESS;
}

error_t connection_handler_listening(worker_info_t* worker)
{
    if (IS_NULL(worker))
        return EXIT_FAILURE;

    conn_ctxt_t conn_ctxt = CONNECTION_HANDLER_CONTEXT__;
    int from_master_fd = worker->to_worker_channel[0];

    FD_ZERO(&conn_ctxt.read_fds);
    FD_SET(from_master_fd, &conn_ctxt.read_fds);
    conn_ctxt.max_fd = from_master_fd;
    
    while (true) {
        fd_set curr_fds = conn_ctxt.read_fds;

        if (IS_ERROR(select(conn_ctxt.max_fd + 1, &curr_fds, NULL, NULL, NULL))) {
            LOG_ERROR(worker->logger, "connection_handler_listening: select error, %s", strerror(errno));
            continue;
        }

        if (FD_ISSET(from_master_fd, &curr_fds)) {
            if (IS_ERROR(connection_handler_new_connection_handle(worker, &conn_ctxt))) {
                LOG_ERROR(worker->logger, "connection_handler_listening: failed to handle new connection");
                continue;
            }
        }

        if (IS_ERROR(connection_handler_connections_activity_handle(worker, &conn_ctxt, &curr_fds))) {
            LOG_ERROR(worker->logger, "connection_handler_listening: failed to handle connections activity");
            continue;
        }
    }
    
    LOG_WARNING(worker->logger, "connection_handler_listening: worker with PID %d finished", worker->pid);
    connection_handler_cleanup(&conn_ctxt);

    return EXIT_SUCCESS;
}