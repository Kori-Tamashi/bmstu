# <имя> <фамилия> <группа>
# Программа поиска суммы бесконечного ряда с заданной точностью за определенное количество итераций

table_width = 75    # ширина таблицы значений

# Ввод входных значений
while True:
    try:
        x = float(input('Введите значение аргумента X: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        eps = float(input('Введите точность, с которой требуется вычислить сумму: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        n = int(input('Введите количество итераций, за которое требуется вычислить сумму: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        h = int(input('Введите значение шага печати: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')


# Вывод таблицы
full_column_width = table_width - 10
column_width = full_column_width // 3
header_format = f"| {'№ итерации':^{column_width}} | {'t':^{column_width}} | {'s':^{column_width}} |"
separator = '-' * (table_width - (full_column_width % 3))

print(separator)
print(header_format)
print(separator)
print(f"| {1:^{column_width}} | {1:^{column_width}g} | {1:^{column_width}g} |")

s = 1
t_last = 1
counter = h
is_accurate = False
for i in range(n):
    t_current = -t_last * (2 * i - 1) / (2 * i + 2) * x
    s += t_current

    if abs(t_current) < eps:
        is_accurate = True
        break

    if i + 1 == counter:
        print(f"| {(counter + 1):^{column_width}} | {t_current:^{column_width}g} | {s:^{column_width}g} |")
        counter += h

    t_last = t_current

print(separator)


# Вывод результата
if is_accurate:
    print(f"Сумма бесконечного ряда равна {s:.5g}, вычислена за {i + 1} итераций")
else:
    print('За введенное количество итераций не удалось вычислить сумму с заданной точностью')