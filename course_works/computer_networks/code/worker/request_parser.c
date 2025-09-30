#include "request_parser.h"

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

http_request_t* http_request_create()
{
    http_request_t* request = (http_request_t*)malloc(sizeof(http_request_t));
    if (IS_NULL(request))
        return NULL;

    memset(request, 0, sizeof(http_request_t));
    return request;
}

void http_request_free(http_request_t* request)
{
    if (IS_NULL(request))
        return;

    if (!IS_NULL(request->method))
        free(request->method);

    if (!IS_NULL(request->path))
        free(request->path);

    if (!IS_NULL(request->version))
        free(request->version);

    if (!IS_NULL(request->headers))
        headers_list_free(request->headers);

    if (!IS_NULL(request->body))
        free(request->body);
}


static error_t parse_request_title(http_request_t* request, const char* title_start, const char* title_end) 
{
    if (IS_NULL(request))
        return EXIT_FAILURE;
    
    if (IS_NULL(title_start))
        return EXIT_FAILURE;

    if (IS_NULL(title_end))
        return EXIT_FAILURE;

    if (title_start > title_end)
        return EXIT_FAILURE;

    const char* current = title_start;
    
    // Парсинг метода
    request->method = extract_string(current, title_end);
    if (IS_NULL(request->method)) {
        return EXIT_FAILURE;
    } else if (strlen(request->method) == 0) {
        free(request->method);
        return EXIT_FAILURE;
    }
    
    current = skip_space(current, title_end);
    current = skip_nonspace(current, title_end);
    
    // Парсинг пути
    request->path = extract_string(current, title_end);
    if (IS_NULL(request->path)) {
        free(request->method);
        return EXIT_FAILURE;
    } else if (strlen(request->path) == 0) {
        free(request->method);
        free(request->path);
        return EXIT_FAILURE;
    }
    
    current = skip_space(current, title_end);
    current = skip_nonspace(current, title_end);
    
    // Парсинг версии
    request->version = extract_string(current, title_end);
    if (IS_NULL(request->version)) {
        free(request->method);
        free(request->path);
        return EXIT_FAILURE;
    } else if (strlen(request->version) == 0) {
        free(request->method);
        free(request->path);
        free(request->version);
        return EXIT_FAILURE;
    }
    
    return EXIT_SUCCESS;
}

static inline error_t parse_request_headers_line(http_request_t* request, const char* line_start, const char* line_end) 
{
    if (IS_NULL(request))
        return EXIT_FAILURE;

    if (IS_NULL(line_start))
        return EXIT_FAILURE;

    if (IS_NULL(line_end))
        return EXIT_FAILURE;

    if (line_start > line_end)
        return EXIT_FAILURE;
    
    const char* colon = line_start;
    while (colon < line_end && *colon != ':') 
        colon++;
    
    if (*colon != ':') 
        return EXIT_FAILURE; 
    
    const char* header = extract_string(line_start, colon);
    if (IS_NULL(header)) {
        return EXIT_FAILURE;
    } else if (strlen(header) == 0) {
        free(header);
        return EXIT_FAILURE;
    }

    const char* value = extract_string(colon + 1, line_end);
    if (IS_NULL(value)) {
        free(header);
        return EXIT_FAILURE;
    }
        
    if (IS_ERROR(headers_list_add_with_data(&request->headers, header, value))) {
        free(header);
        free(value);
        return EXIT_FAILURE;
    }
    
    return EXIT_SUCCESS;
}

static error_t parse_request_headers(http_request_t* request, const char* headers_start, const char* headers_end) 
{
    if (IS_NULL(request))
        return EXIT_FAILURE;

    if (IS_NULL(headers_start))
        return EXIT_FAILURE;

    if (IS_NULL(headers_end))
        return EXIT_FAILURE;
    
    if (headers_start > headers_end)
        return EXIT_FAILURE; 
    
    const char* line_start = headers_start;

    while (line_start < headers_end) {
        const char* line_end = find_crlf(line_start, headers_end);
        if (IS_NULL(line_end)) 
            line_end = headers_end; // Последняя строка без CRLF
        
        if (IS_ERROR(parse_request_headers_line(request, line_start, line_end))) {
            headers_list_free(request->headers);
            return EXIT_FAILURE;
        }
            
        line_start = line_end + 2;
    }
    
    return EXIT_SUCCESS;
}

static error_t parse_request_body(http_request_t* request, const char* body_start, const char* body_end) 
{
    if (IS_NULL(request))
        return EXIT_FAILURE;

    if (IS_NULL(body_start))
        return EXIT_FAILURE;

    if (IS_NULL(body_end))
        return EXIT_FAILURE;
    
    if (body_start > body_end)
        return EXIT_FAILURE; 

    request->body = copy_string(body_start, body_end);
    if (IS_NULL(request->body))
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}

error_t http_request_validate(const char* data) 
{
    if (IS_NULL(data))
        return EXIT_FAILURE;

    size_t length = strlen(data);
    if (length == 0)
        return EXIT_FAILURE;

    // Проверяем наличие CRLF в конце строки запроса
    const char* first_crlf = find_crlf(data, data + length);
    if (IS_NULL(first_crlf))
        return EXIT_FAILURE;

    // Проверяем, что первая строка (заголовок запроса) содержит как минимум 2 пробела
    // для разделения метода, пути и версии
    const char* current = data;
    int space_count = 0;
    
    while (current < first_crlf) {
        if (*current == ' ')
            space_count++;
        current++;
    }

    if (space_count < 2)
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}

http_request_t* http_request_parse(worker_info_t* w, const char* data) 
{
    if (IS_NULL(w))
        return NULL;

    if (IS_NULL(data))
        return NULL;

    if (IS_ERROR(http_request_validate(data)))
        return NULL;

    size_t length = strlen(data);

    const char *data_start = data,
               *data_end = data + length; 

    http_request_t* request = http_request_create();
    if (IS_NULL(request))
        return NULL;

    // Title parsing
    const char *title_start = data_start,
               *title_end = find_crlf(title_start, data_end);
    
    if (IS_NULL(title_end)) {
        http_request_free(request);
        return NULL;
    }

    if (IS_ERROR(parse_request_title(request, title_start, title_end))) {
        http_request_free(request);
        return NULL;
    }

    // Headers parsing
    const char *headers_start = title_end + 2,
               *headers_end = find_double_crlf(headers_start, data_end);
    if (!IS_NULL(headers_end)) {
        const char *body_start = headers_end + 4,
                   *body_end = data_end;
        
        parse_request_body(request, body_start, body_end);
    } else {
        headers_end = data_end;
    }

    parse_request_headers(request, headers_start, headers_end);

    return request;
}

const char* http_request_get_header(http_request_t* request, const char* header)
{
    if (IS_NULL(request))
        return NULL;

    if (IS_NULL(header))
        return NULL;

    headers_list_t* current = request->headers;
    while (!IS_NULL(current)) {
        if (strcmp(current->header, header) == 0) {
            const char *start = current->value,
                       *end = current->value + strlen(current->value);
            return copy_string(start, end);
        }
        current = current->next;
    }

    return NULL;
}
