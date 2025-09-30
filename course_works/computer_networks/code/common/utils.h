#ifndef UTILS_H__
#define UTILS_H__

#include "consts.h"

#define IS_NULL(x)              ((x) == NULL)
#define IS_ERROR(x)             ((x) < 0)
#define IS_EQUAL_STR(x,y)            (strcmp(x, y) == 0)


// End of string check
#define IS_EOS(ptr)             (IS_END_OF_STRING(ptr))
#define IS_END_OF_STRING(ptr)   ((!IS_NULL(ptr)) && (IS_CARRIAGE_RETURN(ptr) || IS_NEWLINE(ptr) || IS_NUL(ptr)))
#define IS_CARRIAGE_RETURN(ptr) ((*(ptr)) == '\r')
#define IS_NEWLINE(ptr)         ((*(ptr)) == '\n')
#define IS_NUL(ptr)             ((*(ptr)) == '\0')

#define MAX(x,y) (((x) > (y)) ? (x) : (y))
#define MIN(x,y) (((x) < (y)) ? (x) : (y))

typedef int error_t;

error_t send_fd(int pipe_fd, int fd_to_send);

int recv_fd(int pipe_fd);

char* dup_string(const char* string);

char* copy_string(const char* start, const char* end);

char* extract_string(const char* start, const char* end);

const char* find_lf(const char* start, const char* end);

const char* find_crlf(const char* start, const char* end);

const char* find_double_crlf(const char* start, const char* end);

const char* skip_space(const char* start, const char* end);

const char* skip_nonspace(const char* start, const char* end);

bool is_directory_exists(char* dir);

bool is_file_exists(const char* filename);

bool is_file_in_directory(const char* directory, const char* filename);

char* absolute_path(const char* folder, const char* filename);

#endif // UTILS_H__s