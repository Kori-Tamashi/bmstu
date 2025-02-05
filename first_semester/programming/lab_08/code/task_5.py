# <имя> <фамилия> <группа>
# Программа создания квадратной матрицы и поиска максимального и минимального элементов матрицы
# над и под главной и побочной диагоналями матрицы соответственно

import random
from config import *

# Ввод количества строк/столбцов матрицы
while True:
    try:
        m = int(input('Введите количество строк/стобцов в матрице: '))
        if m > 0:
            break
        else:
            print(pint_inpt_err_msg)
    except ValueError:
        print(pint_inpt_err_msg)


# Создание матрицы
is_manual_input = (input('Хотите ли вы произвести ввод элементов матрицы вручную? [+/-]: ') == '+')

if is_manual_input:
    print('Выбран ручной ввод элементов матрицы')
else:
    print('Выбрано автоматическое создание матрицы')

matrix = [[0 for _ in range(m)] for _ in range(m)]

if is_manual_input:
    for i in range(m):
        for j in range(m):
            while True:
                try:
                    matrix[i][j] = int(input(f'Введите [{i}][{j}] элемент матрицы: '))
                    break
                except ValueError:
                    print(int_inpt_err_msg)
else:
    matrix = [[random.randint(min_element, max_element) for _ in range(m)] for _ in range(m)]


# Вывод созданной матрицы
print('Созданная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
print()


# Поиск максимального и минимального элементов матрицы над и под главной и побочной диагоналями
# матрицы соответственно
max_elem = float('-Inf')
min_elem = float('Inf')

for i in range(m - 1):
    for j in range(i + 1, m):
        if matrix[i][j] > max_elem:
            max_elem = matrix[i][j]

for i in range(1, m):
    for j in range(m - i, m):
        if matrix[i][j] < min_elem:
            min_elem = matrix[i][j]


# Вывод результата
length = max(len(' '.join(f'{num:>4}' for num in matrix[0][1:m])), 57)

print(f'{"Треугольная матрица над главной диагональю":^{length}}'
      f'{separator}'
      f'{"Треугольная матрица под побочной диагональю":^{length}}')

for i in range(m - 1):
    upper_row = ' '.join(f'{num:>4}' for num in matrix[i][i + 1:m])
    lower_row = ' '.join(f'{num:>4}' for num in matrix[i + 1][m - i - 1:m])

    upper_row = upper_row.replace(f'-{max_elem}', f'*')
    upper_row = upper_row.replace(f'{max_elem}', f'{green_start}{max_elem}{green_end}')
    upper_row = upper_row.replace(f'*', f'-{max_elem}')

    lower_row = lower_row.replace(f'-{min_elem}', f'*')
    lower_row = lower_row.replace(f'{min_elem}', f'{green_start}{min_elem}{green_end}')
    lower_row = lower_row.replace(f'*', f'-{min_elem}')

    if green_start not in upper_row:
        print(f'{upper_row:>{length}}', end=separator)
    else:
        print(f'{upper_row:>{length + 9 * upper_row.count(green_start)}}', end=separator)

    if green_start not in lower_row:
        print(f'{lower_row:>{length}}')
    else:
        print(f'{lower_row:>{length + 9 * lower_row.count(green_start)}}')
print()

print(f'Максимальный элемент матрицы над главной диагональю: {max_elem}')
print(f'Минимальный элемент матрицы под побочной диагональю: {min_elem}')
