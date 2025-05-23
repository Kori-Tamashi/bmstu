-----------------------------------------
-- Events
-----------------------------------------

-- Создание

-- Триггер для автоматического создания связанных данных при создании мероприятия
CREATE OR REPLACE FUNCTION create_event_days_and_menus()
RETURNS TRIGGER AS $$
DECLARE
    day_counter INT;
    new_day_id UUID;
    new_menu_id UUID;
BEGIN
    FOR day_counter IN 1..NEW.days_count LOOP
        -- Создаем новое меню для дня
        new_menu_id := gen_random_uuid();
        INSERT INTO menu (menu_id, name, cost)
        VALUES (new_menu_id, 'Меню для дня ' || day_counter || ' - ' || NEW.name, 0);
        
        -- Создаем новый день
        new_day_id := gen_random_uuid();
        INSERT INTO days (day_id, menu_id, name, sequence_number, description, price)
        VALUES (new_day_id, new_menu_id, 
                'День ' || day_counter || ' - ' || NEW.name,
                day_counter,
                'День мероприятия: ' || NEW.name,
                0);
        
        -- Связываем день с мероприятием
        INSERT INTO events_days (event_id, day_id)
            VALUES (NEW.event_id, new_day_id);
    END LOOP;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_create_event_days
AFTER INSERT ON events
FOR EACH ROW
EXECUTE FUNCTION create_event_days_and_menus();

---- Изменение существующих данных ----

-- Триггер для обновления количества дней мероприятия
CREATE OR REPLACE FUNCTION update_event_days_count()
RETURNS TRIGGER AS $$
DECLARE
    target_event_id UUID;
BEGIN
    -- Определяем мероприятие, для которого произошли изменения
    IF (TG_OP = 'INSERT') THEN
        target_event_id := NEW.event_id;
    ELSIF (TG_OP = 'DELETE') THEN
        target_event_id := OLD.event_id;
    END IF;

    -- Пересчитываем количество дней и обновляем events.days_count
    UPDATE events
    SET days_count = (
        SELECT COUNT(day_id)
        FROM events_days
        WHERE event_id = target_event_id
    )
    WHERE event_id = target_event_id;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_events_days_changed
AFTER INSERT OR DELETE ON events_days
FOR EACH ROW
EXECUTE FUNCTION update_event_days_count();


-- Триггер для обновления количества участников мероприятия
CREATE OR REPLACE FUNCTION update_event_person_count()
RETURNS TRIGGER AS $$
DECLARE
    target_event_id UUID;
    person_count_new INT;
BEGIN
    -- Определяем мероприятие, связанное с днем
    IF (TG_OP = 'INSERT') THEN
        SELECT event_id INTO target_event_id
        FROM events_days
        WHERE day_id = NEW.day_id;

        -- Проверяем, есть ли уже участник в этом мероприятии
        SELECT COUNT(DISTINCT person_id) INTO person_count_new
        FROM persons_days pd
        JOIN events_days ed ON pd.day_id = ed.day_id
        WHERE ed.event_id = target_event_id;

        -- Обновляем счетчик
        UPDATE events
        SET person_count = person_count_new
        WHERE event_id = target_event_id;

    ELSIF (TG_OP = 'DELETE') THEN
        SELECT event_id INTO target_event_id
        FROM events_days
        WHERE day_id = OLD.day_id;

        -- Пересчитываем всех участников мероприятия
        SELECT COUNT(DISTINCT person_id) INTO person_count_new
        FROM persons_days pd
        JOIN events_days ed ON pd.day_id = ed.day_id
        WHERE ed.event_id = target_event_id;

        -- Обновляем счетчик
        UPDATE events
        SET person_count = person_count_new
        WHERE event_id = target_event_id;

    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_persons_days_changed
AFTER INSERT OR UPDATE OR DELETE ON persons_days
FOR EACH ROW
EXECUTE FUNCTION update_event_person_count();

-- Триггерная функция для обновления цен дней мероприятия
CREATE OR REPLACE FUNCTION update_days_prices()
RETURNS TRIGGER AS $$
DECLARE
    affected_event_id UUID;
    day_record RECORD;
BEGIN
    -- Определяем связанное мероприятие в зависимости от операции
    IF TG_TABLE_NAME = 'events' THEN
        affected_event_id := NEW.event_id;
    ELSIF TG_TABLE_NAME = 'days' THEN
        affected_event_id := (SELECT event_id FROM events_days WHERE day_id = NEW.day_id);
    ELSIF TG_TABLE_NAME = 'menu_items' THEN
        IF TG_OP = 'DELETE' THEN
            affected_event_id := (
                SELECT ed.event_id 
                FROM days d
                JOIN events_days ed ON d.day_id = ed.day_id 
                WHERE d.menu_id = OLD.menu_id
            );
        ELSE
            affected_event_id := (
                SELECT ed.event_id 
                FROM days d
                JOIN events_days ed ON d.day_id = ed.day_id 
                WHERE d.menu_id = NEW.menu_id
            );
        END IF;
    ELSIF TG_TABLE_NAME = 'persons_days' THEN
        affected_event_id := (
            SELECT event_id 
            FROM events_days 
            WHERE day_id = NEW.day_id OR day_id = OLD.day_id
            LIMIT 1
        );
    ELSIF TG_TABLE_NAME = 'persons' THEN
        affected_event_id := (
            SELECT ed.event_id
            FROM persons_days pd
            JOIN events_days ed ON pd.day_id = ed.day_id
            WHERE pd.person_id = NEW.person_id
            LIMIT 1
        );
    END IF;

    IF check_balance_solution_exists_exact_excluding_roles(affected_event_id) THEN
        FOR day_record IN
            SELECT d.day_id 
            FROM days d
            JOIN events_days ed ON d.day_id = ed.day_id
            WHERE ed.event_id = affected_event_id
        LOOP
            UPDATE days 
            SET price = day_price_with_profit_excluding_roles(day_record.day_id)
            WHERE day_id = day_record.day_id;
        END LOOP;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_events_changed
AFTER INSERT OR UPDATE ON events
FOR EACH ROW
EXECUTE FUNCTION update_days_prices();

CREATE TRIGGER trigger_days_changed
AFTER UPDATE OF menu_id ON days
FOR EACH ROW
EXECUTE FUNCTION update_days_prices();

CREATE TRIGGER trigger_menu_items_changed
AFTER INSERT OR UPDATE OR DELETE ON menu_items
FOR EACH ROW 
EXECUTE FUNCTION update_days_prices();

CREATE TRIGGER trigger_persons_days_changed
AFTER INSERT OR DELETE ON persons_days
FOR EACH ROW
EXECUTE FUNCTION update_days_prices();

CREATE TRIGGER trigger_person_type_changed
AFTER INSERT OR DELETE OR UPDATE OF type ON persons
FOR EACH ROW
EXECUTE FUNCTION update_days_prices();

CREATE OR REPLACE FUNCTION create_day_for_event(event_id UUID, seq_num INT)
RETURNS VOID AS $$
DECLARE
    new_menu_id UUID := gen_random_uuid();
    new_day_id UUID := gen_random_uuid();
    event_name VARCHAR(255); 
BEGIN
    -- Получаем название мероприятия
    SELECT name INTO event_name 
    FROM events 
    WHERE events.event_id = create_day_for_event.event_id;

    -- Создаем меню 
    INSERT INTO menu (menu_id, name, cost) 
    VALUES (
        new_menu_id, 
        'Меню для дня ' || seq_num || ' - ' || event_name, -- Формат: "Меню для дня X - Название"
        0
    );

    -- Создаем день
    INSERT INTO days (day_id, menu_id, name, sequence_number, description, price)
    VALUES (
        new_day_id,
        new_menu_id,
        'День ' || seq_num || ' - ' || event_name, -- Формат: "День X - Название"
        seq_num,
        'День мероприятия: ' || event_name,
        0
    );

    -- Связываем с мероприятием
    INSERT INTO events_days (event_id, day_id)
    VALUES (event_id, new_day_id);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sync_event_days()
RETURNS TRIGGER AS $$
DECLARE
    current_days INT;
    max_sequence INT;
    days_to_remove UUID[];
BEGIN
    -- Если days_count не изменился или это новая запись, завершаем работу
    IF TG_OP = 'INSERT' THEN
        RETURN NEW; -- Существующий триггер trigger_create_event_days обработает создание
    ELSIF NEW.days_count = OLD.days_count THEN
        RETURN NEW;
    END IF;

    -- Получаем текущее количество дней и максимальный sequence_number
    SELECT COUNT(*), MAX(sequence_number) 
    INTO current_days, max_sequence
    FROM events_days
    JOIN days USING (day_id)
    WHERE event_id = NEW.event_id;

    -- Добавление дней при увеличении days_count
    IF NEW.days_count > current_days THEN
        FOR i IN 1..(NEW.days_count - current_days) LOOP
            PERFORM create_day_for_event(NEW.event_id, max_sequence + i);
        END LOOP;

    -- Удаление дней и связанных данных при уменьшении days_count
    ELSIF NEW.days_count < current_days THEN
        -- Собираем ID дней для удаления
        SELECT ARRAY(
            SELECT day_id 
            FROM events_days 
            JOIN days USING (day_id)
            WHERE event_id = NEW.event_id
            ORDER BY sequence_number DESC
            LIMIT (current_days - NEW.days_count)
        ) INTO days_to_remove;

        -- Удаляем связи участников с днями
        DELETE FROM persons_days WHERE day_id = ANY(days_to_remove);

        -- Явное удаление меню
        DELETE FROM menu 
        WHERE menu_id IN (
            SELECT menu_id FROM days WHERE day_id = ANY(days_to_remove)
        );

		-- Удаляем дни 
        DELETE FROM days WHERE day_id = ANY(days_to_remove);
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_sync_event_days
AFTER INSERT OR UPDATE OF days_count ON events
FOR EACH ROW
EXECUTE FUNCTION sync_event_days();

---- Удаление ----

CREATE OR REPLACE FUNCTION delete_event_related_data()
RETURNS TRIGGER AS $$
DECLARE
    event_days UUID[]; 
	days_menu UUID[];
BEGIN
    SELECT ARRAY(SELECT day_id FROM events_days WHERE event_id = OLD.event_id) 
    INTO event_days;

	SELECT ARRAY(SELECT menu_id 
				 FROM days d
				 JOIN events_days ed ON d.day_id = ed.day_id
				 WHERE ed.event_id = OLD.event_id)
	INTO days_menu;

	ALTER TABLE persons_days DISABLE TRIGGER ALL;
	ALTER TABLE events_days DISABLE TRIGGER ALL;

    -- Удаляем участников мероприятия
    DELETE FROM persons 
    WHERE person_id IN (
        SELECT person_id 
        FROM persons_days 
        WHERE day_id = ANY(event_days)
    );

    -- Удаляем дни мероприятия
    DELETE FROM days 
    WHERE day_id = ANY(event_days);

    -- Удаляем меню дней мероприятия
    DELETE FROM menu 
    WHERE menu_id = ANY(days_menu);

	ALTER TABLE persons_days ENABLE TRIGGER ALL;
	ALTER TABLE events_days ENABLE TRIGGER ALL;

    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_delete_event_data
BEFORE DELETE ON events
FOR EACH ROW
EXECUTE FUNCTION delete_event_related_data();

-----------------------------------------
-- Other
-----------------------------------------

-- Триггер для автоматического обновления стоимости меню
CREATE OR REPLACE FUNCTION update_menu_cost_trigger()
RETURNS TRIGGER AS $$
DECLARE
    affected_menu_id UUID;
BEGIN
    -- Определяем какой меню был изменен
    IF (TG_OP = 'DELETE') THEN
        affected_menu_id := OLD.menu_id;
    ELSE
        affected_menu_id := NEW.menu_id;
    END IF;

    -- Обновляем стоимость меню
    UPDATE menu 
    SET cost = menu_cost(affected_menu_id)
    WHERE menu_id = affected_menu_id;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER menu_items_changed
AFTER INSERT OR UPDATE OR DELETE ON menu_items
FOR EACH ROW EXECUTE FUNCTION update_menu_cost_trigger();


-- Триггерная функция для обновления среднего рейтинга мероприятия
CREATE OR REPLACE FUNCTION update_event_rating()
RETURNS TRIGGER AS $$
DECLARE
    target_event_ids UUID[];
    current_event_id UUID;
    avg_rating NUMERIC;
BEGIN
    IF (TG_OP = 'INSERT') THEN
        target_event_ids := ARRAY[NEW.event_id];
    ELSIF (TG_OP = 'UPDATE') THEN
        target_event_ids := ARRAY[OLD.event_id, NEW.event_id];
    ELSIF (TG_OP = 'DELETE') THEN
        target_event_ids := ARRAY[OLD.event_id];
    END IF;

    FOREACH current_event_id IN ARRAY ARRAY(SELECT DISTINCT unnest(target_event_ids)) LOOP
        SELECT COALESCE(ROUND(AVG(rating)::NUMERIC, 2), 10.00)
        INTO avg_rating
        FROM feedbacks
        WHERE event_id = current_event_id;

        UPDATE events
        SET rating = avg_rating
        WHERE event_id = current_event_id;
    END LOOP;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_event_rating
AFTER INSERT OR UPDATE OR DELETE ON feedbacks
FOR EACH ROW
EXECUTE FUNCTION update_event_rating();