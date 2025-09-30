#include "security_module.h"

security_config_t* security_config_create()
{
    security_config_t* s_config = (security_config_t*)malloc(sizeof(security_config_t));
    if (IS_NULL(s_config))
        return NULL;

    memset(s_config, 0, sizeof(security_config_t));
    return s_config;
}

security_config_t* security_config_create_with_data(char* base_directory, size_t max_file_size, size_t max_uri_length)
{
    if (IS_NULL(base_directory))
        return NULL;

    security_config_t* s_config = (security_config_t*)malloc(sizeof(security_config_t));
    if (IS_NULL(s_config))
        return NULL;

    s_config->max_file_size = max_file_size;
    s_config->max_uri_length = max_uri_length;
    s_config->base_directory = dup_string(base_directory);
    if (IS_NULL(s_config->base_directory)) {
        free(s_config);
        return NULL;
    }

    return s_config;
}

void security_config_free(security_config_t* s_config)
{
    if (IS_NULL(s_config))
        return;

    free(s_config);
}

error_t security_check_method(security_config_t* s_config, char* method)
{
    if (IS_NULL(s_config))
        return EXIT_FAILURE;

    if (IS_NULL(method))
        return EXIT_FAILURE;

    for (int i = 0; i < ALLOWED_METHODS_COUNT; i++) {
        if (IS_EQUAL_STR(method, ALLOWED_METHODS[i]))
            return EXIT_SUCCESS;
    }

    return EXIT_FAILURE;
}

error_t security_check_uri_length(security_config_t* s_config, char* path) 
{
    if (IS_NULL(s_config))
        return EXIT_FAILURE;

    if (IS_NULL(path))
        return EXIT_FAILURE;
    
    return strlen(path) > s_config->max_uri_length ? EXIT_FAILURE : EXIT_SUCCESS;
}

error_t security_check_path_traversal(security_config_t* s_config, char* path)
{
    if (IS_NULL(s_config))
        return EXIT_FAILURE;

    if (IS_NULL(path))
        return EXIT_FAILURE;
    
    char resolved_path[PATH_MAX];
    char resolved_base[PATH_MAX];
    memset(resolved_path, 0, sizeof(resolved_path));
    memset(resolved_base, 0, sizeof(resolved_base));

    if (IS_NULL(realpath(s_config->base_directory, resolved_base)))
        return EXIT_FAILURE;

    char full_path[PATH_MAX];
    memset(full_path, 0, sizeof(full_path));
    snprintf(full_path, sizeof(full_path), "%s/%s", s_config->base_directory, path);

    if (IS_NULL(realpath(full_path, resolved_path))) 
        return EXIT_FAILURE;

    return (strncmp(resolved_path, resolved_base, strlen(resolved_base)) == 0) ? EXIT_SUCCESS : EXIT_FAILURE;
}

error_t security_check_path_type(char* path)
{
    if (IS_NULL(path))
        return EXIT_FAILURE;

    return (is_file_exists(path) || is_directory_exists(path)) ? EXIT_SUCCESS : EXIT_FAILURE;
}

error_t security_check_file_exists(char* filepath)
{
    if (IS_NULL(filepath))
        return EXIT_FAILURE;

    return is_file_exists(filepath) ? EXIT_SUCCESS : EXIT_FAILURE;
}

error_t security_check_directory_exists(char* directory_path)
{
    if (IS_NULL(directory_path))
        return EXIT_FAILURE;

    return is_directory_exists(directory_path) ? EXIT_SUCCESS : EXIT_FAILURE;
}

error_t security_check_file_access(char* filepath)
{
    if (IS_NULL(filepath))
        return EXIT_FAILURE;

    if (IS_ERROR(access(filepath, F_OK)))
        return EXIT_FAILURE;

    if (IS_ERROR(access(filepath, R_OK)))
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}

error_t security_check_directory_access(char* directory_path)
{
    if (IS_NULL(directory_path))
        return EXIT_FAILURE;

    if (IS_ERROR(access(directory_path, F_OK)))
        return EXIT_FAILURE;

    if (IS_ERROR(access(directory_path, R_OK | X_OK)))
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}

error_t security_check_file_size(char* filepath)
{
    if (IS_NULL(filepath))
        return EXIT_FAILURE;

    struct stat file_stat;
    if (IS_ERROR(stat(filepath, &file_stat)))
        return EXIT_FAILURE;

    if (file_stat.st_size > (off_t)DEFAULT_MAX_FILE_SIZE)
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}

error_t security_check_file_emptiness(char* filepath)
{
    if (IS_NULL(filepath))
        return EXIT_FAILURE;

    struct stat file_stat;
    if (IS_ERROR(stat(filepath, &file_stat)))
        return EXIT_FAILURE;

    if (file_stat.st_size == 0)
        return EXIT_FAILURE;

    return EXIT_SUCCESS;
}