-- Удаление таблиц (в порядке зависимостей)
DROP TABLE IF EXISTS events_days;
DROP TABLE IF EXISTS menu_items;
DROP TABLE IF EXISTS persons_days;
DROP TABLE IF EXISTS users_events;
DROP TABLE IF EXISTS users_persons;
DROP TABLE IF EXISTS feedbacks;
DROP TABLE IF EXISTS items;
DROP TABLE IF EXISTS days;
DROP TABLE IF EXISTS menu;
DROP TABLE IF EXISTS persons;
DROP TABLE IF EXISTS events;
DROP TABLE IF EXISTS locations;
DROP TABLE IF EXISTS users;

-- Удаление ENUM-типов
DROP TYPE IF EXISTS user_role_enum;
DROP TYPE IF EXISTS gender_enum;
DROP TYPE IF EXISTS person_type_enum;
DROP TYPE IF EXISTS item_type_enum;