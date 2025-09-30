// client.c
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>

#define BUFFER_SIZE 4096
#define DEFAULT_PORT 8080

void print_usage(const char* program_name) {
    printf("Usage: %s <server_ip> [port]\n", program_name);
    printf("Default port: %d\n", DEFAULT_PORT);
}

int main(int argc, char* argv[]) {
    if (argc < 2) {
        print_usage(argv[0]);
        return EXIT_FAILURE;
    }

    const char* server_ip = argv[1];
    int port = (argc > 2) ? atoi(argv[2]) : DEFAULT_PORT;

    // Создание сокета
    int sock = socket(AF_INET, SOCK_STREAM, 0);
    if (sock < 0) {
        perror("Socket creation failed");
        return EXIT_FAILURE;
    }

    // Настройка адреса сервера
    struct sockaddr_in server_addr;
    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(port);
    
    if (inet_pton(AF_INET, server_ip, &server_addr.sin_addr) <= 0) {
        perror("Invalid address/Address not supported");
        close(sock);
        return EXIT_FAILURE;
    }

    // Подключение к серверу
    printf("Connecting to %s:%d...\n", server_ip, port);
    if (connect(sock, (struct sockaddr*)&server_addr, sizeof(server_addr)) < 0) {
        perror("Connection failed");
        close(sock);
        return EXIT_FAILURE;
    }
    printf("Connected successfully!\n");

    // Формирование HTTP-запроса
    const char* request_template = 
        "GET ../bin HTTP/1.1\r\n";
    
    char request[BUFFER_SIZE];
    snprintf(request, sizeof(request), request_template, server_ip, port);
    
    // Отправка запроса
    printf("Sending request:\n%s\n", request);
    if (send(sock, request, strlen(request), 0) < 0) {
        perror("Send failed");
        close(sock);
        return EXIT_FAILURE;
    }

    // Получение ответа
    char response[BUFFER_SIZE];
    ssize_t bytes_received;
    printf("Response:\n");
    
    while ((bytes_received = recv(sock, response, sizeof(response) - 1, 0)) > 0) {
        response[bytes_received] = '\0';
        printf("%s", response);
    }
    
    if (bytes_received < 0) {
        perror("Receive failed");
    }
    
    printf("\nConnection closed.\n");
    close(sock);
    return EXIT_SUCCESS;
}