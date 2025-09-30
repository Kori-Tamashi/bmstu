#ifndef CONNECTION_HANDLER_H__
#define CONNECTION_HANDLER_H__

#include "consts.h"
#include "utils.h"

#include "prefork_manager.h"

typedef struct {
    fd_set read_fds;
    int max_fd;
    int conn_sockets[MAX_CONNECTIONS];
    size_t conn_sockets_count;
} conn_ctxt_t;

error_t connection_handler_listening(worker_info_t* worker);

#endif // CONNECTION_HANDLER_H__