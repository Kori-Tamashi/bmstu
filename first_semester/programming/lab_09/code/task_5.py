# <имя> <фамилия> <группа>
# Программа замены гласных букв на точки

import random
from config import *

# Ввод количества строк матрицы
while True:
    try:
        rows = int(input('Введите количество строк матрицы: '))
        if rows <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Ввод количества столбцов матрицы
while True:
    try:
        cols = int(input('Введите количество столбцов матрицы: '))
        if cols <= 0:
            print(pint_inpt_err_msg)
        else:
            break
    except ValueError:
        print(pint_inpt_err_msg)


# Создание матрицы
is_manual = (input('Хотите ли вы ввести элементы матрицы вручную? [+/-]: ') == '+')

if is_manual:
    print('Выбран ручной ввод матрицы')
else:
    print('Выбрано автоматическое создание матрицы')

matrix = [[' ' for _ in range(cols)] for _ in range(rows)]

if is_manual:
    for i in range(rows):
        for j in range(cols):
            while True:
                char = input(f'Введите символ для позиции [{i}][{j}]: ')
                if len(char) != 1:
                    print("Ошибка: введите ровно один символ")
                else:
                    matrix[i][j] = char
                    break
else:
    for i in range(rows):
        for j in range(cols):
            matrix[i][j] = random.choice(chars)


# Вывод исходной матрицы
print('\nИсходная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))


# Создание преобразованной матрицы
transformed_matrix = [row.copy() for row in matrix]

for i in range(rows):
    for j in range(cols):
        if transformed_matrix[i][j] in vowels:
            transformed_matrix[i][j] = '.'


# Вывод преобразованной матрицы
print('\nПреобразованная матрица:')
for row in transformed_matrix:
    print(' '.join(f'{num:>4}' for num in row))
