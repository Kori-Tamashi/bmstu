#include "response_builder.h"

static inline headers_list_t* headers_list_create()
{
    headers_list_t* h_list = (headers_list_t*)malloc(sizeof(headers_list_t));
    if (IS_NULL(h_list))
        return NULL;

    memset(h_list, 0, sizeof(headers_list_t));
    return h_list;
}

static inline headers_list_t* headers_list_create_with_data(char* header, char* value)
{
    if (IS_NULL(header))
        return EXIT_FAILURE;

    if (IS_NULL(value))
        return EXIT_FAILURE;

    headers_list_t* h_list = (headers_list_t*)malloc(sizeof(headers_list_t));
    if (IS_NULL(h_list))
        return NULL;

    h_list->header = dup_string(header);
    if (IS_NULL(h_list->header)) {
        free(h_list);
        return NULL;
    }

    h_list->value = dup_string(value);
    if (IS_NULL(h_list->value)) {
        free(h_list);
        free(h_list->header);
        return NULL;
    }
    
    h_list->next = NULL;

    return h_list;
}

static inline void headers_list_free(headers_list_t* h_list)
{
    if (IS_NULL(h_list))
        return;

    headers_list_free(h_list->next);

    if (!IS_NULL(h_list->header))
        free(h_list->header);

    if (!IS_NULL(h_list->value))
        free(h_list->value);

    free(h_list);
}

static inline error_t headers_list_add(headers_list_t** h_list, headers_list_t* node)
{
    if (IS_NULL(h_list))
        return EXIT_FAILURE;

    if (IS_NULL(node))
        return EXIT_FAILURE;

    if (IS_NULL(*h_list)) {
        *h_list = node;
    } else {
        headers_list_t* current = *h_list;
        while (!IS_NULL(current->next))
            current = current->next;
        current->next = node;
    }

    if (!IS_NULL(node->next))
        node->next = NULL;

    return EXIT_SUCCESS;
}

static inline error_t headers_list_add_with_data(headers_list_t** h_list, char* header, char* value)
{
    if (IS_NULL(h_list))
        return EXIT_FAILURE;

    if (IS_NULL(header))
        return EXIT_FAILURE;

    if (IS_NULL(value))
        return EXIT_FAILURE;

    headers_list_t* node = headers_list_create_with_data(header, value);
    if (IS_NULL(node))
        return EXIT_FAILURE;

    return headers_list_add(h_list, node);
}

static inline error_t headers_list_remove(headers_list_t** h_list, char* header) 
{
    if (IS_NULL(h_list))
        return EXIT_FAILURE;

    if (IS_NULL(*h_list))
        return EXIT_FAILURE;

    headers_list_t* current = *h_list;
    headers_list_t* prev = NULL;

    while (!IS_NULL(current)) {
        bool match = false;

        if (IS_NULL(header) && IS_NULL(current->header)) {
            match = true;
        } else if (!IS_NULL(header) && !IS_NULL(current->header) && 
                   strcmp(header, current->header) == 0) {
            match = true;
        }

        if (match) {
            if (IS_NULL(prev)) 
                *h_list = current->next;
            else 
                prev->next = current->next;

            headers_list_free(current);
            return EXIT_SUCCESS;
        }

        prev = current;
        current = current->next;
    }

    return EXIT_SUCCESS;
}

http_response_t* http_response_create()
{
    http_response_t* response = (http_response_t*)malloc(sizeof(http_response_t));
    if (IS_NULL(response))
        return NULL;

    memset(response, 0, sizeof(http_response_t));
    response->headers = NULL;
    response->body = NULL;

    return response;
}

void http_response_free(http_response_t* response)
{
    if (IS_NULL(response))
        return;

    if (!IS_NULL(response->headers))
        headers_list_free(response->headers);

    if (!IS_NULL(response->body))
        free(response->body);

    free(response);
}

error_t http_response_add_header(http_response_t* response, char* header, char* value)
{
    if (IS_NULL(response))
        return EXIT_FAILURE;

    if (IS_NULL(header))
        return EXIT_FAILURE;

    if (IS_NULL(value))
        return EXIT_FAILURE;

    if (IS_ERROR(headers_list_add_with_data(&response->headers, header, value)))
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}

static inline const char* http_status_message(http_status_code_t code)
{
    switch (code) {
        case HTTP_OK: return "OK";
        case HTTP_NO_CONTENT: return "No Content";
        case HTTP_BAD_REQUEST: return "Bad Request";
        case HTTP_FORBIDDEN: return "Forbidden";
        case HTTP_NOT_FOUND: return "Not Found";
        case HTTP_METHOD_NOT_ALLOWED: return "Method Not Allowed";
        case HTTP_URI_TOO_LONG: return "URI Too Long";
        case HTTP_INTERNAL_ERROR: return "Internal Server Error";
        default: return "Unknown Status";
    }
}

http_response_t* http_response_create_ok(file_read_result_t* file_result)
{
    if (IS_NULL(file_result)) 
        return NULL;

    http_response_t* response = http_response_create();
    if (IS_NULL(response))
        return NULL;

    response->status_code = HTTP_OK;

    if (!IS_NULL(file_result->content)) {
        response->body_length = file_result->content_length;
        response->body = (char*)malloc(file_result->content_length);
        if (IS_NULL(response->body)) {
            http_response_free(response);
            return NULL;
        }
        memcpy(response->body, file_result->content, file_result->content_length);
    } 

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }
    
    if (!IS_NULL(file_result->content_type)) {
        if (IS_ERROR(http_response_add_header(response, "Content-Type", file_result->content_type))) {
            http_response_free(response);
            return NULL;
        }   
    }
    
    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", file_result->content_length);
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    if (file_result->is_directory) {
        if (IS_ERROR(http_response_add_header(response, "Is-Directory", "true"))) {
            http_response_free(response);
            return NULL;
        }
    }

    return response;
}

http_response_t* http_response_create_no_content() 
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response)) 
        return NULL;

    response->status_code = HTTP_NO_CONTENT;
    
    // 204 No Content не должен иметь тела сообщения
    response->body_length = 0;
    response->body = NULL;

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Length", "0"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_bad_request() 
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response)) 
        return NULL;

    response->status_code = HTTP_BAD_REQUEST;
    
    const char* bad_request_html = 
        "<!DOCTYPE html>"
        "<html>"
        "<head><title>400 Bad Request</title></head>"
        "<body>"
        "<h1>400 Bad Request</h1>"
        "<p>The server cannot process the request due to a client error.</p>"
        "</body>"
        "</html>";
    
    response->body_length = strlen(bad_request_html);
    response->body = strdup(bad_request_html);
    if (IS_NULL(response->body)) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Type", "text/html"))) {
        http_response_free(response);
        return NULL;
    }

    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", strlen(bad_request_html));
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_forbidden()
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response)) 
        return NULL;

    response->status_code = HTTP_FORBIDDEN;
    
    const char* forbidden_html = 
        "<!DOCTYPE html>"
        "<html>"
        "<head><title>403 Forbidden</title></head>"
        "<body>"
        "<h1>403 Forbidden</h1>"
        "<p>You don't have permission to access this resource.</p>"
        "</body>"
        "</html>";
    
    response->body_length = strlen(forbidden_html);
    response->body = strdup(forbidden_html);
    if (IS_NULL(response->body)) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Type", "text/html"))) {
        http_response_free(response);
        return NULL;
    }

    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", strlen(forbidden_html));
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_not_found()
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response))
        return NULL;

    response->status_code = HTTP_NOT_FOUND;
    
    const char* not_found_html = 
        "<!DOCTYPE html>"
        "<html>"
        "<head><title>404 Not Found</title></head>"
        "<body>"
        "<h1>404 Not Found</h1>"
        "<p>The requested resource was not found on this server.</p>"
        "</body>"
        "</html>";
    
    response->body_length = strlen(not_found_html);
    response->body = strdup(not_found_html);
    if (IS_NULL(response->body)) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Type", "text/html"))) {
        http_response_free(response);
        return NULL;
    }

    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", strlen(not_found_html));
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_method_not_allowed()
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response)) 
        return NULL;

    response->status_code = HTTP_METHOD_NOT_ALLOWED;
    
    const char* method_not_allowed_html = 
        "<!DOCTYPE html>"
        "<html>"
        "<head><title>405 Method Not Allowed</title></head>"
        "<body>"
        "<h1>405 Method Not Allowed</h1>"
        "<p>The requested method is not allowed for this resource.</p>"
        "</body>"
        "</html>";
    
    response->body_length = strlen(method_not_allowed_html);
    response->body = strdup(method_not_allowed_html);
    if (IS_NULL(response->body)) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Type", "text/html"))) {
        http_response_free(response);
        return NULL;
    }

    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", strlen(method_not_allowed_html));
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    // Для 405 рекомендуется добавлять заголовок Allow с разрешенными методами
    if (IS_ERROR(http_response_add_header(response, "Allow", "GET, HEAD"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_uri_too_long()
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response)) 
        return NULL;

    response->status_code = HTTP_URI_TOO_LONG;
    
    const char* uri_too_long_html = 
        "<!DOCTYPE html>"
        "<html>"
        "<head><title>414 URI Too Long</title></head>"
        "<body>"
        "<h1>414 URI Too Long</h1>"
        "<p>The requested URI is too long for the server to process.</p>"
        "</body>"
        "</html>";
    
    response->body_length = strlen(uri_too_long_html);
    response->body = strdup(uri_too_long_html);
    if (IS_NULL(response->body)) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Type", "text/html"))) {
        http_response_free(response);
        return NULL;
    }

    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", strlen(uri_too_long_html));
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_internal_error()
{
    http_response_t* response = http_response_create();
    if (IS_NULL(response)) 
        return NULL;

    response->status_code = HTTP_INTERNAL_ERROR;
    
    const char* error_html = 
        "<!DOCTYPE html>"
        "<html>"
        "<head><title>500 Internal Server Error</title></head>"
        "<body>"
        "<h1>500 Internal Server Error</h1>"
        "<p>An internal server error occurred.</p>"
        "</body>"
        "</html>";
    
    response->body_length = strlen(error_html);
    response->body = strdup(error_html);
    if (IS_NULL(response->body)) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Server", "Simple HTTP Server"))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Content-Type", "text/html"))) {
        http_response_free(response);
        return NULL;
    }

    char content_length[32];
    snprintf(content_length, sizeof(content_length), "%zu", strlen(error_html));
    if (IS_ERROR(http_response_add_header(response, "Content-Length", content_length))) {
        http_response_free(response);
        return NULL;
    }

    if (IS_ERROR(http_response_add_header(response, "Connection", "close"))) {
        http_response_free(response);
        return NULL;
    }

    return response;
}

http_response_t* http_response_create_by_status(http_status_code_t status, file_read_result_t* file_result)
{
    switch (status)
    {
    case HTTP_OK:
        return http_response_create_ok(file_result);
        break;
    case HTTP_NO_CONTENT:
        return http_response_create_no_content();
        break;
    case HTTP_BAD_REQUEST:
        return http_response_create_bad_request();
        break;
    case HTTP_FORBIDDEN:
        return http_response_create_forbidden();
        break;
    case HTTP_NOT_FOUND:
        return http_response_create_not_found();
        break;
    case HTTP_METHOD_NOT_ALLOWED:
        return http_response_create_method_not_allowed();
        break;
    case HTTP_URI_TOO_LONG:
        return http_response_create_uri_too_long();
        break;
    case HTTP_INTERNAL_ERROR:
        return http_response_create_internal_error();
        break;
    default:
        return NULL;
        break;
    }
}

http_status_code_t http_response_secure_request(http_request_t* http_request)
{
    if (IS_NULL(http_request))
        return HTTP_INTERNAL_ERROR;

    security_config_t* s_config = security_config_create_with_data(DEFAULT_BASE_DIR, DEFAULT_MAX_FILE_SIZE, DEFAULT_URI_LENGTH);
    if (IS_NULL(s_config))
        return HTTP_INTERNAL_ERROR;

    if (IS_ERROR(security_check_method(s_config, http_request->method))) {
        security_config_free(s_config);
        return HTTP_METHOD_NOT_ALLOWED;
    }

    if (IS_ERROR(security_check_uri_length(s_config, http_request->path))) {
        security_config_free(s_config);
        return HTTP_URI_TOO_LONG;
    }

    char* full_path = absolute_path(DEFAULT_BASE_DIR, http_request->path);
    if (IS_NULL(full_path)) {
        security_config_free(s_config);
        return HTTP_INTERNAL_ERROR;
    }

    if (!IS_ERROR(security_check_file_exists(full_path))) {
        if (IS_ERROR(security_check_path_traversal(s_config, http_request->path))) {
            security_config_free(s_config);
            return HTTP_FORBIDDEN;
        }

        if (IS_ERROR(security_check_file_access(full_path))) {
            free(full_path);
            security_config_free(s_config);
            return HTTP_FORBIDDEN;
        }

        if (IS_ERROR(security_check_file_emptiness(full_path))) {
            free(full_path);
            security_config_free(s_config);
            return HTTP_NO_CONTENT;
        }

        if (IS_ERROR(security_check_file_size(full_path))) {
            free(full_path);
            security_config_free(s_config);
            return HTTP_PAYLOAD_TOO_LARGE;
        }
        
    } else if (!IS_ERROR(security_check_directory_exists(full_path))) {
        if (IS_ERROR(security_check_path_traversal(s_config, http_request->path))) {
            security_config_free(s_config);
            return HTTP_FORBIDDEN;
        }
        
        if (IS_ERROR(security_check_directory_access(full_path))) {
            free(full_path);
            security_config_free(s_config);
            return HTTP_FORBIDDEN;
        }
    } else {
        free(full_path);
        security_config_free(s_config);
        return HTTP_NOT_FOUND;
    }

    free(full_path);
    security_config_free(s_config);
    return HTTP_OK;
}

http_response_t* http_response_create_get(http_request_t* http_request)
{
    if (IS_NULL(http_request))
        return NULL;
        
    char* path = absolute_path(DEFAULT_BASE_DIR, http_request->path);
    if (IS_NULL(path))
        return http_response_create_by_status(HTTP_INTERNAL_ERROR, NULL);
    
    file_read_result_t* fr_result = file_reader_get(path);
    if (IS_NULL(fr_result)) {
        free(path);
        return http_response_create_by_status(HTTP_INTERNAL_ERROR, NULL);
    }
        
    http_response_t* http_response = http_response_create_by_status(HTTP_OK, fr_result);

    free(path);
    file_read_result_free(fr_result);
    return http_response;
}

http_response_t* http_response_create_head(http_request_t* http_request)
{
    if (IS_NULL(http_request))
        return NULL;

    char* path = absolute_path(DEFAULT_BASE_DIR, http_request->path);
    if (IS_NULL(path))
        return http_response_create_by_status(HTTP_INTERNAL_ERROR, NULL);
    
    file_read_result_t* fr_result = file_reader_get(path);
    if (IS_NULL(fr_result)) {
        free(path);
        return http_response_create_by_status(HTTP_INTERNAL_ERROR, NULL);
    }

    if (!IS_NULL(fr_result->content)) {
        free(fr_result->content);
        fr_result->content = NULL;
    }

    http_response_t* http_response = http_response_create_by_status(HTTP_OK, fr_result);

    free(path);
    file_read_result_free(fr_result);
    return http_response;
}

http_response_t* http_response_create_by_request(http_request_t* http_request)
{
    if (IS_NULL(http_request))
        return NULL;

    http_status_code_t response_status = http_response_secure_request(http_request);

    if (response_status == HTTP_OK) {
        if (IS_EQUAL_STR(http_request->method, "GET")) 
            return http_response_create_get(http_request);
        else if (IS_EQUAL_STR(http_request->method, "HEAD"))
            return http_response_create_head(http_request);
        else
            return NULL;
    } 

    return http_response_create_by_status(response_status, NULL);
}

static inline char* http_build_status(http_response_t* response)
{
    if (IS_NULL(response))
        return NULL;

    const char* status_message = http_status_message(response->status_code);
    size_t line_length = snprintf(NULL, 0, "HTTP/1.1 %d %s", response->status_code, status_message);

    char* status_line = (char*)malloc(line_length + 1);
    if (IS_NULL(status_line)) 
        return NULL;
    
    snprintf(status_line, line_length + 1, "HTTP/1.1 %d %s", 
             response->status_code, status_message);

    return status_line;
}

static inline char* http_build_headers(http_response_t* response)
{
    if (IS_NULL(response))
        return NULL;

    headers_list_t* current;
    size_t headers_length = 0;
    
    current = response->headers;
    while (!IS_NULL(current)) {
        headers_length += strlen(current->header) + strlen(current->value) + 4; // +4 для ": " и "\r\n"
        current = current->next;
    }
    
    char* headers_str = (char*)malloc(headers_length + 1);
    if (IS_NULL(headers_str)) 
        return NULL;
    
    char* ptr = headers_str;
    current = response->headers;
    while (!IS_NULL(current)) {
        int written = snprintf(ptr, headers_length - (ptr - headers_str) + 4, 
                              "%s: %s\r\n", current->header, current->value);
        ptr += written;
        current = current->next;
    }

    return headers_str;
}

char* http_response_build(http_response_t* response)
{
    if (IS_NULL(response))
        return NULL;

    // Формируем строки
    char* status_msg = http_build_status(response);
    if (IS_NULL(status_msg))
        return NULL;

    char* headers_msg = http_build_headers(response);
    if (IS_NULL(headers_msg)) {
        free(status_msg);
        return NULL;
    }

    // Объединяем строки
    size_t length = strlen(status_msg) + strlen(headers_msg) + 4;

    char* response_msg = (char*)malloc(length + 1);
    if (IS_NULL(response_msg))
        return NULL;

    snprintf(response_msg, length + 1, "%s\r\n%s\r\n", status_msg, headers_msg);

    free(status_msg);
    free(headers_msg);

    return response_msg;
}

