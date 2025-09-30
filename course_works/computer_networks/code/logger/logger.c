#include "logger.h"

static void file_lock(int fd) {
    struct flock lock;
    lock.l_type = F_WRLCK;
    lock.l_start = 0;
    lock.l_whence = SEEK_SET;
    lock.l_len = 0;
    lock.l_pid = getpid();
    
    while (fcntl(fd, F_SETLKW, &lock) < 0) {
        if (errno != EINTR) {
            perror("fcntl");
            break;
        }
    }
}

static void file_unlock(int fd) {
    struct flock lock;
    lock.l_type = F_UNLCK;
    lock.l_start = 0;
    lock.l_whence = SEEK_SET;
    lock.l_len = 0;
    lock.l_pid = getpid();
    
    if (fcntl(fd, F_SETLK, &lock) < 0) {
        perror("fcntl");
    }
}

logger_t* logger_init(log_target_t target, const char* filename, process_type_t p_type)
{
    logger_t* logger = (logger_t*)malloc(sizeof(logger_t));
    if (IS_NULL(logger))
        return NULL;

    logger->log_target = target;
    logger->p_type = p_type;
    logger->pid = getpid();

    switch (target)
    {
    case LOG_TARGET_CONSOLE: {
            logger->fd = STDOUT_FILENO;
            break;
        }
    case LOG_TARGET_ERRORS: {
        logger->fd = STDERR_FILENO;
        break;
    }
    case LOG_TARGET_FILE: {
        if (IS_NULL(filename)) {
            free(logger);
            return NULL;
        }
        if (IS_ERROR(logger->fd = open(filename, O_WRONLY | O_CREAT | O_APPEND, 0644))) {
            free(logger);
            return NULL;
        }
        break;
    }
    case LOG_TARGET_SYSLOG: {
        if (IS_ERROR(logger->fd = open(SYSLOG, O_WRONLY | O_CREAT | O_APPEND, 0644))) {
            free(logger);
            return NULL;
        }
        break;
    }
    default:
        logger->fd = stderr;
        break;
    }

    return logger;
}

void logger_free(logger_t* logger)
{
    if (IS_NULL(logger))
        return;

    close(logger->fd);
    free(logger);
}

void logger_log(logger_t* logger, log_level_t level, const char* format, ...)
{
    if (IS_NULL(logger))
        return;

    time_t now = time(NULL);
    struct tm* timeinfo;
    localtime_r(&now, &timeinfo);
    char timestamp[20];
    strftime(timestamp, sizeof(timestamp), "%Y-%m-%d %H:%M:%S", &timeinfo);

    const char* level_str;
    switch (level)
    {
    case LOG_LEVEL_DEBUG: level_str = "DEBUG"; break;
    case LOG_LEVEL_INFO: level_str = "INFO"; break;
    case LOG_LEVEL_WARNING: level_str = "WARNING"; break;
    case LOG_LEVEL_ERROR: level_str = "ERROR"; break;
    case LOG_LEVEL_CRITICAL: level_str = "CRITICAL"; break;
    default: level_str = "UNKNOWN"; break;
    }

    const char* process_type;
    switch (logger->p_type)
    {
    case MASTER: process_type = "MASTER"; break;
    case WORKER: process_type = "WORKER"; break;
    default:
        break;
    }

    // Форматируем сообщение
    char message[1024];
    va_list args;
    va_start(args, format);
    int message_len = vsnprintf(message, sizeof(message), format, args);
    va_end(args);
    
    if (message_len >= sizeof(message)) {
        message[sizeof(message) - 1] = '\0';
        message_len = sizeof(message) - 1;
    }
    
    // Блокируем файл для межпроцессной синхронизации
    if (logger->log_target == LOG_TARGET_FILE || logger->log_target == LOG_TARGET_SYSLOG) {
        file_lock(logger->fd);
    }
    
    // Формируем итоговую строку лога
    char log_line[1200];
    int log_len = snprintf(log_line, sizeof(log_line), 
                          "[%s] [%s]\t [%s] [PID:%d] %s\n",
                          timestamp, level_str, process_type, logger->pid, message);
    
    if (log_len > 0) {
        write(logger->fd, log_line, log_len);
    }
    
    // Разблокируем файл
    if (logger->log_target == LOG_TARGET_FILE || logger->log_target == LOG_TARGET_SYSLOG) {
        file_unlock(logger->fd);
    }
}

