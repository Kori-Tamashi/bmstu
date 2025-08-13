-- db-insert.sql
-- Универсальный скрипт для заполнения БД с учетом триггеров

-------------------------------
-- 1. Очистка данных (опционально)
-------------------------------
-- TRUNCATE TABLE ... CASCADE; 

-------------------------------
-- 2. Вставка пользователей
-------------------------------
INSERT INTO users (user_id, name, phone, gender, password, role) VALUES
-- Администратор
('6ba7b810-9dad-11d1-80b4-00c04fd430c8', 'Иван Петров', '+79191234567', 'Мужчина', 'adminpass', 'Администратор'),
-- Обычный пользователь
('6ba7b811-9dad-11d1-80b4-00c04fd430c8', 'Мария Сидорова', '+79197654321', 'Женщина', 'userpass', 'Зарегистрированный пользователь');

-------------------------------
-- 3. Вставка локаций
-------------------------------
INSERT INTO locations (location_id, name, description, price, capacity) VALUES
('6ba7b812-9dad-11d1-80b4-00c04fd430c8', 'Конференц-зал "Столица"', 'Зал на 200 человек с проектором', 50000, 200),
('6ba7b813-9dad-11d1-80b4-00c04fd430c8', 'Ресторан "Золотой дракон"', 'Банкетный зал с кухней', 100000, 150);

-------------------------------
-- 4. Вставка предметов меню
-------------------------------
INSERT INTO items (item_id, name, type, price) VALUES
('6ba7b814-9dad-11d1-80b4-00c04fd430c8', 'Кофе', 'Однодневный', 150),
('6ba7b815-9dad-11d1-80b4-00c04fd430c8', 'Фуршет "Премиум"', 'Многодневный', 2000);

-------------------------------
-- 5. Вставка мероприятий (триггер создаст дни, меню и свяжет их через days)
-------------------------------
INSERT INTO events (event_id, location_id, name, description, date, person_count, days_count, percent, rating) VALUES
('6ba7b816-9dad-11d1-80b4-00c04fd430c8', '6ba7b812-9dad-11d1-80b4-00c04fd430c8', 
'IT-конференция 2023', 'Ежегодная конференция разработчиков', '2023-11-15', 0, 3, 20, 9.5);

-- После выполнения триггера:
-- 1. В таблице days будут 3 записи с menu_id
-- 2. В events_days будут связи event_id-day_id
-- 3. В menu будут 3 новых меню

-------------------------------
-- 6. Заполнение меню (корректный способ через days)
-------------------------------
WITH event_data AS (
    SELECT d.day_id, d.menu_id 
    FROM events_days ed
    JOIN days d ON ed.day_id = d.day_id
    WHERE ed.event_id = '6ba7b816-9dad-11d1-80b4-00c04fd430c8'
)
INSERT INTO menu_items (menu_id, item_id, amount)
SELECT menu_id, item_id, 
    CASE WHEN items.type = 'Однодневный' THEN 50 ELSE 10 END
FROM event_data
CROSS JOIN items;

-- Триггер menu_items_changed обновит cost в menu автоматически

-------------------------------
-- 7. Вставка участников
-------------------------------
INSERT INTO persons (person_id, name, type, paid) VALUES
('6ba7b817-9dad-11d1-80b4-00c04fd430c8', 'Алексей Иванов', 'Организатор', TRUE),
('6ba7b818-9dad-11d1-80b4-00c04fd430c8', 'Ольга Николаева', 'VIP-персона', TRUE);

-------------------------------
-- 8. Связь пользователей с участниками
-------------------------------
INSERT INTO users_persons (user_id, person_id) VALUES
('6ba7b810-9dad-11d1-80b4-00c04fd430c8', '6ba7b817-9dad-11d1-80b4-00c04fd430c8'),
('6ba7b811-9dad-11d1-80b4-00c04fd430c8', '6ba7b818-9dad-11d1-80b4-00c04fd430c8');

-------------------------------
-- 9. Связь участников с днями (триггер обновит person_count)
-------------------------------
INSERT INTO persons_days (person_id, day_id)
-- Получаем первые два дня мероприятия
SELECT p.person_id, ed.day_id 
FROM persons p
CROSS JOIN (
    SELECT day_id 
    FROM events_days 
    WHERE event_id = '6ba7b816-9dad-11d1-80b4-00c04fd430c8'
    LIMIT 2
) ed;

-- Триггеры trigger_persons_days_insert и update_event_person_count обновят person_count в events

-------------------------------
-- 10. Вставка отзывов
-------------------------------
INSERT INTO feedbacks (feedback_id, event_id, person_id, comment, rating) VALUES
('6ba7b819-9dad-11d1-80b4-00c04fd430c8', '6ba7b816-9dad-11d1-80b4-00c04fd430c8', 
'6ba7b818-9dad-11d1-80b4-00c04fd430c8', 'Отличная организация!', 10);

-------------------------------
-- Проверка работы триггеров
-------------------------------
-- Проверка созданных дней
SELECT * FROM days WHERE menu_id IN (
    SELECT menu_id FROM events_days WHERE event_id = '6ba7b816-9dad-11d1-80b4-00c04fd430c8'
);

-- Проверка стоимости меню (должна быть 10*150 + 10*2000 = 21500)
SELECT * FROM menu;

-- Проверка количества участников (должно быть 2)
SELECT person_count FROM events WHERE event_id = '6ba7b816-9dad-11d1-80b4-00c04fd430c8';

-- Проверка количества дней (должно быть 3)
SELECT days_count FROM events WHERE event_id = '6ba7b816-9dad-11d1-80b4-00c04fd430c8';