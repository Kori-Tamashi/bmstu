enum perf_type_id {
    PERF_TYPE_HARDWARE        = 0,    // Аппаратные события
    PERF_TYPE_SOFTWARE        = 1,    // Программные события
    PERF_TYPE_TRACEPOINT      = 2,    // Точки трассировки
    PERF_TYPE_HW_CACHE        = 3,    // События кэша
    PERF_TYPE_RAW             = 4,    // Сырые события PMU
    PERF_TYPE_BREAKPOINT      = 5,    // Точки останова
    PERF_TYPE_MAX
};