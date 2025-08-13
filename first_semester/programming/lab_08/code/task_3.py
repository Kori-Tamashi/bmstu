# <имя> <фамилия> <группа>
# Программа создания матрицы и поиска столбца с наименьшим количеством отрицательных элементов

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


# Поиск столбца с наименьшим количеством отрицательных элементов
min_column_index = 0
min_k = float('Inf')

for j in range(n):
    k = sum(1 for i in range(m) if matrix[i][j] < 0)
    if k < min_k:
        min_k = k
        min_column_index = j


# Вывод результата
print('Столбец с наименьшим количеством отрицательных элементов:')
for i in range(m):
    print(''.join(f'{matrix[i][min_column_index]:>4}'))
