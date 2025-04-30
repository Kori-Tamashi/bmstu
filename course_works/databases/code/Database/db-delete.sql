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

-- Удаление триггеров
DROP TRIGGER IF EXISTS trigger_create_event_days ON events;
DROP TRIGGER IF EXISTS trigger_delete_event_data ON events;
DROP TRIGGER IF EXISTS menu_items_changed ON menu_items;
DROP TRIGGER IF EXISTS trigger_persons_days_insert ON persons_days;
DROP TRIGGER IF EXISTS trigger_persons_days_delete ON persons_days;
DROP TRIGGER IF EXISTS trigger_events_changed ON events;
DROP TRIGGER IF EXISTS trigger_days_changed ON days;
DROP TRIGGER IF EXISTS trigger_menu_items_changed ON menu_items;
DROP TRIGGER IF EXISTS trigger_persons_days_changed ON persons_days;

-- Удаление функций
DROP FUNCTION IF EXISTS update_day_cost();
DROP FUNCTION IF EXISTS update_event_days_count();
DROP FUNCTION IF EXISTS update_menu_cost();
DROP FUNCTION IF EXISTS create_event_days_and_menus();
DROP FUNCTION IF EXISTS delete_event_related_data();
DROP FUNCTION IF EXISTS update_menu_cost_trigger;
DROP FUNCTION IF EXISTS update_event_person_count;
DROP FUNCTION IF EXISTS event_cost(UUID);
DROP FUNCTION IF EXISTS day_cost(UUID);
DROP FUNCTION IF EXISTS menu_cost(UUID);
DROP FUNCTION IF EXISTS item_cost(UUID);
DROP FUNCTION IF EXISTS event_day_combinations(UUID);
DROP FUNCTION IF EXISTS day_participants_count(UUID);
DROP FUNCTION IF EXISTS days_participants_count(UUID[]);
DROP FUNCTION IF EXISTS event_participants_count(UUID);
DROP FUNCTION IF EXISTS day_coefficient_1d(UUID);
DROP FUNCTION IF EXISTS days_coefficient_nd(UUID[]);
DROP FUNCTION IF EXISTS fundamental_price_1d(UUID);
DROP FUNCTION IF EXISTS fundamental_price_nd(UUID);
DROP FUNCTION IF EXISTS day_price_with_profit(UUID);
DROP FUNCTION IF EXISTS days_price_with_profit(UUID[]);
DROP FUNCTION IF EXISTS check_balance_solution_exists(UUID);
DROP FUNCTION IF EXISTS calculate_day_cost(UUID);
DROP FUNCTION IF EXISTS calculate_menu_cost(UUID);
DROP FUNCTION IF EXISTS days_n_cost(UUID[]);
DROP FUNCTION IF EXISTS update_days_prices;
