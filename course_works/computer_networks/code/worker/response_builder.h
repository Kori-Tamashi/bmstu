#ifndef RESPONSE_BUILDER_H__
#define RESPONSE_BUILDER_H__

#include "consts.h"
#include "utils.h"

#include "request_parser.h"
#include "security_module.h"
#include "file_reader.h"

typedef struct {
    http_status_code_t status_code;
    headers_list_t* headers;
    char* body;
    size_t body_length;
} http_response_t;

http_response_t* http_response_create();

http_response_t* http_response_create_by_status(http_status_code_t status, file_read_result_t* file_result);

http_response_t* http_response_create_by_request(http_request_t* http_request);

error_t http_response_add_header(http_response_t* response, char* header, char* value);

char* http_response_build(http_response_t* response);

void http_response_free(http_response_t* response);

#endif // RESPONSE_BUILDER_H__