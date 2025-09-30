#include "file_reader.h"

file_reader_config_t* file_reader_config_create()
{
    file_reader_config_t* fr_cfg = (file_reader_config_t*)malloc(sizeof(file_reader_config_t));
    if (IS_NULL(fr_cfg))
        return NULL;

    memset(fr_cfg, 0, sizeof(file_reader_config_t));
    fr_cfg->base_directory = NULL;
    fr_cfg->default_file = NULL;

    return fr_cfg;
}

file_reader_config_t* file_reader_config_create_with_data(const char* base_dir, size_t max_file_size) 
{
    if (IS_NULL(base_dir))
        return NULL;

    file_reader_config_t* fr_cfg = file_reader_config_create();
    if (IS_NULL(fr_cfg))
        return fr_cfg;

    fr_cfg->max_file_size = max_file_size;

    fr_cfg->base_directory = dup_string(base_dir);
    if (IS_NULL(fr_cfg->base_directory)) {
        free(fr_cfg);
        return NULL;
    }

    fr_cfg->default_file = dup_string(DEFAULT_FILE);
    if (IS_NULL(fr_cfg->default_file)){
        free(fr_cfg->base_directory);
        free(fr_cfg);
        return NULL;
    }

    return fr_cfg;
}

void file_reader_config_free(file_reader_config_t* fr_cfg)
{
    if (IS_NULL(fr_cfg)) 
        return;

    if (!IS_NULL(fr_cfg->base_directory))
        free(fr_cfg->base_directory);

    if (!IS_NULL(fr_cfg->default_file))
        free(fr_cfg->default_file);
    
    free(fr_cfg);
}

file_read_result_t* file_read_result_create()
{
    file_read_result_t* fr_result = (file_read_result_t*)malloc(sizeof(file_read_result_t));
    if (IS_NULL(fr_result))
        return NULL;

    memset(fr_result, 0, sizeof(file_read_result_t));
    fr_result->content = NULL;
    fr_result->content_type = NULL;

    return fr_result;
}

void file_read_result_free(file_read_result_t* fr_result)
{
    if (IS_NULL(fr_result))
        return NULL;

    if (!IS_NULL(fr_result->content))
        free(fr_result->content);

    if (!IS_NULL(fr_result->content_type))
        free(fr_result->content_type);

    free(fr_result);
}


bool file_reader_path_is_safe(const char* base_dir, const char* request_path)
{
    if (IS_NULL(base_dir))
        return false;

    if (IS_NULL(request_path))
        return false;

    char resolved_path[PATH_MAX];
    char resolved_base[PATH_MAX];
    
    if (IS_NULL(realpath(base_dir, resolved_base)))
        return false;

    char full_path[PATH_MAX];
    snprintf(full_path, sizeof(full_path), "%s/%s", base_dir, request_path);
    
    if (IS_NULL(realpath(full_path, resolved_path))) 
        return false;

    return (strncmp(resolved_path, resolved_base, strlen(resolved_base)) == 0);
}

const char* file_reader_get_mime_type(const char* filename)
{
    if (IS_NULL(filename))
        return "application/octet-stream";

    const char* extension = strrchr(filename, '.');
    if (IS_NULL(extension))
        return "application/octet-stream";

    if (IS_EQUAL_STR(extension, ".html") || IS_EQUAL_STR(extension, ".htm")) {
        return "text/html";
    } else if (IS_EQUAL_STR(extension, ".css")) {
        return "text/css";
    } else if (IS_EQUAL_STR(extension, ".js")) {
        return "application/javascript";
    } else if (IS_EQUAL_STR(extension, ".json")) {
        return "application/json";
    } else if (IS_EQUAL_STR(extension, ".xml")) {
        return "application/xml";
    } else if (IS_EQUAL_STR(extension, ".jpg") || IS_EQUAL_STR(extension, ".jpeg")) {
        return "image/jpeg";
    } else if (IS_EQUAL_STR(extension, ".png")) {
        return "image/png";
    } else if (IS_EQUAL_STR(extension, ".gif")) {
        return "image/gif";
    } else if (IS_EQUAL_STR(extension, ".svg")) {
        return "image/svg+xml";
    } else if (IS_EQUAL_STR(extension, ".ico")) {
        return "image/x-icon";
    } else if (IS_EQUAL_STR(extension, ".txt")) {
        return "text/plain";
    } else if (IS_EQUAL_STR(extension, ".pdf")) {
        return "application/pdf";
    } else if (IS_EQUAL_STR(extension, ".zip")) {
        return "application/zip";
    } else {
        return "application/octet-stream";
    }
}

static char* read_file_content(const char* filepath, size_t* content_length)
{
    if (IS_NULL(filepath)) 
        return NULL;

    FILE* file = fopen(filepath, "rb");
    if (IS_NULL(file)) {
        return NULL;
    }

    if (IS_ERROR(fseek(file, 0, SEEK_END))) { 
        fclose(file);
        return NULL;
    }

    long file_size = ftell(file);
    if (IS_ERROR(file_size)) {
        fclose(file);
        return NULL;
    }

    rewind(file);

    char* content = (char*)malloc(file_size);
    if (IS_NULL(content)) {
        fclose(file);
        return NULL;
    }

    size_t bytes_read = fread(content, 1, file_size, file);
    if (bytes_read != (size_t)file_size) {
        free(content);
        fclose(file);
        return NULL;
    }

    *content_length = file_size;
    fclose(file);

    return content;
}

static char* read_directory_content(const char* dir_path)
{
    if (IS_NULL(dir_path))
        return NULL;

    DIR* dir = opendir(dir_path);
    if (IS_NULL(dir)) 
        return NULL;

    char* html = NULL;
    size_t html_size = 0;
    FILE* html_stream = open_memstream(&html, &html_size);
    
    if (IS_NULL(html_stream)) {
        closedir(dir);
        return NULL;
    }

    fprintf(html_stream, "<!DOCTYPE html>\n");
    fprintf(html_stream, "<html>\n<head>\n<title>Directory %s listing</title>\n", dir_path);
    fprintf(html_stream, "<style>body { font-family: Arial, sans-serif; margin: 40px; }</style>\n");
    fprintf(html_stream, "</head>\n<body>\n");
    fprintf(html_stream, "<h1>Directory %s listing</h1>\n", dir_path);
    fprintf(html_stream, "<ul>\n");

    struct dirent* entry;
    while (!IS_NULL((entry = readdir(dir)))) {
        if (IS_EQUAL_STR(entry->d_name, ".") || IS_EQUAL_STR(entry->d_name, ".."))
            continue;

        char encoded_name[PATH_MAX];
        snprintf(encoded_name, sizeof(encoded_name), "%s/%s", dir_path, entry->d_name);
        fprintf(html_stream, "<li><a href=\"%s\">%s</a></li>\n", encoded_name, entry->d_name);
    }

    fprintf(html_stream, "</ul>\n</body>\n</html>\n");
    fclose(html_stream);
    closedir(dir);
    
    return html;
}

file_read_result_t* file_reader_get_file(const char* file_path)
{
    if (IS_NULL(file_path))
        return NULL;

    file_read_result_t* fr_result = file_read_result_create();
    if (IS_NULL(fr_result))
        return NULL;

    fr_result->is_directory = false;
    fr_result->content = read_file_content(file_path, &fr_result->content_length);
    if (IS_NULL(fr_result->content)) {
        file_read_result_free(fr_result);
        return NULL;
    }

    fr_result->content_type = dup_string(file_reader_get_mime_type(file_path));
    if (IS_NULL(fr_result->content_type)) {
        file_read_result_free(fr_result);
        return NULL;
    }

    return fr_result;
}

file_read_result_t* file_reader_get_directory(const char* dir_path)
{
    if (IS_NULL(dir_path))
        return NULL;  

    file_read_result_t* fr_result = file_read_result_create();
    if (IS_NULL(fr_result))
        return NULL;

    fr_result->is_directory = true;
    fr_result->content = read_directory_content(dir_path);
    if (IS_NULL(fr_result->content)) {
        file_read_result_free(fr_result);
        return NULL;
    }

    fr_result->content_length = strlen(fr_result->content);
    fr_result->content_type = dup_string("text/html");
    if (IS_NULL(fr_result->content_type)) {
        file_read_result_free(fr_result);
        return NULL;
    }

    return fr_result;
}

file_read_result_t* file_reader_get(const char* full_path)
{
    if (IS_NULL(full_path))
        return NULL;

    if (is_directory_exists(full_path)) 
        return file_reader_get_directory(full_path);
    else if (is_file_exists(full_path)) 
        return file_reader_get_file(full_path);

    return NULL;
}