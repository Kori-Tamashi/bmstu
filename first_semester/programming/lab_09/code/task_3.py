# <имя> <фамилия> <группа>
# Программа изменения матриц

import random
from config import *

# Ввод количества строк матрицы
while True:
    try:
        m = int(input('Введите количество строк в матрицах D и Z: '))
        if m > 0:
            break
        else:
            print(pint_inpt_err_msg)
    except ValueError:
        print(pint_inpt_err_msg)


# Ввод количества столбцов матрицы
while True:
    try:
        n = int(input('Введите количество столбцов в матрицах D и Z: '))
        if n > 0:
            break
        else:
            print(pint_inpt_err_msg)
    except ValueError:
        print(pint_inpt_err_msg)


# Создание матрицы D
is_manual_input = (input('Хотите ли Вы произвести ввод элементов матрицы D вручную? [+/-]: ') == '+')

if is_manual_input:
    print('Выбран ручной ввод элементов матрицы D')
else:
    print('Выбрано автоматическое создание матрицы D')

D = [[0 for _ in range(n)] for _ in range(m)]

if is_manual_input:
    for i in range(m):
        for j in range(n):
            while True:
                try:
                    D[i][j] = int(input(f'Введите D[{i}][{j}] элемент матрицы: '))
                    break
                except ValueError:
                    print(int_inpt_err_msg)
else:
    D = [[random.randint(min_element, max_element) for _ in range(n)] for _ in range(m)]


# Создание матрицы Z
is_manual_input = (input('Хотите ли Вы произвести ввод элементов матрицы Z вручную? [+/-]: ') == '+')

if is_manual_input:
    print('Выбран ручной ввод элементов матрицы Z')
else:
    print('Выбрано автоматическое создание матрицы Z')

Z = [[0 for _ in range(n)] for _ in range(m)]

if is_manual_input:
    for i in range(m):
        for j in range(n):
            while True:
                try:
                    Z[i][j] = int(input(f'Введите Z[{i}][{j}] элемент матрицы: '))
                    break
                except ValueError:
                    print(int_inpt_err_msg)
else:
    Z = [[random.randint(min_element, max_element) for _ in range(n)] for _ in range(m)]


# Вывод созданных матриц D и Z
print('\nСозданная матрица D:')
for row in D:
    print(' '.join(f'{num:>4}' for num in row))

print('\nСозданная матрица Z:')
for row in Z:
    print(' '.join(f'{num:>4}' for num in row))


# Создание массива sum_Z
sum_Z = [sum(row) for row in Z]


# Создание массива G
G = [sum(element > sum_Z[i] for element in D[i]) for i in range(m)]


# Поиск максимального элемента массива G
max_G = max(G) if G else 0


# Создание матрицы D_new
D_new = [row.copy() for row in D]

for i in range(m):
    for j in range(n):
        D_new[i][j] *= max_G


# Вывод результатов
print('\nМассив G:', G)
print('\nМаксимальный элемент массива G:', max_G)
print('\nИзмененная матрица D:')
for row in D_new:
    print(' '.join(f'{num:>4}' for num in row))








