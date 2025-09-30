#include "utils.h"

error_t send_fd(int pipe_fd, int fd_to_send)
{
    struct msghdr msg = {0};
    struct cmsghdr *cmsg;
    char buf[1] = {'F'};
    struct iovec io = { .iov_base = buf, .iov_len = 1 };

    union {
        char buf[CMSG_SPACE(sizeof(int))];
        struct cmsghdr align;
    } u;

    msg.msg_iov = &io;
    msg.msg_iovlen = 1;
    msg.msg_control = u.buf;
    msg.msg_controllen = sizeof(u.buf);

    // Важно: инициализировать остальные поля структуры msghdr
    msg.msg_name = NULL;
    msg.msg_namelen = 0;
    msg.msg_flags = 0;

    cmsg = CMSG_FIRSTHDR(&msg);
    cmsg->cmsg_level = SOL_SOCKET;
    cmsg->cmsg_type = SCM_RIGHTS;
    cmsg->cmsg_len = CMSG_LEN(sizeof(int));
    
    int* fd_ptr = (int*)CMSG_DATA(cmsg);
    *fd_ptr = fd_to_send;

    ssize_t result = sendmsg(pipe_fd, &msg, 0);
    if (result < 0) {
        perror("sendmsg failed");
        return EXIT_FAILURE;
    }
    
    return EXIT_SUCCESS;
}

int recv_fd(int pipe_fd)
{
    struct msghdr msg = {0};
    struct cmsghdr *cmsg;
    char buf[1];
    struct iovec io = { .iov_base = buf, .iov_len = 1 };

    union {
        char buf[CMSG_SPACE(sizeof(int))];
        struct cmsghdr align;
    } u;

    msg.msg_iov = &io;
    msg.msg_iovlen = 1;
    msg.msg_control = u.buf;
    msg.msg_controllen = sizeof(u.buf);
    msg.msg_name = NULL;
    msg.msg_namelen = 0;

    ssize_t result = recvmsg(pipe_fd, &msg, 0);
    if (result < 0) {
        perror("recvmsg failed");
        return -1;
    }

    // Проверяем, что мы получили правильное контрольное сообщение
    cmsg = CMSG_FIRSTHDR(&msg);
    if (cmsg == NULL || cmsg->cmsg_level != SOL_SOCKET || cmsg->cmsg_type != SCM_RIGHTS) {
        fprintf(stderr, "No control message or wrong type\n");
        return -1;
    }

    int received_fd;
    memcpy(&received_fd, CMSG_DATA(cmsg), sizeof(int));
    return received_fd;
}

const char* find_lf(const char* start, const char* end) 
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    if (start > end)
        return NULL;

    const char* ptr = start;
    while (ptr < end) {
        if (ptr[0] == '\n') {
            return ptr;
        } 
        ptr++;
    }

    return NULL;
}

const char* find_crlf(const char* start, const char* end) 
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    if (start > end)
        return NULL;

    const char* ptr = start;
    while (ptr < end - 1) {
        if (ptr[0] == '\r' && ptr[1] == '\n') {
            return ptr;
        } 
        ptr++;
    }

    return NULL;
}

const char* find_double_crlf(const char* start, const char* end)
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    if (start > end)
        return NULL;

    const char* ptr = start;
    while (ptr < end - 3) {
        if (ptr[0] == '\r' && ptr[1] == '\n' && 
            ptr[2] == '\r' && ptr[3] == '\n') {
            return ptr;
        }
        ptr++;
    }
    return NULL;
}

const char* skip_space(const char* start, const char* end) 
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    const char* ptr = start;
    while (ptr < end && isspace(*ptr))
        ptr++;

    return ptr;
}

const char* skip_nonspace(const char* start, const char* end) 
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    const char* ptr = start;
    while (ptr < end && !isspace(*ptr))
        ptr++;

    return ptr;
}

char* copy_string(const char* start, const char* end) 
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    if (start > end)
        return NULL;

    size_t len = end - start;
    char* str = (char*)malloc(len + 1);
    if (IS_NULL(str)) 
        return NULL;

    memcpy(str, start, len);
    str[len] = '\0';
    return str;
}

char* dup_string(const char* string)
{
    if (IS_NULL(string))
        return NULL;

    return copy_string(string, string + strlen(string));
}

char* extract_string(const char* start, const char* end)
{
    if (IS_NULL(start))
        return NULL;

    if (IS_NULL(end))
        return NULL;

    if (start > end)
        return NULL;

    const char* string_start = skip_space(start, end);
    if (IS_NULL(string_start))
        return NULL;

    const char* string_end = skip_nonspace(string_start, end);
    if (IS_NULL(string_end))
        return NULL;

    return copy_string(string_start, string_end);
}

bool is_directory_exists(char* dir)
{
    if (IS_NULL(dir))
        return false;

    struct stat dir_stat;
    return !IS_ERROR(stat(dir, &dir_stat)) && S_ISDIR(dir_stat.st_mode);
}

bool is_file_exists(const char* filename)
{
    if (IS_NULL(filename))
        return false;

    struct stat file_stat;
    return !IS_ERROR(stat(filename, &file_stat)) && S_ISREG(file_stat.st_mode);
}

bool is_file_in_directory(const char* directory, const char* filename)
{
    if (IS_NULL(directory) || IS_NULL(filename))
        return false;

    // Проверяем, что директория существует
    if (!is_directory_exists(directory))
        return false;

    // Строим полный путь к файлу
    char full_path[PATH_MAX];
    snprintf(full_path, sizeof(full_path), "%s/%s", directory, filename);

    // Проверяем, что файл существует
    if (!is_file_exists(full_path))
        return false;

    // Дополнительная проверка через нормализацию путей (защита от path traversal)
    char resolved_path[PATH_MAX];
    char resolved_dir[PATH_MAX];
    
    // Нормализуем путь к директории
    if (realpath(directory, resolved_dir) == NULL)
        return false;

    // Нормализуем полный путь к файлу
    if (realpath(full_path, resolved_path) == NULL)
        return false;

    // Проверяем, что нормализованный путь файла начинается с нормализованного пути директории
    size_t dir_len = strlen(resolved_dir);
    if (strncmp(resolved_path, resolved_dir, dir_len) != 0)
        return false;

    // Дополнительная проверка: после пути директории должен быть / или конец строки
    if (resolved_path[dir_len] != '/' && resolved_path[dir_len] != '\0')
        return false;

    return true;
}

char* absolute_path(const char* folder, const char* filename) 
{
    if (IS_NULL(folder) || IS_NULL(filename))
        return NULL;

    if (strlen(folder) == 0 || strlen(filename) == 0)
        return NULL;

    char cwd[PATH_MAX];
    if (IS_NULL(getcwd(cwd, sizeof(cwd)))) 
        return NULL;

    size_t cwd_len = strlen(cwd);
    size_t folder_len = strlen(folder);
    size_t filename_len = strlen(filename);
    size_t total_length = cwd_len + folder_len + filename_len + 3; // +3 для двух '/' и '\0'
    
    if (total_length > PATH_MAX) 
        return NULL;

    char* absolute_path = (char*)malloc(total_length);
    if (IS_NULL(absolute_path)) 
        return NULL;

    char* ptr = absolute_path;
    
    // Копируем текущую директорию
    memcpy(ptr, cwd, cwd_len);
    ptr += cwd_len;
    
    if (cwd_len > 0 && cwd[cwd_len - 1] != '/') {
        *ptr++ = '/';
    }
    
    // Копируем папку
    memcpy(ptr, folder, folder_len);
    ptr += folder_len;
    
    if (folder_len > 0 && folder[folder_len - 1] != '/') {
        *ptr++ = '/';
    }
    
    // Копируем имя файла
    memcpy(ptr, filename, filename_len);
    ptr += filename_len;
    
    *ptr = '\0';

    return absolute_path;
}