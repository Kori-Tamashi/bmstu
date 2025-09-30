#include "connection_distributor.h"
#include "prefork_manager.h"

typedef struct thread_args {
    distributor_ctxt_t* d_ctxt;
    worker_pool_t* wp;
} thread_args_t;

logger_t* logger;
distributor_ctxt_t* distributor;
worker_pool_t* worker_pool;

void sig_handler(int sig_num)
{
    worker_pool_free(worker_pool);
    distributor_ctxt_free(distributor);
    logger_free(logger);

    exit(EXIT_SUCCESS);
}

error_t thread_distributor_start(void *arg)
{
    thread_args_t* args = (thread_args_t*)arg;
    if (IS_ERROR(distributor_listening(args->d_ctxt, args->wp)))
        return EXIT_FAILURE;
    return EXIT_SUCCESS;
}

error_t thread_worker_pool_start(void* arg)
{
    worker_pool_t* wp = (worker_pool_t*) arg;
    if (IS_ERROR(worker_pool_monitoring(wp)))
        return EXIT_FAILURE;
    return EXIT_SUCCESS;
}

int main(void)
{
    srand(time(NULL));
    
    if (signal(SIGINT, sig_handler) == SIG_ERR)
        return EXIT_FAILURE;

    logger = logger_init(LOG_TARGET, NULL, MASTER);
    if (IS_NULL(logger)) 
        return EXIT_FAILURE;
    else
        LOG_INFO(logger, "main: logger created");

    distributor = distributor_ctxt_create(logger);
    if (IS_NULL(distributor)) {
        LOG_CRITICAL(logger, "main error: distributor is NULL");
        return EXIT_FAILURE;
    } else {
        LOG_INFO(logger, "main: connection distributor created");
    } 

    worker_pool = worker_pool_create(MIN_WORKERS, logger);
    if (IS_NULL(distributor))
    {
        LOG_CRITICAL(logger, "main error: worker pool is NULL");
        distributor_ctxt_free(distributor);
        return EXIT_FAILURE;
    } else {
        LOG_INFO(logger, "main: worker pool created");
    }

    pthread_t d_thread = {0}, wp_thread = {0};
    thread_args_t args = (thread_args_t) {
        .d_ctxt = distributor,
        .wp = worker_pool
    };
    
    if (IS_ERROR(pthread_create(&d_thread, NULL, thread_distributor_start, &args))) {
        LOG_CRITICAL(logger, "main error: failed thread create for distributor");
        distributor_ctxt_free(distributor);
        worker_pool_free(worker_pool);
        return EXIT_FAILURE;
    } else {
        LOG_INFO(logger, "main: distributor thread created");
    }

    if (IS_ERROR(pthread_create(&wp_thread, NULL, thread_worker_pool_start, worker_pool))){
        LOG_CRITICAL(logger, "main error: failed thread create for worker pool");
        distributor_ctxt_free(distributor);
        worker_pool_free(worker_pool);
        return EXIT_FAILURE;
    } else {
        LOG_INFO(logger, "main: worker pool thread created");
    }
    
    while (true) {

    }

    worker_pool_free(worker_pool);
    distributor_ctxt_free(distributor);
    logger_free(logger);

    return EXIT_SUCCESS;
}

