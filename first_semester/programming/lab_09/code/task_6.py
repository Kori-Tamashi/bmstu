# <имя> <фамилия> <группа>
# Программа построчного перемножения матриц

import random
from config import *

# Ввод размеров матриц
while True:
    try:
        rows = int(input('Введите количество строк матриц A и B: '))
        if rows <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)

while True:
    try:
        cols = int(input('Введите количество столбцов матриц A и B: '))
        if cols <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Создание матрицы A
a_manual = (input('Хотите ли вы ввести элементы матрицы A вручную? [+/-]: ') == '+')

if a_manual:
    print('Выбран ручной ввод матрицы A')
else:
    print('Выбрано автоматическое создание матрицы A')

A = [[0] * cols for _ in range(rows)]

if a_manual:
    for i in range(rows):
        for j in range(cols):
            while True:
                try:
                    A[i][j] = float(input(f'Введите элемент A[{i}][{j}]: '))
                    break
                except ValueError:
                    print(float_inpt_err_msg)
else:
    A = [[random.randint(min_element, max_element) for _ in range(cols)] for _ in range(rows)]


# Создание матрицы B
b_manual = (input('Хотите ли вы ввести элементы матрицы B вручную? [+/-]: ') == '+')

if b_manual:
    print('Выбран ручной ввод матрицы B')
else:
    print('Выбрано автоматическое создание матрицы B')

B = [[0] * cols for _ in range(rows)]

if b_manual:
    for i in range(rows):
        for j in range(cols):
            while True:
                try:
                    B[i][j] = float(input(f'Введите элемент B[{i}][{j}]: '))
                    break
                except ValueError:
                    print(float_inpt_err_msg)
else:
    B = [[random.randint(min_element, max_element) for _ in range(cols)] for _ in range(rows)]


# Вывод созданных матриц
print('\nСозданная матрица A:')
for row in A:
    print(' '.join(f'{num:>8.2f}' for num in row))

print('\nСозданная матрица B:')
for row in B:
    print(' '.join(f'{num:>8.2f}' for num in row))


# Создание матрицы C построчным перемножением
C = [[A[i][j] * B[i][j] for j in range(cols)] for i in range(rows)]


# Создание массива V (суммы по столбцам)
V = [0.0] * cols
for j in range(cols):
    for i in range(rows):
        V[j] += C[i][j]


# Вывод результатов
print('\nМатрица C (результат построчного перемножения):')
for row in C:
    print(' '.join(f'{num:>8.2f}' for num in row))

print('\nМассив V (суммы по столбцам матрицы C):')
print(' '.join(f'{num:>8.2f}' for num in V))