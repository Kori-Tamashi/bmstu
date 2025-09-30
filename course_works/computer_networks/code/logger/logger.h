#ifndef LOGGER_H__
#define LOGGER_H__

#include "consts.h"
#include "utils.h"

#define SYSLOG "/var/log/syslog"

typedef enum {
    MASTER,
    WORKER
} process_type_t;

typedef enum {
    LOG_LEVEL_DEBUG,
    LOG_LEVEL_INFO,
    LOG_LEVEL_WARNING,
    LOG_LEVEL_ERROR,
    LOG_LEVEL_CRITICAL
} log_level_t;

typedef enum {
    LOG_TARGET_CONSOLE,
    LOG_TARGET_ERRORS,
    LOG_TARGET_FILE,
    LOG_TARGET_SYSLOG
} log_target_t;

typedef struct {
    log_target_t log_target;
    int fd;
    process_type_t p_type;
    pid_t pid;
} logger_t;

logger_t* logger_init(log_target_t target, const char* filename, process_type_t p_type);

void logger_log(logger_t* logger, log_level_t level, const char* format, ...);

void logger_free(logger_t* logger);

#define LOG_DEBUG(logger, ...) logger_log(logger, LOG_LEVEL_DEBUG, __VA_ARGS__)
#define LOG_INFO(logger, ...) logger_log(logger, LOG_LEVEL_INFO, __VA_ARGS__)
#define LOG_WARNING(logger, ...) logger_log(logger, LOG_LEVEL_WARNING, __VA_ARGS__)
#define LOG_ERROR(logger, ...) logger_log(logger, LOG_LEVEL_ERROR, __VA_ARGS__)
#define LOG_CRITICAL(logger, ...) logger_log(logger, LOG_LEVEL_CRITICAL, __VA_ARGS__)

#endif // LOGGER_H__