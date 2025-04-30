-----------------------------------------
-- Функции стоимости 
-----------------------------------------

-- Стоимость предмета (C_O)
CREATE OR REPLACE FUNCTION item_cost(item_id UUID) 
RETURNS NUMERIC AS $$
DECLARE
    result NUMERIC;
BEGIN
    SELECT price INTO result 
    FROM items 
    WHERE items.item_id = item_cost.item_id;
    
    RETURN COALESCE(result, 0);
END;
$$ LANGUAGE plpgsql STABLE;

-- Стоимость меню (C_M)
CREATE OR REPLACE FUNCTION menu_cost(target_menu_id UUID)
RETURNS NUMERIC AS $$
DECLARE
    total NUMERIC := 0;
BEGIN
    SELECT SUM(item_cost(i.item_id) * mi.amount)
    INTO total
    FROM menu_items mi
    JOIN items i ON mi.item_id = i.item_id
    WHERE mi.menu_id = target_menu_id;

    RETURN COALESCE(total, 0);
END;
$$ LANGUAGE plpgsql STABLE;

-- Стоимость дня (C_D)
CREATE OR REPLACE FUNCTION day_cost(target_day_id UUID)
RETURNS NUMERIC AS $$
DECLARE
    menu_id UUID;
BEGIN
    SELECT d.menu_id INTO menu_id
    FROM days d
    WHERE d.day_id = target_day_id;

    RETURN menu_cost(menu_id);
END;
$$ LANGUAGE plpgsql STABLE;

-- Стоимость дня n-го порядка (C_D)
CREATE OR REPLACE FUNCTION days_n_cost(day_ids UUID[]) 
RETURNS NUMERIC AS $$
DECLARE
    total NUMERIC := 0;
    day_id UUID;
BEGIN
    FOREACH day_id IN ARRAY day_ids LOOP
        total := total + day_cost(day_id);
    END LOOP;
    RETURN total;
END;
$$ LANGUAGE plpgsql;

-- Стоимость мероприятия (C_E)
CREATE OR REPLACE FUNCTION event_cost(target_event_id UUID)
RETURNS NUMERIC AS $$
DECLARE
    total NUMERIC := 0;
BEGIN
    SELECT SUM(day_cost(d.day_id))
    INTO total
    FROM events_days ed
    JOIN days d ON ed.day_id = d.day_id
    WHERE ed.event_id = target_event_id;

    RETURN COALESCE(total, 0);
END;
$$ LANGUAGE plpgsql STABLE;

-----------------------------------------
-- Вспомогательные функции
-----------------------------------------

-- Функция текущих комбинаций дней H(E)
CREATE OR REPLACE FUNCTION event_day_combinations(target_event_id UUID)
RETURNS SETOF UUID[] AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT combo.days_combo
    FROM (
        SELECT 
            pd.person_id,
            ARRAY_AGG(pd.day_id ORDER BY pd.day_id) FILTER (
                WHERE EXISTS (
                    SELECT 1 
                    FROM events_days ed 
                    WHERE ed.day_id = pd.day_id 
                    AND ed.event_id = target_event_id
                )
            ) AS days_combo
        FROM persons_days pd
        GROUP BY pd.person_id
    ) combo
    WHERE combo.days_combo IS NOT NULL
    AND array_length(combo.days_combo, 1) > 0;
END;
$$ LANGUAGE plpgsql STABLE;

-- Функция количества участников дня (N(d))
CREATE OR REPLACE FUNCTION day_participants_count(target_day_id UUID)
RETURNS INT AS $$
BEGIN
    RETURN (
        SELECT COUNT(DISTINCT pd.person_id)
        FROM persons_days pd
        WHERE pd.day_id = target_day_id
    );
END;
$$ LANGUAGE plpgsql STABLE;

-- Функция количества участников набора дней (n-ный порядок)
CREATE OR REPLACE FUNCTION days_participants_count(day_ids UUID[])
RETURNS INT AS $$
DECLARE
    valid_days UUID[];
BEGIN
    -- Фильтрация валидных уникальных дней
    SELECT ARRAY_AGG(DISTINCT d) INTO valid_days 
    FROM unnest(day_ids) AS d
    WHERE d IS NOT NULL;

    -- Проверка на пустой массив
    IF array_length(valid_days, 1) IS NULL THEN
        RETURN 0;
    END IF;

    RETURN (
        SELECT COUNT(*)
        FROM (
            SELECT pd.person_id
            FROM persons_days pd
            WHERE pd.day_id = ANY(valid_days)
            GROUP BY pd.person_id
            HAVING COUNT(DISTINCT pd.day_id) = array_length(valid_days, 1)
        ) AS matching_participants
    );
END;
$$ LANGUAGE plpgsql STABLE;

-- Функция количества участников мероприятия N_E(E)
CREATE OR REPLACE FUNCTION event_participants_count(target_event_id UUID)
RETURNS INT AS $$
BEGIN
    RETURN (
        SELECT COUNT(DISTINCT pd.person_id)
        FROM events_days ed
        JOIN persons_days pd ON ed.day_id = pd.day_id
        WHERE ed.event_id = target_event_id
    );
END;
$$ LANGUAGE plpgsql STABLE;

-- Функция коэффициента дня
CREATE OR REPLACE FUNCTION day_coefficient_1d(target_day_id UUID)
RETURNS NUMERIC AS $$
DECLARE
    current_day_cost NUMERIC;
    min_event_cost NUMERIC;
    event_id_var UUID; -- Переименованная переменная
BEGIN
    -- Получаем принадлежность дня к мероприятию
    SELECT ed.event_id, day_cost(d.day_id)
    INTO event_id_var, current_day_cost
    FROM days d
    JOIN events_days ed ON d.day_id = ed.day_id
    WHERE d.day_id = target_day_id;

    -- Находим минимальную стоимость в мероприятии
    SELECT MIN(day_cost(d.day_id))
    INTO min_event_cost
    FROM events_days ed
    JOIN days d ON ed.day_id = d.day_id
    WHERE ed.event_id = event_id_var; 

    -- Проверка деления на ноль
    IF min_event_cost <= 0 THEN
        RAISE EXCEPTION 'Минимальная стоимость должна быть положительной';
    END IF;

    RETURN current_day_cost / min_event_cost;
END;
$$ LANGUAGE plpgsql STABLE;


-- Функция коэффициента дня n-го порядка
CREATE OR REPLACE FUNCTION days_coefficient_nd(day_ids UUID[])
RETURNS NUMERIC AS $$
DECLARE
    total_cost NUMERIC := 0;
    min_event_cost NUMERIC;
    target_event_id UUID; 
    day_id UUID;
BEGIN
    -- Получаем event_id первого дня
    SELECT ed.event_id INTO target_event_id
    FROM events_days ed
    WHERE ed.day_id = day_ids[1]
    LIMIT 1;

    -- Проверяем принадлежность всех дней к одному мероприятию
    IF EXISTS (
        SELECT 1
        FROM unnest(day_ids) d
        LEFT JOIN events_days ed ON d = ed.day_id
        WHERE ed.event_id != target_event_id OR ed.event_id IS NULL 
    ) THEN
        RAISE EXCEPTION 'Дни принадлежат разным мероприятиям';
    END IF;

    -- Вычисляем общую стоимость и минимальную стоимость
    SELECT SUM(day_cost(d.day_id)), MIN(day_cost(d.day_id))
    INTO total_cost, min_event_cost
    FROM unnest(day_ids) AS ud
    JOIN days d ON d.day_id = ud;

    IF min_event_cost <= 0 THEN
        RAISE EXCEPTION 'Минимальная стоимость должна быть положительной';
    END IF;

    RETURN total_cost / min_event_cost;
END;
$$ LANGUAGE plpgsql STABLE;

-- Фундаментальная цена (nD случай)
CREATE OR REPLACE FUNCTION fundamental_price_nd(target_event_id UUID)
RETURNS NUMERIC AS $$
DECLARE
    total_cost NUMERIC;
    sum_an NUMERIC := 0;
    combo UUID[];
BEGIN
    total_cost := event_cost(target_event_id);

    FOR combo IN 
        SELECT * FROM event_day_combinations(target_event_id)
    LOOP
        sum_an := sum_an + 
            days_coefficient_nd(combo) * 
            days_participants_count(combo);
    END LOOP;

    IF sum_an <= 0 THEN
        RAISE EXCEPTION 'Невозможно рассчитать фундаментальную цену: суммарный коэффициент участия неположителен';
    END IF;

    RETURN total_cost / sum_an;
END;
$$ LANGUAGE plpgsql STABLE;

-- Цена дня первого порядка (с наценкой)
CREATE OR REPLACE FUNCTION day_price_with_profit(target_day_id UUID)
RETURNS NUMERIC AS $$
DECLARE
    event_percent NUMERIC;
    fundamental_p NUMERIC;
    coefficient NUMERIC;
BEGIN
    -- Получаем процент наценки и фундаментальную цену
    SELECT 
        e.percent, 
        fundamental_price_nd(e.event_id)
    INTO 
        event_percent,
        fundamental_p
    FROM events e
    JOIN events_days ed ON e.event_id = ed.event_id
    WHERE ed.day_id = target_day_id;

    -- Рассчитываем коэффициент дня
    coefficient := day_coefficient_1d(target_day_id);
    
    RETURN (1 + COALESCE(event_percent, 0)/100) * coefficient * fundamental_p;
END;
$$ LANGUAGE plpgsql STABLE;

-- Цена набора дней n-го порядка (с наценкой)
CREATE OR REPLACE FUNCTION days_price_with_profit(day_ids UUID[])
RETURNS NUMERIC AS $$
DECLARE
    event_id UUID;
    event_percent NUMERIC;
    fundamental_p NUMERIC;
    coefficient NUMERIC;
BEGIN
    -- Проверяем принадлежность дней к одному мероприятию
    SELECT ed.event_id INTO event_id
    FROM events_days ed
    WHERE ed.day_id = day_ids[1]
    LIMIT 1;

    IF EXISTS (
        SELECT 1
        FROM unnest(day_ids) d
        LEFT JOIN events_days ed ON d = ed.day_id
        WHERE ed.event_id != event_id OR ed.event_id IS NULL
    ) THEN
        RAISE EXCEPTION 'Дни принадлежат разным мероприятиям';
    END IF;

    -- Получаем процент наценки и фундаментальную цену
    SELECT 
        percent, 
        fundamental_price_nd(event_id)
    INTO 
        event_percent,
        fundamental_p
    FROM events
    WHERE event_id = event_id;

    -- Рассчитываем коэффициент для набора дней
    coefficient := days_coefficient_nd(day_ids);
    
    RETURN (1 + COALESCE(event_percent, 0)/100) * coefficient * fundamental_p;
END;
$$ LANGUAGE plpgsql STABLE;

--
CREATE OR REPLACE FUNCTION check_balance_solution_exists(target_event_id UUID)
RETURNS BOOLEAN AS $$
DECLARE
    has_positive_costs BOOLEAN;
    has_participants BOOLEAN;
BEGIN
    -- Проверка что все дни имеют положительную стоимость
    SELECT NOT EXISTS (
        SELECT 1
        FROM events_days ed
        JOIN days d ON ed.day_id = d.day_id
        WHERE ed.event_id = target_event_id
          AND day_cost(d.day_id) <= 0
    ) INTO has_positive_costs;

    -- Проверка наличия участников
    SELECT EXISTS (
        SELECT 1
        FROM persons_days pd
        JOIN events_days ed ON pd.day_id = ed.day_id
        WHERE ed.event_id = target_event_id
        LIMIT 1
    ) INTO has_participants;

    -- Проверка что мероприятие существует и имеет дни
    PERFORM 1
    FROM events
    WHERE event_id = target_event_id
      AND EXISTS (
          SELECT 1
          FROM events_days
          WHERE event_id = target_event_id
      );

    IF NOT FOUND THEN
        RETURN FALSE;
    END IF;

    RETURN has_positive_costs AND has_participants;
END;
$$ LANGUAGE plpgsql STABLE;