-- Создание ENUM-типов
CREATE TYPE user_role_enum AS ENUM ('Администратор', 'Зарегистрированный пользователь', 'Гость');
CREATE TYPE gender_enum AS ENUM ('Мужчина', 'Женщина');
CREATE TYPE person_type_enum AS ENUM ('Организатор', 'VIP-персона', 'Простой участник');
CREATE TYPE item_type_enum AS ENUM ('Однодневный', 'Многодневный');

-- Таблица пользователей (Users)
CREATE TABLE users (
    user_id UUID,
    name VARCHAR(255),
    phone VARCHAR(255),
    gender gender_enum,
    password VARCHAR(255),
    role user_role_enum
);

-- Таблица локаций (Locations)
CREATE TABLE locations (
    location_id UUID,
    name VARCHAR(255),
    description TEXT,
    price NUMERIC(14,2),
    capacity INT
);

-- Таблица мероприятий (Events)
CREATE TABLE events (
    event_id UUID,
    location_id UUID,
    name VARCHAR(255),
    description TEXT,
    date DATE,
    person_count INT,
    days_count INT,
    percent NUMERIC(14,4),
    rating NUMERIC(14,4)
);

-- Таблица дней мероприятий (Days)
CREATE TABLE days (
    day_id UUID,
    menu_id UUID,
    name VARCHAR(255),
    sequence_number INT,
    description TEXT,
    price NUMERIC(14,4)
);

-- Таблица участников (Persons)
CREATE TABLE persons (
    person_id UUID,
    name VARCHAR(255),
    type person_type_enum,
    paid BOOLEAN
);

-- Таблица меню (Menu)
CREATE TABLE menu (
    menu_id UUID,
    name VARCHAR(255),
    cost NUMERIC(14,4)
);

-- Таблица предметов меню (Items)
CREATE TABLE items (
    item_id UUID,
    name VARCHAR(255),
    type item_type_enum,
    price NUMERIC(14,4)
);

-- Таблица отзывов (Feedbacks)
CREATE TABLE feedbacks (
    feedback_id UUID,
    event_id UUID,
    person_id UUID,
    comment TEXT,
    rating NUMERIC(14,4)
);

-- Связь пользователей и мероприятий (User Events)
CREATE TABLE users_events (
    user_id UUID,
    event_id UUID
);

-- Связь пользователей и участников (User Persons)
CREATE TABLE users_persons (
    user_id UUID,
    person_id UUID
);

-- Связь дней и мероприятий (Days Events)
CREATE TABLE events_days (
    event_id UUID,
    day_id UUID
);

-- Связь участников и дней мероприятий (Person's Days)
CREATE TABLE persons_days (
    person_id UUID,
    day_id UUID
);

-- Связь меню и предметов меню (Menu Items)
CREATE TABLE menu_items (
    menu_id UUID,
    item_id UUID,
    amount INT
);

