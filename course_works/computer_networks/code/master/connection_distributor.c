#include "connection_distributor.h"

#define CONNECTION connection_config_init()
#define D_SOCKADDR distributor_sockaddr_in_init()
#define D_SOCKINFO distributor_sockaddr_info_init()
#define D_CONFIG   distributor_config_init()
#define D_SOCKET   distributor_socket_create(D_CONFIG)

static inline conn_cfg_t connection_config_init()
{
    return (conn_cfg_t) {
        .conn_sock = 0,
        .addr_info = {0}
    };
}

static inline struct sockaddr_in distributor_sockaddr_in_init()
{
    return (struct sockaddr_in) {
        .sin_family = AF_INET,
        .sin_port = htons(SERVER_PORT),
        .sin_addr.s_addr = INADDR_ANY,
    };
}

static inline sockaddr_info_t distributor_sockaddr_info_init()
{
    return (sockaddr_info_t) {
        .addr = D_SOCKADDR,
        .addr_len = sizeof(D_SOCKADDR)
    };
}

static inline distributor_cfg_t distributor_config_init()
{
    return (distributor_cfg_t) {
        .listen_port = SERVER_PORT,
        .addr_info = D_SOCKINFO
    };
}

static inline int distributor_socket_create(distributor_cfg_t d_cfg, logger_t* logger)
{
    int listen_sock = socket(AF_INET, SOCK_STREAM, 0);
    if (IS_ERROR(listen_sock)) {
        LOG_ERROR(logger, "distributor_socket_create error: failed to create socket: %s", strerror(errno));
        return EXIT_FAILURE;
    }

    if (IS_ERROR(bind(listen_sock, 
        (struct sockaddr *) &d_cfg.addr_info.addr, d_cfg.addr_info.addr_len))) {
        LOG_ERROR(logger, "distributor_socket_create error: failed to bind socket: %s", strerror(errno));
        close(listen_sock);
        return EXIT_FAILURE;
    }

    if (IS_ERROR(listen(listen_sock, MAX_CONNECTIONS))) {
        LOG_ERROR(logger, "distributor_socket_create error: failed to listen on socket: %s", strerror(errno));
        close(listen_sock);
        return EXIT_FAILURE;
    }

    LOG_INFO(logger, "distributor_socket_create: socket created successfully on port %d", SERVER_PORT);
    return listen_sock;
}

distributor_ctxt_t* distributor_ctxt_create(logger_t* logger)
{
    if (IS_NULL(logger)) 
        return NULL;

    distributor_ctxt_t* d_ctxt = (distributor_ctxt_t *)malloc(sizeof(distributor_ctxt_t));
    if (IS_NULL(d_ctxt)) {
        LOG_ERROR(logger, "distributor_ctxt_create error: malloc returned NULL");
        return NULL;
    }

    d_ctxt->logger = logger;
    d_ctxt->config = D_CONFIG;
    d_ctxt->listen_sock = distributor_socket_create(D_CONFIG, logger);

    if (IS_ERROR(d_ctxt->listen_sock)) {
        LOG_ERROR(logger, "distributor_ctxt_create error: failed to create socket");
        free(d_ctxt);
        return NULL;
    }

    LOG_INFO(logger, "distributor_ctxt_create: connection distributor created successfully");
    return d_ctxt;
}

void distributor_ctxt_free(distributor_ctxt_t* d_ctxt)
{
    if (IS_NULL(d_ctxt))
        return;

    if (d_ctxt->listen_sock >= 0) {
        close(d_ctxt->listen_sock);
        LOG_DEBUG(d_ctxt->logger, "distributor_ctxt_free: closed listen socket");
    }

    free(d_ctxt);
    LOG_DEBUG(d_ctxt->logger, "distributor_ctxt_free: freed distributor context");
}

error_t distributor_accept_connection(distributor_ctxt_t* d_ctxt, conn_cfg_t* conn)
{
    if (IS_NULL(d_ctxt) || IS_NULL(conn)) {
        LOG_ERROR(d_ctxt->logger, "distributor_accept_connection error: invalid arguments");
        return EXIT_FAILURE;
    }

    conn->conn_sock = accept(d_ctxt->listen_sock, 
        (struct sockaddr *) &conn->addr_info.addr, 
        &conn->addr_info.addr_len);

    if (IS_ERROR(conn->conn_sock)) {
        if (errno != EAGAIN && errno != EWOULDBLOCK) {
            LOG_ERROR(d_ctxt->logger, "distributor_accept_connection error: accept failed: %s", strerror(errno));
        }
        return EXIT_FAILURE;
    }

    char client_ip[INET_ADDRSTRLEN];
    inet_ntop(AF_INET, &(conn->addr_info.addr.sin_addr), client_ip, INET_ADDRSTRLEN);
    LOG_INFO(d_ctxt->logger, "distributor_accept_connection: accepted connection from %s:%d (socket_fd %d)", 
             client_ip, ntohs(conn->addr_info.addr.sin_port), conn->conn_sock);

    return EXIT_SUCCESS;
}

static inline error_t distributor_conn_delegate(conn_cfg_t* conn, worker_pool_t* wp)
{
    if (IS_NULL(wp)) 
        return EXIT_FAILURE;

    if (IS_NULL(conn)) {
        LOG_ERROR(wp->logger, "distributor_conn_delegate error: %s", "conn is NULL");
        return EXIT_FAILURE;
    }

    worker_info_t* w_info = worker_pool_get_worker(wp);
    if (IS_NULL(w_info)) {
        LOG_DEBUG(wp->logger, "distributor_conn_delegate: no idle workers available");
        
        if (worker_pool_is_max_workers(wp)) {
            LOG_WARNING(wp->logger, "distributor_conn_delegate: maximum workers reached, cannot create more");
            return EXIT_FAILURE;
        }

        LOG_INFO(wp->logger, "distributor_conn_delegate: creating new worker");
        if (IS_ERROR(worker_pool_add_worker(wp))) {
            LOG_ERROR(wp->logger, "distributor_conn_delegate: failed to add new worker");
            return EXIT_FAILURE;
        }

        w_info = worker_pool_get_worker(wp);
        if (IS_NULL(w_info)) {
            LOG_ERROR(wp->logger, "distributor_conn_delegate: failed to get idle worker after creation");
            return EXIT_FAILURE;
        }
    }
    
    if (IS_ERROR(send_fd(w_info->to_worker_channel[1], conn->conn_sock))) {
        LOG_ERROR(wp->logger, "distributor_conn_delegate: failed to send fd to worker %d", w_info->pid);
        return EXIT_FAILURE;
    }
    
    LOG_INFO(wp->logger, "distributor_conn_delegate: delegated connection to worker with PID %d", w_info->pid);
        
    return EXIT_SUCCESS;
}

error_t distributor_listening(distributor_ctxt_t* d_ctxt, worker_pool_t* wp)
{
    if (IS_NULL(d_ctxt) || IS_NULL(wp)) {
        LOG_ERROR(d_ctxt->logger, "distributor_listening error: invalid arguments");
        return EXIT_FAILURE;
    }

    LOG_INFO(d_ctxt->logger, "distributor_listening: starting connection distribution");
    
    while (true) {
        conn_cfg_t conn = CONNECTION;
        conn.addr_info.addr_len = sizeof(conn.addr_info.addr);

        if (IS_ERROR(distributor_accept_connection(d_ctxt, &conn))) {
            if (errno != EAGAIN && errno != EWOULDBLOCK) {
                LOG_WARNING(d_ctxt->logger, "distributor_listening: accept failed, continuing");
            }
            continue;
        }

        if (IS_ERROR(distributor_conn_delegate(&conn, wp))) {
            LOG_WARNING(d_ctxt->logger, "distributor_listening: failed to delegate connection, closing");
            close(conn.conn_sock);
            continue;
        }

        close(conn.conn_sock);
    }

    return EXIT_SUCCESS;
}
