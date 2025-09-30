#ifndef CONSTS_H__
#define CONSTS_H__

#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <unistd.h>
#include <signal.h>
#include <string.h>
#include <fcntl.h>
#include <errno.h>
#include <stdarg.h>
#include <time.h>
#include <pthread.h>
#include <dirent.h>
#include <sys/wait.h>
#include <sys/stat.h>
#include <sys/socket.h>
#include <sys/select.h>
#include <sys/types.h>
#include <arpa/inet.h>
#include <linux/limits.h>

#define EXIT_FAILURE -1

#define LOG_TARGET LOG_TARGET_ERRORS

#define SERVER_PORT 8080
#define MAX_CONNECTIONS 1024

#define MAX_WORKERS 10
#define MIN_WORKERS 3

#define DEFAULT_BASE_DIR "base"
#define DEFAULT_FILE "index.html"
#define DEFAULT_MAX_FILE_SIZE (128 * 1024 * 1024) // 128MB
#define DEFAULT_REQUEST_LENGTH (2 * PATH_MAX)
#define DEFAULT_URI_LENGTH PATH_MAX

static const char* ALLOWED_METHODS[] = {"GET", "HEAD"};
static const size_t ALLOWED_METHODS_COUNT = sizeof(ALLOWED_METHODS) / sizeof(const char*);

typedef enum {
    HTTP_OK = 200,
    HTTP_NO_CONTENT = 204,
    HTTP_BAD_REQUEST = 400,
    HTTP_FORBIDDEN = 403,
    HTTP_NOT_FOUND = 404,
    HTTP_METHOD_NOT_ALLOWED = 405,
    HTTP_PAYLOAD_TOO_LARGE = 413,
    HTTP_URI_TOO_LONG = 414,
    HTTP_INTERNAL_ERROR = 500
} http_status_code_t;

#endif // CONSTS_H__