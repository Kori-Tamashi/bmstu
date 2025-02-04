# <имя> <фамилия> <группа>
# Программа создания массива и добавления в него соседних удвоенных значений всех четных элементов

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
i = 0
while i < len(arr):
    if arr[i] % 2 == 0:
        arr.insert(i + 1, arr[i] * 2)
        i += 1
    i += 1


# Вывод обработанного массива
print(f"Обработанный массив: {arr}")
