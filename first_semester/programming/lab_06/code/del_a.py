# <имя> <фамилия> <группа>
# Программа создания массива и удаления элемента из него по индексу с помощью средств Python

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


# Ввод индекса для удаления элемента
while True:
    try:
        index = int(input(f'Введите индекс удаляемого элемента (от 0 до {n - 1}): '))
        if 0 <= index <= n - 1:
            break
        else:
            print(uint_inpt_err_msg + f' (от 0 до {n - 1})')
    except ValueError:
        print(uint_inpt_err_msg)


# Обработка массива
del arr[index]


# Вывод обработанного массива
print(f'Обработанный массив: {arr}')
