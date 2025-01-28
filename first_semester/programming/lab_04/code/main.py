# <имя> <фамилия> <группа>
# Программа для вывода таблицы значений функций q1, q2 и q3, поиска максимума функции q2
# и построения графика функции q1 на заданном отрезке

from math import log1p

flt_inpt_err_msg = 'Ошибка: введите вещественное числовое значение'

table_width = 75    # ширина таблицы значений
graph_width = 150   # ширина графика функции q1

# Ввод входных значений
while True:
    try:
        r_0 = float(input('Введите начальное значение координаты X: '))
        break
    except ValueError:
        print(flt_inpt_err_msg)

while True:
    try:
        r_n = float(input('Введите конечное значение координаты X: '))
        break
    except ValueError:
        print(flt_inpt_err_msg)

while True:
    try:
        h = float(input('Введите значение шага координаты X: '))
        break
    except ValueError:
        print(flt_inpt_err_msg)


# Вывод таблицы значений функций q1, q2 и q3
full_column_width = table_width - 13
column_width = full_column_width // 4
separator = '-' * (table_width - (full_column_width % 4))
header_format = (f"| {'r':^{column_width}} | {'q1':^{column_width}} | {'q2':^{column_width}} "
                 f"| {'q3':^{column_width}} |")

print(separator)
print(header_format)
print(separator)

q2_positives_sum = 0        # сумма положительных значений функции q_2
q1_max = float('-Inf')      # максимальное значение функции q_1
q1_min = float('Inf')       # минимальное значение функции q_1

r = r_0
steps = (r_n - r_0) / h
for _ in range(int(steps) + 1):
    q1 = r ** 3 - 5.57 * r ** 2 - 193 * r - 633.1

    if q1_max < q1:
        q1_max = q1
        q1_max_r = r
    if q1_min > q1:
        q1_min = q1

    if r > 0:
        q2 = r * log1p(r) - 52
        if q2 > 0:
            q2_positives_sum += q2
        q3 = (q1 ** 3 - q2 ** 3) / 1000
    else:
        q2 = float('nan')
        q3 = float('nan')

    row_format = (f"| {r:^{column_width}g} | {q1:^{column_width}g} | {q2:^{column_width}g} "
                  f"| {q3:^{column_width}g} |")
    print(row_format)

    r += h

print(separator)


# Ввод количества засечек
while True:
    try:
        serifs_count = int(input('Введите количество засечек (от 4-х до 8-ми): '))
        if 4 <= serifs_count <= 8:
            break
        else:
            print('Ошибка: количество засечек должно быть от 4-х до 8-ми')
    except ValueError:
        print('Ошибка: введите натуральное числовое значение')


# Вывод графика функции q1
indent = 10
dist = graph_width // serifs_count
value = (q1_max - q1_min) / (serifs_count - 1)

## Вывод значений засечек
y = q1_min
print(' ' * indent, end='')
for i in range(serifs_count):
    print(f'{y:^7.2g}', end=' ' * (dist - 8))
    if i < serifs_count - 1:
        y += value
print()

## Вывод оси Y
print(' ' * indent, '|--', sep='', end='')
line_segment = '-' * (dist - 6) + '----|--'
for i in range(serifs_count - 2):
    print(line_segment, end='')
print('-' * (dist - 6) + '----|', sep='')

## Отрисовка графика
r = r_0
for i in range(int(steps) + 1):
    q1 = r ** 3 - 5.57 * r ** 2 - 193 * r - 633.1
    k = round((q1 - q1_min) / (q1_max - q1_min) * (graph_width - dist + 2))

    if r == q1_max_r:
        if q1_max_r % 4 == 0:
            k -= 1
        if q1_max_r == 5:
            k += 2
        if q1_max_r == 6:
            k += 3

    if k > graph_width:
        k = graph_width

    m = round(-q1_min / (q1_max - q1_min) * (graph_width - dist + 2)) if (
                q1_min < 0 and q1_max > 0) else None

    if q1_min < 0 and q1_max > 0:
        if q1 < 0 and not (abs(q1) < 1e-5):
            print(f'{r:^8.1f} |{" " * k}*', end='')
            print(f'{" " * abs(m - k - 1)}|')
        elif abs(q1) <= 1e-5:
            print(f'{r:^8.1f} |{" " * k}|*')
        else:
            print(f'{r:^8.1f} |{" " * m}|', end='')
            print(f'{" " * abs(k - m - 1)}*')
    else:
        print(f'{r:^8.1f} |{" " * k}*')

    r += h


# Вывод результатов
print(f'Сумма положительных значений q2: {q2_positives_sum:7.2f}')
