// connection_distributor.c
if (IS_ERROR(listen(listen_sock, MAX_WORKERS * MAX_CONNECTIONS))) {
    LOG_ERROR(logger, "distributor_socket_create error: failed to listen on socket: %s", strerror(errno));
    close(listen_sock);
    return EXIT_FAILURE;
}