struct perf_event {
    struct list_head event_entry;
    struct hlist_node hlist_entry;
    struct perf_event_attr attr;        // Атрибуты события
    struct perf_event_context *ctx;     // Контекст события
    enum perf_event_state state;        // Состояние события
    atomic64_t count;                   // Значение счетчика
    // ... другие поля
};