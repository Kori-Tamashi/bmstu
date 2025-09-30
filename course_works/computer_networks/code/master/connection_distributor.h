#ifndef CONNECTION_DISTRIBUTOR_H__
#define CONNECTION_DISTRIBUTOR_H__

#include "consts.h"
#include "utils.h"
#include "prefork_manager.h"
#include "logger.h"

// Структура для хранения данных об адресе
typedef struct {
    struct sockaddr_in addr;
    socklen_t addr_len;
} sockaddr_info_t;

// Структура для хранения данных о подключении
typedef struct {
    int conn_sock;
    sockaddr_info_t addr_info;
} conn_cfg_t;

// Структура для хранения данных о распределителе
typedef struct {
    int listen_port;
    sockaddr_info_t addr_info;
} distributor_cfg_t;

// Структура для хранения контекста распределителя
typedef struct {
    int listen_sock;
    distributor_cfg_t config;
    logger_t* logger;
} distributor_ctxt_t;

distributor_ctxt_t* distributor_ctxt_create(logger_t* logger);

error_t distributor_accept_connection(distributor_ctxt_t* d_ctxt, conn_cfg_t* conn);

error_t distributor_listening(distributor_ctxt_t* d_ctxt, worker_pool_t* wp);

void distributor_ctxt_free(distributor_ctxt_t* d_ctxt);

#endif // CONNECTION_DISTRIBUTOR_H__