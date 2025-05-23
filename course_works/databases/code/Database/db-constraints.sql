-- Ограничения для таблицы пользователей (Users)
ALTER TABLE users
    ADD PRIMARY KEY (user_id),
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN phone SET NOT NULL,
    ALTER COLUMN gender SET NOT NULL,
    ALTER COLUMN password SET NOT NULL,
    ALTER COLUMN role SET NOT NULL,
    ADD CONSTRAINT unique_phone UNIQUE (phone);

-- Ограничения для таблицы локаций (Locations)
ALTER TABLE locations
    ADD PRIMARY KEY (location_id),
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN description SET NOT NULL,
    ALTER COLUMN price SET NOT NULL,
    ALTER COLUMN capacity SET NOT NULL,
    ADD CHECK (price >= 0),
    ADD CHECK (capacity >= 0);

-- Ограничения для таблицы мероприятий (Events)
ALTER TABLE events
    ADD PRIMARY KEY (event_id),
    ALTER COLUMN location_id SET NOT NULL,
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN description SET NOT NULL,
    ALTER COLUMN date SET NOT NULL,
    ALTER COLUMN person_count SET NOT NULL,
    ALTER COLUMN days_count SET NOT NULL,
    ADD CHECK (days_count > 0),
    ADD CHECK (person_count >= 0),
    ADD CHECK (percent >= 0),
    ADD CHECK (rating BETWEEN 0 AND 10);

-- Внешние ключи для мероприятий
ALTER TABLE events
    ADD FOREIGN KEY (location_id) REFERENCES locations(location_id);

-- Ограничения для таблицы меню (Menu)
ALTER TABLE menu
    ADD PRIMARY KEY (menu_id),
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN cost SET NOT NULL,
    ADD CHECK (cost >= 0);

-- Ограничения для таблицы дней (Days)
ALTER TABLE days
    ADD PRIMARY KEY (day_id),
    ALTER COLUMN menu_id SET NOT NULL,
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN sequence_number SET NOT NULL,
    ALTER COLUMN description SET NOT NULL,
    ALTER COLUMN price SET NOT NULL,
    ADD CHECK (sequence_number > 0),
    ADD CHECK (price >= 0);

-- Внешние ключи для дней
ALTER TABLE days
    ADD FOREIGN KEY (menu_id) REFERENCES menu(menu_id);

-- Ограничения для таблицы участников (Persons)
ALTER TABLE persons
    ADD PRIMARY KEY (person_id),
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN type SET NOT NULL,
    ALTER COLUMN paid SET NOT NULL;

-- Ограничения для таблицы предметов (Items)
ALTER TABLE items
    ADD PRIMARY KEY (item_id),
    ALTER COLUMN name SET NOT NULL,
    ALTER COLUMN type SET NOT NULL,
    ALTER COLUMN price SET NOT NULL,
    ADD CHECK (price >= 0);

-- Ограничения для таблицы отзывов (Feedback)
ALTER TABLE feedbacks
    ADD PRIMARY KEY (feedback_id),
    ALTER COLUMN event_id SET NOT NULL,
    ALTER COLUMN person_id SET NOT NULL,
    ALTER COLUMN comment SET NOT NULL,
    ALTER COLUMN rating SET NOT NULL,
    ADD CHECK (rating BETWEEN 0 AND 10);

-- Внешние ключи для отзывов
ALTER TABLE feedbacks
    ADD FOREIGN KEY (event_id) REFERENCES events(event_id) ON DELETE CASCADE,
    ADD FOREIGN KEY (person_id) REFERENCES persons(person_id) ON DELETE CASCADE;

-- Ограничения для таблицы User Events
ALTER TABLE users_events
    ADD PRIMARY KEY (user_id, event_id),
    ADD FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    ADD FOREIGN KEY (event_id) REFERENCES events(event_id) ON DELETE CASCADE;

-- Ограничения для таблицы User Persons
ALTER TABLE users_persons
    ADD PRIMARY KEY (user_id, person_id),
    ADD FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    ADD FOREIGN KEY (person_id) REFERENCES persons(person_id) ON DELETE CASCADE;

-- Ограничения для таблицы Events Days
ALTER TABLE events_days
    ADD PRIMARY KEY (event_id, day_id),
    ADD FOREIGN KEY (event_id) REFERENCES events(event_id) ON DELETE CASCADE,
    ADD FOREIGN KEY (day_id) REFERENCES days(day_id) ON DELETE CASCADE;

-- Ограничения для таблицы Person's Days
ALTER TABLE persons_days
    ADD PRIMARY KEY (person_id, day_id),
    ADD FOREIGN KEY (person_id) REFERENCES persons(person_id) ON DELETE CASCADE,
    ADD FOREIGN KEY (day_id) REFERENCES days(day_id) ON DELETE CASCADE;

-- Ограничения для таблицы Menu Items
ALTER TABLE menu_items
    ADD PRIMARY KEY (menu_id, item_id),
    ADD FOREIGN KEY (menu_id) REFERENCES menu(menu_id) ON DELETE CASCADE,
    ADD FOREIGN KEY (item_id) REFERENCES items(item_id) ON DELETE CASCADE,
    ADD CHECK (amount > 0);