# <имя> <фамилия> <группа>
# Программа формирования матрицы по формуле

import math
import random
from config import *

# Ввод количества элементов в массиве D
while True:
    try:
        d_length = int(input('Введите количество элементов в массиве D: '))
        if d_length <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Создание массива D
D = [0] * d_length

d_is_manual_input = (input('Хотите ли Вы произвести ввод элементов массива D вручную? [+/-]: ') == '+')

if d_is_manual_input:
    print('Выбран ручной ввод элементов массива D')
else:
    print('Выбрано автоматическое создание массива D')

if d_is_manual_input:
    for i in range(d_length):
        try:
            D[i] = float(input(f'Введите {i}-й элемент массива D: '))
        except ValueError:
            print(float_inpt_err_msg)
else:
    D = [random.randint(min_element, max_element) for _ in range(d_length)]

print("Созданный массив: ", D)


# Ввод количества элементов в массиве F
while True:
    try:
        f_length = int(input('Введите количество элементов в массиве F: '))
        if f_length <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Создание массива F
F = [0] * f_length

f_is_manual_input = (input('Хотите ли Вы произвести ввод элементов массива F вручную? [+/-]: ') == '+')

if f_is_manual_input:
    print('Выбран ручной ввод элементов массива F')
else:
    print('Выбрано автоматическое создание массива F')

if f_is_manual_input:
    for i in range(f_length):
        try:
            F[i] = float(input(f'Введите {i}-й элемент массива F: '))
        except ValueError:
            print(float_inpt_err_msg)
else:
    F = [random.randint(min_element, max_element) for _ in range(f_length)]

print("Созданный массив: ", F)


# Создание матрицы А
A = [[math.sin(D[j] + F[k]) for k in range(f_length)] for j in range(d_length)]


# Создание массива AV
AV = [
    sum(positive) / len(positive) if (positive := [el for el in A[j] if el > 0]) else 0
    for j in range(d_length)
]


# Создание массива L
L = [sum(A[j][k] < AV[j] for k in range(f_length)) for j in range(d_length)]


# Вывод данных
col_width = 10
l_width = max(len(str(max(L))), 3)

header = "Матрица А".ljust(f_length * (col_width + 1) - 1)
header += separator + "AV".center(col_width)
header += separator + "L".center(l_width)

print()
print(header)
print("-" * len(header))

for j in range(d_length):
    matrix_row = " ".join([f"{x: {col_width}.5f}" for x in A[j]])
    av_str = f"{AV[j]: {col_width}.5f}"
    l_str = f"{L[j]:^{l_width}d}"
    row_str = matrix_row + separator + av_str + separator + l_str
    print(row_str)

print("-" * len(header))
