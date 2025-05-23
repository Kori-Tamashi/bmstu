CREATE ROLE admin WITH PASSWORD 'admin';       -- Администратор
CREATE ROLE user_role WITH PASSWORD 'user';    -- Зарегистрированный пользователь
CREATE ROLE guest WITH PASSWORD 'guest';       -- Гость

-- Для администратора
GRANT ALL PRIVILEGES ON DATABASE eventor TO admin;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO admin;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO admin;

-- Для пользователя (user_role)
GRANT CONNECT ON DATABASE eventor TO user_role;

-- Права на чтение всех таблиц
GRANT SELECT ON ALL TABLES IN SCHEMA public TO user_role;

-- Права на модификацию events
GRANT INSERT, UPDATE, DELETE ON TABLE events TO user_role;

-- Права на добавление в menu, items, locations
GRANT INSERT ON TABLE menu, items, locations TO user_role;

-- Для гостя
GRANT CONNECT ON DATABASE eventor TO guest;
GRANT SELECT ON TABLE users TO guest;

-- Настройка прав по умолчанию для новых таблиц
ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT SELECT ON TABLES TO guest, user_role;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT INSERT, UPDATE, DELETE ON TABLE events TO user_role;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
GRANT INSERT ON TABLE menu, items, locations TO user_role;