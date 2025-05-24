-- Удаление всех объектов базы данных в правильном порядке

-----------------------------------------
-- Удаление триггеров
-----------------------------------------

DROP TRIGGER IF EXISTS trigger_create_event_days ON events CASCADE;
DROP TRIGGER IF EXISTS trigger_events_days_changed ON events_days CASCADE;
DROP TRIGGER IF EXISTS trigger_persons_days_changed ON persons_days CASCADE;
DROP TRIGGER IF EXISTS trigger_events_changed ON events CASCADE;
DROP TRIGGER IF EXISTS trigger_days_changed ON days CASCADE;
DROP TRIGGER IF EXISTS trigger_menu_items_changed ON menu_items CASCADE;
DROP TRIGGER IF EXISTS trigger_persons_days_changed ON persons_days CASCADE;
DROP TRIGGER IF EXISTS trigger_person_type_changed ON persons CASCADE;
DROP TRIGGER IF EXISTS trigger_sync_event_days ON events CASCADE;
DROP TRIGGER IF EXISTS trigger_delete_event_data ON events CASCADE;
DROP TRIGGER IF EXISTS menu_items_changed ON menu_items CASCADE;
DROP TRIGGER IF EXISTS trigger_update_event_rating ON feedbacks CASCADE;

-----------------------------------------
-- Удаление функций
-----------------------------------------

DROP FUNCTION IF EXISTS create_event_days_and_menus() CASCADE;
DROP FUNCTION IF EXISTS update_event_days_count() CASCADE;
DROP FUNCTION IF EXISTS update_event_person_count() CASCADE;
DROP FUNCTION IF EXISTS update_days_prices() CASCADE;
DROP FUNCTION IF EXISTS create_day_for_event(UUID, INT) CASCADE;
DROP FUNCTION IF EXISTS sync_event_days() CASCADE;
DROP FUNCTION IF EXISTS delete_event_related_data() CASCADE;
DROP FUNCTION IF EXISTS update_menu_cost_trigger() CASCADE;
DROP FUNCTION IF EXISTS update_event_rating() CASCADE;

-- Удаление функций из db-functions.sql
DROP FUNCTION IF EXISTS item_cost(UUID) CASCADE;
DROP FUNCTION IF EXISTS menu_cost(UUID) CASCADE;
DROP FUNCTION IF EXISTS day_cost(UUID) CASCADE;
DROP FUNCTION IF EXISTS days_n_cost(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS event_cost(UUID) CASCADE;
DROP FUNCTION IF EXISTS event_day_combinations(UUID) CASCADE;
DROP FUNCTION IF EXISTS event_day_combinations_excluding_roles(UUID) CASCADE;
DROP FUNCTION IF EXISTS day_participants_count(UUID) CASCADE;
DROP FUNCTION IF EXISTS days_participants_count(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS event_participants_count(UUID) CASCADE;
DROP FUNCTION IF EXISTS day_coefficient_1d(UUID) CASCADE;
DROP FUNCTION IF EXISTS days_coefficient_nd(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS fundamental_price_nd(UUID) CASCADE;
DROP FUNCTION IF EXISTS day_price_with_profit(UUID) CASCADE;
DROP FUNCTION IF EXISTS days_price_with_profit(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS check_balance_solution_exists(UUID) CASCADE;
DROP FUNCTION IF EXISTS check_balance_solution_exists_exact_excluding_roles(UUID) CASCADE;
DROP FUNCTION IF EXISTS get_person_count_exact(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS get_person_count_exact_excluding_roles(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS day_participants_count_excluding_roles(UUID) CASCADE;
DROP FUNCTION IF EXISTS days_participants_count_excluding_roles(UUID[]) CASCADE;
DROP FUNCTION IF EXISTS fundamental_price_nd_excluding_roles(UUID) CASCADE;
DROP FUNCTION IF EXISTS day_price_with_profit_excluding_roles(UUID) CASCADE;

-----------------------------------------
-- Удаление таблиц (с учетом зависимостей)
-----------------------------------------

-- Сначала удаляем связующие таблицы
DROP TABLE IF EXISTS 
    users_events,
    users_persons,
    events_days,
    persons_days,
    menu_items 
CASCADE;

-- Затем основные таблицы
DROP TABLE IF EXISTS 
    feedbacks,
    persons,
    days,
    events,
    menu,
    items,
    locations,
    users 
CASCADE;

-----------------------------------------
-- Удаление ENUM-типов
-----------------------------------------

DROP TYPE IF EXISTS 
    user_role_enum,
    gender_enum,
    person_type_enum,
    item_type_enum 
CASCADE;

-----------------------------------------
-- Удаление ролей
-----------------------------------------

REVOKE ALL PRIVILEGES ON DATABASE eventor FROM admin;
REVOKE ALL PRIVILEGES ON DATABASE eventor FROM user_role;
REVOKE ALL PRIVILEGES ON DATABASE eventor FROM guest;

REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM admin;
REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM user_role;
REVOKE ALL PRIVILEGES ON ALL TABLES IN SCHEMA public FROM guest;

REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM admin;
REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM user_role;
REVOKE ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public FROM guest;

REVOKE ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public FROM admin;
REVOKE ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public FROM user_role;
REVOKE ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public FROM guest;


DROP ROLE IF EXISTS admin;
DROP ROLE IF EXISTS user_role;
DROP ROLE IF EXISTS guest;