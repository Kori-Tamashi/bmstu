# <имя> <фамилия> <группа>
# Программа создания массива и поиска в нём самой длинной непрерывной последовательности
# простых чисел

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


# Поиск самой длинной непрерывной последовательности простых чисел
longest_sequence = []
current_sequence = []

for element in arr:
    if element > 1 and all(element % d != 0 for d in range(2, int(element ** 0.5) + 1)):
        current_sequence.append(element)
        if len(current_sequence) > len(longest_sequence):
            longest_sequence = current_sequence.copy()
    else:
        current_sequence = []


# Вывод результата
if longest_sequence:
    print(f'Самой длинной неприрывной последовательностью простых чисел является {longest_sequence}')
else:
    print('Невозможно получить результат: в массиве остутствуют простые числа')
