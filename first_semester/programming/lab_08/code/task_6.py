# <имя> <фамилия> <группа>
# Программа создания квадратной матрицы и её транспонирования

import random
from config import *

# Ввод количества строк/столбцов матрицы
while True:
    try:
        m = int(input('Введите количество строк/столбцов в матрице: '))
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


# Обработка матрицы
for i in range(m):
    for j in range(i + 1, m):
        matrix[j][i], matrix[i][j] = matrix[i][j], matrix[j][i]


# Вывод результата
print('Обработанная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
