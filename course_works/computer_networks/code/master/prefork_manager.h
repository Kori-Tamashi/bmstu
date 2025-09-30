#ifndef PREFORK_MANAGER_H__
#define PREFORK_MANAGER_H__

#include "consts.h"
#include "utils.h"

#include "logger.h"

// Структура для хранения данных о рабочем процессе
typedef struct {
    pid_t pid;
    int to_worker_channel[2];
    logger_t* logger;
} worker_info_t;

// Структура для хранения данных о рабочих процессах
typedef struct {
    size_t worker_count;
    worker_info_t workers[MAX_WORKERS];
    pthread_mutex_t mutex;
    logger_t* logger;
} worker_pool_t;

worker_pool_t* worker_pool_create(size_t w_count, logger_t* logger);

error_t worker_pool_add_worker(worker_pool_t* wp);

error_t worker_pool_remove_worker(worker_pool_t* wp);

bool worker_pool_is_max_workers(worker_pool_t* wp);

worker_info_t* worker_pool_get_worker(worker_pool_t* wp);

error_t worker_pool_monitoring(worker_pool_t* wp);

void worker_pool_free(worker_pool_t* wp);

#endif // PREFORK_MANAGER_H__