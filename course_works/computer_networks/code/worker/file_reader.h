#ifndef FILE_READER_H__
#define FILE_READER_H__

#include "consts.h"
#include "utils.h"
#include "logger.h"

// Структура конфигурации file_reader
typedef struct {
    char* base_directory;          // Базовая директория для статических файлов
    size_t max_file_size;          // Максимальный размер файла для чтения
    char* default_file;            // Файл по умолчанию (index.html и т.д.)
} file_reader_config_t;

// Структура результата чтения файла
typedef struct {
    char* content;             // Содержимое файла
    size_t content_length;     // Длина содержимого
    char* content_type;        // MIME-тип содержимого
    bool is_directory;         // Является ли путь директорией
} file_read_result_t;


file_reader_config_t* file_reader_config_create();

file_reader_config_t* file_reader_config_create_with_data(const char* base_dir, size_t max_file_size);

void file_reader_config_free(file_reader_config_t* fr_cfg);

file_read_result_t* file_read_result_create();

void file_read_result_free(file_read_result_t* fr_result);

file_read_result_t* file_reader_get(const char* full_path);

#endif // FILE_READER_H__