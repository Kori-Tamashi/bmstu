# <имя> <фамилия> <группа>
# Программа создания матрицы и перестановки местами строк с наибольшим и наименьшим количеством
# отрицательных элементов

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
        n = int(input('Введите количество столбцов в матрице: '))
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


# Перестановка местами строк с наибольшим и наименьшим количеством отрицательных элементов
min_row_index = 0
max_row_index = 0
min_k = float('Inf')
max_k = float('-Inf')

for i in range(m):
    k = sum(1 for j in range(n) if matrix[i][j] < 0)

    if k < min_k:
        min_k = k
        min_row_index = i

    if k > max_k:
        max_k = k
        max_row_index = i

matrix[min_row_index], matrix[max_row_index] = matrix[max_row_index], matrix[min_row_index]
min_row_index, max_row_index = max_row_index, min_row_index


# Вывод результата
print(f'Строка с наименьшим количеством отрицательных элементов: {matrix[min_row_index]}')
print(f'Строка с наибольшим количеством отрицательных элементов: {matrix[max_row_index]}')
print()

print('Обработанная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
