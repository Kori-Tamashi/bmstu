# <имя> <фамилия> <группа>
# Программа поиска максимальных элементов в строках

import random
from config import *

# Ввод размеров матрицы D
while True:
    try:
        rows = int(input('Введите количество строк матрицы D: '))
        if rows <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)

while True:
    try:
        cols = int(input('Введите количество столбцов матрицы D: '))
        if cols <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Создание матрицы D
d_manual = (input('Хотите ли вы ввести элементы матрицы D вручную? [+/-]: ') == '+')

if d_manual:
    print('Выбран ручной ввод матрицы D')
else:
    print('Выбрано автоматическое создание матрицы D')

D = [[0] * cols for _ in range(rows)]

if d_manual:
    for i in range(rows):
        for j in range(cols):
            while True:
                try:
                    D[i][j] = float(input(f'Введите элемент D[{i}][{j}]: '))
                    break
                except ValueError:
                    print(float_inpt_err_msg)
else:
    D = [[random.randint(min_element, max_element) for _ in range(cols)] for _ in range(rows)]


# Вывод созданной матрицы D
print('\nСозданная матрица D:')
for row in D:
    print(' '.join(f'{num:>4}' for num in row))


# Создание массива I
while True:
    try:
        i_count = int(input('\nВведите количество элементов в массиве I: '))
        if i_count <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)

i_manual = (input('Хотите ли вы ввести элементы массива I вручную? [+/-]: ') == '+')

if i_manual:
    print('Выбран ручной ввод массива I')
else:
    print('Выбрано автоматическое создание массива I')

I = [0] * i_count

if i_manual:
    print('Вводите индексы строк (начиная с 0):')
    for idx in range(i_count):
        while True:
            try:
                I[idx] = int(input(f'Элемент I[{idx}]: '))
                if not (0 <= I[idx] < rows):
                    print(f'Ошибка: индекс должен быть в диапазоне [0; {rows-1}]')
                else:
                    break
            except ValueError:
                print(int_inpt_err_msg)
else:
    I = [random.randint(0, rows-1) for _ in range(i_count)]


# Вывод созданного массива I
print('\nСозданный массив I:', ' '.join(str(i) for i in I))


# Поиск максимумов в указанных строках
R = [0.0] * i_count

for idx in range(i_count):
    row_idx = I[idx]
    R[idx] = max(D[row_idx])


# Расчет среднего арифметического
avg_R = sum(R) / len(R) if R else 0


# Вывод результатов
print('\nМассив R:', ' '.join(f'{val:.2f}' for val in R))
print(f'Среднее арифметическое: {avg_R:.4f}')
