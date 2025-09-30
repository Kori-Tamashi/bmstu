# Компилятор и флаги
CC = gcc
CFLAGS = 
CPPFLAGS = -I./common -I./logger -I./master -I./worker
LDFLAGS = -lpthread
DEPFLAGS = -MT $@ -MMD -MP -MF $(DEP_DIR)/$*.d

# Директории
SRC_DIRS = common logger master worker
OUT_DIR = out
BIN_DIR = bin
OBJ_DIR = $(OUT_DIR)/obj
DEP_DIR = $(OUT_DIR)/dep

# Цели сборки
SERVER_TARGET = $(BIN_DIR)/server
CLIENT_TARGET = $(BIN_DIR)/client

# Исходные файлы
COMMON_SRCS = common/utils.c
LOGGER_SRCS = logger/logger.c
MASTER_SRCS = master/connection_distributor.c master/prefork_manager.c master/main.c
WORKER_SRCS = worker/connection_handler.c worker/request_parser.c \
              worker/response_builder.c worker/file_reader.c \
			  worker/security_module.c
CLIENT_SRCS = client/client.c

# Объектные файлы
COMMON_OBJS = $(patsubst %.c,$(OBJ_DIR)/%.o,$(COMMON_SRCS))
LOGGER_OBJS = $(patsubst %.c,$(OBJ_DIR)/%.o,$(LOGGER_SRCS))
MASTER_OBJS = $(patsubst %.c,$(OBJ_DIR)/%.o,$(MASTER_SRCS))
WORKER_OBJS = $(patsubst %.c,$(OBJ_DIR)/%.o,$(WORKER_SRCS))
CLIENT_OBJS = $(patsubst %.c,$(OBJ_DIR)/%.o,$(CLIENT_SRCS))

# Все объектные файлы
ALL_OBJS = $(COMMON_OBJS) $(LOGGER_OBJS) $(MASTER_OBJS) $(WORKER_OBJS) $(CLIENT_OBJS)

# Файлы зависимостей
ALL_DEPS = $(patsubst %.c,$(DEP_DIR)/%.d,$(COMMON_SRCS) $(LOGGER_SRCS) $(MASTER_SRCS) $(WORKER_SRCS) $(CLIENT_SRCS))

# Константы выполнения
SERVER_IP = 127.0.0.1
SERVER_PORT = 8080

# Основные цели
all: $(SERVER_TARGET) $(CLIENT_TARGET)

# Сборка сервера
$(SERVER_TARGET): $(MASTER_OBJS) $(WORKER_OBJS) $(COMMON_OBJS) $(LOGGER_OBJS) | $(BIN_DIR)
	@echo "Linking server..."
	$(CC) $^ -o $@ $(LDFLAGS)
	@echo "Server built: $@"

# Сборка клиента
$(CLIENT_TARGET): $(CLIENT_OBJS) $(COMMON_OBJS) | $(BIN_DIR)
	@echo "Linking client..."
	$(CC) $^ -o $@
	@echo "Client built: $@"

# Создание директорий
$(OUT_DIR) $(OBJ_DIR) $(BIN_DIR) $(DEP_DIR):
	@mkdir -p $@

# Создание поддиректорий для объектных файлов и зависимостей
$(OBJ_DIR)/%.o: %.c | $(OUT_DIR) $(OBJ_DIR) $(DEP_DIR)
	@mkdir -p $(dir $@)
	@mkdir -p $(dir $(DEP_DIR)/$*)
	@echo "Compiling $<..."
	$(CC) $(CPPFLAGS) $(CFLAGS) $(DEPFLAGS) -c $< -o $@

%.lastbuiltstate:
	@rm -f *.lastbuiltstate
	@touch $@

# Включение файлов зависимостей
-include $(ALL_DEPS)

# Утилиты
.PHONY: clean cleanall install debug release test valgrind

# Очистка
clean:
	@echo "Cleaning object files and dependencies..."
	@rm -rf $(OUT_DIR) 

cleanall: clean
	@echo "Cleaning binaries..."
	@rm -rf $(BIN_DIR)
	@rm -f *.lastbuiltstate

# Сборка для отладки
debug: CFLAGS += -DDEBUG -O0
debug: cleanall all debug.lastbuiltstate

# Сборка для релиза
release: CFLAGS += -O2 -DNDEBUG
release: cleanall all release.lastbuiltstate

# Запуск тестов (заглушка)
test: all
	@echo "Running tests..."
	@# Здесь можно добавить запуск тестов
	@echo "Tests completed"

# Запуск с Valgrind
valgrind: debug
	@echo "Running server with Valgrind..."
	valgrind --leak-check=full --track-origins=yes $(SERVER_TARGET)

# Запуск сервера
run: all
	@echo "Starting server..."
	./$(SERVER_TARGET)

# Запуск клиента (пример)
run-client: all
	@echo "Starting client..."
	./$(CLIENT_TARGET) $(SERVER_IP) $(SERVER_PORT)

# Информация о проекте
info:
	@echo "Project Structure:"
	@echo "Source directories: $(SRC_DIRS)"
	@echo "Common sources: $(COMMON_SRCS)"
	@echo "Logger sources: $(LOGGER_SRCS)"
	@echo "Master sources: $(MASTER_SRCS)"
	@echo "Worker sources: $(WORKER_SRCS)"
	@echo "Client sources: $(CLIENT_SRCS)"
	@echo "Server target: $(SERVER_TARGET)"
	@echo "Client target: $(CLIENT_TARGET)"

# Помощь
help:
	@echo "Available targets:"
	@echo "  all       - Build server and client (default)"
	@echo "  server    - Build only server"
	@echo "  client    - Build only client"
	@echo "  clean     - Remove object files and dependencies"
	@echo "  cleanall  - Remove all generated files"
	@echo "  debug     - Build with debug flags"
	@echo "  release   - Build with optimization flags"
	@echo "  test      - Run tests"
	@echo "  valgrind  - Run with Valgrind memory checker"
	@echo "  run       - Build and run server"
	@echo "  run-client- Build and run client"
	@echo "  info      - Show project information"
	@echo "  help      - Show this help"

# Псевдонимы
server: $(SERVER_TARGET)
client: $(CLIENT_TARGET)

# Убедимся, что директории создаются перед сборкой
$(ALL_OBJS): | $(OBJ_DIR) $(DEP_DIR)

# Предотвращение удаления промежуточных файлов
.SECONDARY: $(ALL_OBJS)