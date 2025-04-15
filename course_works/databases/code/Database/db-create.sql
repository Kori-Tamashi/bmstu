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
    price NUMERIC,
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
    percent NUMERIC,
    rating NUMERIC
);

-- Таблица дней мероприятий (Days)
CREATE TABLE days (
    day_id UUID,
    menu_id UUID,
    name VARCHAR(255),
    sequence_number INT,
    description TEXT,
    price NUMERIC
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
    cost NUMERIC
);

-- Таблица предметов меню (Items)
CREATE TABLE items (
    item_id UUID,
    name VARCHAR(255),
    type item_type_enum,
    price NUMERIC
);

-- Таблица отзывов (Feedbacks)
CREATE TABLE feedbacks (
    feedback_id UUID,
    event_id UUID,
    person_id UUID,
    comment TEXT,
    rating NUMERIC
);

-- Связь пользователей и мероприятий (User Events)
CREATE TABLE users_events (
    user_id UUID,
    event_id UUID
);

-- Связь дней и мероприятий (Days Events)
CREATE TABLE events_days (
    event_id UUID,
    day_id UUID
);

-- Связь участников и дней мероприятий (Person's Days)
CREATE TABLE persons_days (
    person_id UUID,
    day_id UUID,
    menu_id UUID
);

-- Связь меню и предметов меню (Menu Items)
CREATE TABLE menu_items (
    menu_id UUID,
    item_id UUID,
    amount INT
);

-- INSERT INTO users VALUES('f0fe5f0b-cfad-4caf-acaf-f6685c3a5fc6', 'Misha', '+7 (919) 406-8111', 'Мужчина', 'pupirkalove', 'Администратор');