# <имя> <фамилия> <группа>
# Программа создания массива и смены местами его последнего нулевого и максимального отрицательного
# элементов

import random
from config import *

# Ввод количества элементов массива
while True:
    try:
        n = int(input('Введите количество элементов в исходном массиве: '))
        if n > 0:
            break
        else:
            print(pint_inpt_err_msg)
    except ValueError:
        print(pint_inpt_err_msg)


# Создание массива
is_manual_input = (input('Хотите ли вы произвести ввод элементов массива вручную? [+/-]: ') == '+')

if is_manual_input:
    print("Выбран ручной ввод элементов массива")
else:
    print("Выбрано автоматическое создание массива")

arr = [0] * n
if is_manual_input:
    for i in range(n):
        while True:
            try:
                arr[i] = int(input(f"Введите {i}-й элемент массива: "))
                break
            except ValueError:
                print(int_inpt_err_msg)
else:
    arr = [random.randint(min_element, max_element) for _ in range(n)]


# Вывод созданного массива
print(f"Созданный массив: {arr}")


# Обработка массива
null_index = -1
max_negative_index = -1
max_negative = float('-Inf')

for i in range(n):
    if arr[i] == 0:
        null_index = i
    if max_negative < arr[i] < 0:
        max_negative = arr[i]
        max_negative_index = i

arr[null_index], arr[max_negative_index] = arr[max_negative_index], arr[null_index]


# Вывод результата
if null_index != -1 and max_negative_index != -1:
    print(f'Обработанный массив: {arr}')
elif null_index == -1 and max_negative_index == -1:
    print('Невозможно получить результат: все элементы массива положительные')
elif null_index == -1:
    print('Невозможно получить результат: среди элементов массива нет нулевого')
else:
    print('Невозможно получить результат: среди элементов массива нет отрицательного')
