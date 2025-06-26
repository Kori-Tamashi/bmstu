# <Имя> <Фамилия> <Группа>
# Программа вывода среза трехмерного массива

import random
from config import *

# Ввод размеров трехмерного массива
while True:
    try:
        x_size = int(input('Введите размер X трёхмерного массива: '))
        if x_size <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)

while True:
    try:
        y_size = int(input('Введите размер Y трёхмерного массива: '))
        if y_size <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)

while True:
    try:
        z_size = int(input('Введите размер Z трёхмерного массива: '))
        if z_size <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Создание трехмерного массива
is_manual = (input('Хотите ли вы ввести элементы массива вручную? [+/-]: ') == '+')

if is_manual:
    print('Выбран ручной ввод трёхмерного массива')
else:
    print('Выбрано автоматическое создание трёхмерного массива')

arr_3d = [[[0 for _ in range(z_size)] for _ in range(y_size)] for _ in range(x_size)]

if is_manual:
    for x in range(x_size):
        for y in range(y_size):
            for z in range(z_size):
                while True:
                    try:
                        arr_3d[x][y][z] = int(input(f'Введите элемент [{x}][{y}][{z}]: '))
                        break
                    except ValueError:
                        print(int_inpt_err_msg)
else:
    for x in range(x_size):
        for y in range(y_size):
            for z in range(z_size):
                arr_3d[x][y][z] = random.randint(min_element, max_element)


# Ввод номера среза
while True:
    try:
        slice_index = int(input('\nВведите номер среза по второму индексу (от 1 до {}): '.format(y_size)))
        if 1 <= slice_index <= y_size:
            break
        else:
            print(f'Ошибка: номер среза должен быть от 1 до {y_size}')
    except ValueError:
        print(int_inpt_err_msg)


# Получение среза
slice_matrix = [[0] * z_size for _ in range(x_size)]
for x in range(x_size):
    for z in range(z_size):
        slice_matrix[x][z] = arr_3d[x][slice_index-1][z]


# Вывод среза
print(f'\nСрез по второму индексу (Y={slice_index}):')
max_width = max(len(str(min_element)), len(str(max_element))) + 2

for row in slice_matrix:
    print(' '.join(f'{num:>{max_width}}' for num in row))
