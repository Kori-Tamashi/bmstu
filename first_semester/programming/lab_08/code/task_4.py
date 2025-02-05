# <имя> <фамилия> <группа>
# Программа создания матрицы и перестановки местами столбцов с максимальной и минимальной
# суммой элементов

import random
from config import *

# Ввод количества строк матрицы
while True:
    try:
        m = int(input('Введите количество строк в матрице: '))
        if m > 0:
            break
        else:
            print(pint_inpt_err_msg)
    except ValueError:
        print(pint_inpt_err_msg)


# Ввод количества столбцов матрицы
while True:
    try:
        n = int(input('Введите количество стобцов в матрице: '))
        if n > 0:
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

matrix = [[0 for _ in range(n)] for _ in range(m)]

if is_manual_input:
    for i in range(m):
        for j in range(n):
            while True:
                try:
                    matrix[i][j] = int(input(f'Введите [{i}][{j}] элемент матрицы: '))
                    break
                except ValueError:
                    print(int_inpt_err_msg)
else:
    matrix = [[random.randint(min_element, max_element) for _ in range(n)] for _ in range(m)]


# Вывод созданной матрицы
print('Созданная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
print()


# Перестановка местами столбцов с максимальной и минимальной суммой элементов
min_column_index = 0
max_column_index = 0
min_sum = float('Inf')
max_sum = float('-Inf')

for j in range(n):
    s = sum(matrix[i][j] for i in range(m))

    if s < min_sum:
        min_sum = s
        min_column_index = j

    if s > max_sum:
        max_sum = s
        max_column_index = j

for i in range(m):
    matrix[i][min_column_index], matrix[i][max_column_index] = (matrix[i][max_column_index],
                                                                matrix[i][min_column_index])

min_column_index, max_column_index = max_column_index, min_column_index


# Вывод результата
print('Столбец с минимальной суммой элементов   Столбец с максимальной суммой элементов')
for i in range(m):
    print(f'{matrix[i][min_column_index]:^38}   {matrix[i][max_column_index]:^38}')
print()

print('Обработанная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
