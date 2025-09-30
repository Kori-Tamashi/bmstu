#ifndef REQUEST_PARSER_H__
#define REQUEST_PARSER_H__

#include "consts.h"
#include "utils.h"

#include "prefork_manager.h"

#define MAX_METHOD_LEN 16
#define MAX_PATH_LEN 1024
#define MAX_VERSION_LEN 16

typedef struct headers_list {
    char* header;
    char* value;
    struct headers_list* next;
} headers_list_t;

typedef struct {
    char* method;      
    char* path;        
    char* version; 
    char* body; 
    headers_list_t* headers;     
} http_request_t;

http_request_t* http_request_create();

http_request_t* http_request_parse(worker_info_t* w, const char* data);

const char* http_request_get_header(http_request_t* request, const char* header);

void http_request_free(http_request_t* request);

#endif // REQUEST_PARSER_H__