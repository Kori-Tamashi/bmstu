# <имя> <фамилия> <группа>
# Программа поворота квадратной матрицы

import random
from config import *

# Ввод количества строк/столбцов матрицы
while True:
    try:
        m = int(input('Введите количество строк/столбцов в матрице: '))
        break
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


# Поворот матрицы на 90 градусов по часовой стрелки
for i in range(m // 2):
    for j in range(i, m - i - 1):
        temp = matrix[i][j]
        matrix[i][j] = matrix[m - 1 - j][i]
        matrix[m - 1 - j][i] = matrix[m - 1 - i][m - 1 - j]
        matrix[m - 1 - i][m - 1 - j] = matrix[j][m - 1 - i]
        matrix[j][m - 1 - i] = temp


# Вывод промежуточной матрицы
print('Промежуточная матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
print()


# Поворот матрицы на 90 градусов против часовой стрелки
for i in range(m // 2):
    for j in range(i, m - i - 1):
        temp = matrix[i][j]
        matrix[i][j] = matrix[j][m - 1 - i]
        matrix[j][m - 1 - i] = matrix[m - 1 - i][m - 1 - j]
        matrix[m - 1 - i][m - 1 - j] = matrix[m - 1 - j][i]
        matrix[m - 1 - j][i] = temp


# Вывод итоговой матрицы
print('Итоговая матрица:')
for row in matrix:
    print(' '.join(f'{num:>4}' for num in row))
print()
