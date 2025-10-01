#include "prefork_manager.h"
#include "connection_handler.h"

#define CHILD_PROCESS     0
#define WORKER__          worker_info_init()



static inline worker_info_t worker_info_init()
{
    return (worker_info_t) {
        .pid = 0,
        .to_worker_channel = {0, 0},
        .logger = NULL
    };
}

int worker_pool_process_free(worker_info_t* w, logger_t* logger)
{
    if (IS_NULL(w)) {
        LOG_ERROR(logger, "worker_pool_process_free error: %s", "worker_info_t is NULL");
        return EXIT_FAILURE;
    }

    if (IS_ERROR(kill(w->pid, SIGTERM))) {
        LOG_ERROR(logger, "worker_pool_process_free error: failed to kill process %d: %s", 
                 w->pid, strerror(errno));
        return EXIT_FAILURE;
    }

    if (IS_ERROR(close(w->to_worker_channel[0]))) {
        LOG_WARNING(logger, "worker_pool_process_free: failed to close to_worker_channel[0] \for process %d", w->pid);
    }
    
    if (IS_ERROR(close(w->to_worker_channel[1]))) {
        LOG_WARNING(logger, "worker_pool_process_free: failed to close to_worker_channel[1] for process %d", w->pid);
    }
    
    LOG_INFO(logger, "worker_pool_process_free: worker process %d terminated successfully", w->pid);
    return EXIT_SUCCESS;
}

int worker_pool_processes_free(worker_pool_t* wp)
{
    if (IS_NULL(wp))
        return;

    LOG_INFO(wp->logger, "worker_pool_processes_free: erminating %d worker processes", wp->worker_count);
    
    for (int i = 0; i < wp->worker_count; i++) {
        if (IS_ERROR(worker_pool_process_free(wp->workers + i, wp->logger))) {
            LOG_ERROR(wp->logger, "worker_pool_processes_free error: failed to terminate worker with PID %d", wp->workers[i].pid);
            return EXIT_FAILURE;
        }
    }

    LOG_INFO(wp->logger, "worker_pool_processes_free: all worker processes terminated successfully");
    return EXIT_SUCCESS;
}

error_t worker_pool_process_create(worker_info_t *w_info, logger_t* logger)
{
    if (IS_NULL(w_info))
        return EXIT_FAILURE;

    if (IS_NULL(logger))
        return EXIT_FAILURE;

    if (IS_ERROR(socketpair(AF_UNIX, SOCK_STREAM, 0, w_info->to_worker_channel))) {
        LOG_ERROR(logger, "worker_pool_process_create error: failed to create to_worker socketpair: %s", strerror(errno));
        return EXIT_FAILURE;
    }

    w_info->pid = fork();
    if (IS_ERROR(w_info->pid)) {
        LOG_ERROR(logger, "worker_pool_process_create error: fork failed: %s", strerror(errno));
        close(w_info->to_worker_channel[0]);
        close(w_info->to_worker_channel[1]);
        return EXIT_FAILURE;
    }

    if (w_info->pid == CHILD_PROCESS) {
        logger_t* worker_logger = logger_init(LOG_TARGET, NULL, WORKER);
        if (IS_NULL(worker_logger)) 
            exit(EXIT_FAILURE);

        w_info->pid = getpid();
        w_info->logger = worker_logger;
        LOG_INFO(worker_logger, "connection_handler_handler: worker process with PID %d started", w_info->pid);

        if (IS_ERROR(connection_handler_listening(w_info))) {
            logger_free(worker_logger);
            exit(EXIT_FAILURE);
        }
            
        logger_free(worker_logger);
        exit(EXIT_SUCCESS);
    } else {
        LOG_INFO(logger, "worker_pool_process_create: created worker process with PID %d", w_info->pid);
    }
    
    return EXIT_SUCCESS;
}

int worker_pool_processes_create(worker_pool_t* wp, size_t p_count)
{
    if (IS_NULL(wp))
        return;

    LOG_INFO(wp->logger, "worker_pool_processes_create: creating %zu worker processes", p_count);
    
    for (wp->worker_count = 0; wp->worker_count < p_count; wp->worker_count++) {
        if (IS_ERROR(worker_pool_process_create(wp->workers + wp->worker_count, wp->logger))) {
            LOG_ERROR(wp->logger, "worker_pool_processes_create error: failed to create worker %zu", wp->worker_count);
            worker_pool_processes_free(wp);
            return EXIT_FAILURE;
        }
        LOG_DEBUG(wp->logger, "worker_pool_processes_create: worker %zu created with PID %d", wp->worker_count, wp->workers[wp->worker_count].pid);
    }

    LOG_INFO(wp->logger, "worker_pool_processes_create: successfully created %zu worker processes", p_count);
    return EXIT_SUCCESS;
}

worker_pool_t* worker_pool_create(size_t w_count, logger_t* logger)
{
    if (IS_NULL(logger))
        return NULL;

    if (w_count > MAX_WORKERS) {
        LOG_ERROR(logger, "worker_poll create error: %s", "w_count > MAX_WORKERS");
        return NULL;
    }
        
    worker_pool_t* wp = (worker_pool_t*)malloc(sizeof(worker_pool_t));
    if (IS_NULL(wp)) {
        LOG_ERROR(logger, "worker_poll create error: %s", "malloc returned NULL");
        return NULL;
    }

    wp->logger = logger;
    wp->worker_count = 0;
    for (int i = 0; i < MAX_WORKERS; i++)
        wp->workers[i] = WORKER__;

    if (IS_ERROR(pthread_mutex_init(&wp->mutex, NULL))) {
        LOG_ERROR(logger, "worker_poll create error: %s", "pthread_mutex_init error");
        worker_pool_free(wp);
        return NULL;
    }
    
    if (IS_ERROR(worker_pool_processes_create(wp, w_count))) {
        LOG_ERROR(logger, "worker_poll create error: %s", "worker_poll processes create error");
        worker_pool_free(wp);
        return NULL;
    }
        
    return wp;
}

error_t worker_pool_add_worker(worker_pool_t* wp)
{
    if (IS_NULL(wp))
        return;

    if (wp->worker_count == MAX_WORKERS) {
        LOG_WARNING(wp->logger, "worker_pool_add_worker: maximum workers (%d) reached", MAX_WORKERS);
        return EXIT_FAILURE;
    }

    LOG_INFO(wp->logger, "worker_pool_add_worker: adding new worker process");
    
    if (IS_ERROR(worker_pool_process_create(wp->workers + wp->worker_count, wp->logger))) {
        LOG_ERROR(wp->logger, "worker_pool_add_worker error: failed to create worker");
        return EXIT_FAILURE;
    } else {
        wp->worker_count++;
        LOG_INFO(wp->logger, "worker_pool_add_worker: worker added successfully, total workers: %zu", wp->worker_count);
    }

    return EXIT_SUCCESS;
}


error_t worker_pool_remove_worker(worker_pool_t* wp)
{
    if (IS_NULL(wp))
        return;

    if (wp->worker_count == 0) {
        LOG_WARNING(wp->logger, "worker_pool_remove_worker: no workers to remove");
        return EXIT_FAILURE;
    }

    pid_t pid_to_remove = wp->workers[wp->worker_count - 1].pid;
    LOG_INFO(wp->logger, "worker_pool_remove_worker: removing worker process %d", pid_to_remove);
    
    if (IS_ERROR(worker_pool_process_free(wp->workers + wp->worker_count - 1, wp->logger))) {
        LOG_ERROR(wp->logger, "worker_pool_remove_worker error: failed to remove worker %d", pid_to_remove);
        return EXIT_FAILURE;
    } else {
        wp->worker_count--;
        wp->workers[wp->worker_count] = WORKER__;
        LOG_INFO(wp->logger, "worker_pool_remove_worker: worker with PID %d removed successfully, total workers: %zu", pid_to_remove, wp->worker_count);
    }

    return EXIT_SUCCESS;
}

void worker_pool_free(worker_pool_t* wp)
{
    if (IS_NULL(wp))
        return;

    LOG_INFO(wp->logger, "worker_pool_free: freeing worker pool");
    worker_pool_processes_free(wp);
    pthread_mutex_destroy(&wp->mutex);
    free(wp);
    LOG_INFO(wp->logger, "worker_pool_free: worker pool freed successfully");
}

bool worker_pool_is_max_workers(worker_pool_t* wp)
{
    return wp->worker_count == MAX_WORKERS;
}

static inline size_t worker_pool_process_count_open_fds(pid_t pid)
{
    char path[PATH_MAX];
    snprintf(path, sizeof(path), "/proc/%d/fd", pid);
    
    DIR *dir = opendir(path);
    if (IS_NULL(dir)) 
        return __INT_MAX__; 
    
    size_t count = 0;
    struct dirent *entry;
    while ((entry = readdir(dir)) != NULL) {
        if (strcmp(entry->d_name, ".") != 0 && strcmp(entry->d_name, "..") != 0) {
            count++;
        }
    }
    
    closedir(dir);
    return count;
}

static inline worker_info_t* worker_pool_get_worker_with_min_fds(worker_pool_t* wp)
{
    if (IS_NULL(wp) || wp->worker_count == 0) 
        return NULL;
    
    worker_info_t* best_worker = NULL;
    size_t min_fds = __INT_MAX__;
    
    for (size_t i = 0; i < wp->worker_count; i++) {
        worker_info_t* worker = &wp->workers[i];
        
        if (!process_is_alive(worker->pid) || process_is_terminated(worker->pid)) 
            continue;
        
        size_t fds_count = worker_pool_process_count_open_fds(worker->pid);
        LOG_DEBUG(wp->logger, "distributor_get_worker_with_min_fds: worker %d has %zu open fds", 
                 worker->pid, fds_count);
        
        if (fds_count < min_fds) {
            min_fds = fds_count;
            best_worker = worker;
        }
    }
    
    if (best_worker) {
        LOG_DEBUG(wp->logger, "distributor_get_worker_with_min_fds: selected worker %d with %zu fds", 
                 best_worker->pid, min_fds);
    }
    
    return best_worker;
}

worker_info_t* worker_pool_get_rand_worker(worker_pool_t* wp) 
{
    if (IS_NULL(wp)) 
        return NULL;

    return wp->workers + (rand() % wp->worker_count);
}

worker_info_t* worker_pool_get_worker(worker_pool_t* wp)
{
    if (IS_NULL(wp)) 
        return NULL;
    
    return worker_pool_get_worker_with_min_fds(wp);
}

static inline void worker_pool_processes_check(worker_pool_t* wp)
{
    if (IS_NULL(wp)) 
        return;
    
    for (int i = 0; i < wp->worker_count; i++) {
        if (!process_is_alive(wp->workers[i].pid) || process_is_terminated(wp->workers[i].pid)) {
            LOG_WARNING(wp->logger, "worker_pool_processes_check: worker process with PID %d is not alive, restarting", wp->workers[i].pid);
            if (IS_ERROR(worker_pool_process_create(wp->workers + i, wp->logger))) {
                LOG_ERROR(wp->logger, "worker_pool_processes_check error: failed to restart worker process %d", wp->workers[i].pid);
            } else {
                LOG_INFO(wp->logger, "worker_pool_processes_check: worker process %d restarted successfully", wp->workers[i].pid);
            }
        }
    }
}

error_t worker_pool_monitoring(worker_pool_t* wp)
{
    if (IS_NULL(wp))
        return;

    LOG_INFO(wp->logger, "worker_pool_monitoring: starting worker pool monitoring");
    
    while (true) {
        pthread_mutex_lock(&wp->mutex);
        worker_pool_processes_check(wp);
        pthread_mutex_unlock(&wp->mutex);
        sleep(5);
    }

    return EXIT_SUCCESS;
}