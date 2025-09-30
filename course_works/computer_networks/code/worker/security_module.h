#ifndef SECURITY_MODULE_H__
#define SECURITY_MODULE_H__

#include "consts.h"
#include "utils.h"
#include "logger.h"

typedef struct {
    char* base_directory;
    size_t max_file_size;
    size_t max_uri_length;
} security_config_t;

security_config_t* security_config_create();

security_config_t* security_config_create_with_data(char* base_directory, size_t max_file_size, size_t max_uri_length);

void security_config_free(security_config_t* s_config);

error_t security_check_method(security_config_t* s_config, char* method);

error_t security_check_uri_length(security_config_t* s_config, char* path);

error_t security_check_path_traversal(security_config_t* s_config, char* path);

error_t security_check_path_type(char* path);

error_t security_check_file_exists(char* filepath);

error_t security_check_directory_exists(char* directory_path);

error_t security_check_file_access(char* filepath);

error_t security_check_directory_access(char* directory_path);

error_t security_check_file_size(char* filepath);

error_t security_check_file_emptiness(char* filepath);

#endif // SECURITY_MODULE_H__