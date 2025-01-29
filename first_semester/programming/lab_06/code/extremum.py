# <имя> <фамилия> <группа>
# Программа создания массива и поиска в нём K-го экстремума

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


# Поиск экстремумов
extremums = [arr[i] for i in range(1, n - 1) if (arr[i] > arr[i - 1] and arr[i] > arr[i + 1]) or
                                                (arr[i] < arr[i - 1] and arr[i] < arr[i + 1])]
e_count = len(extremums)


# Проверка наличия экстремумов
if e_count == 0:
    print('Невозможно получить результат: в массиве отсутствуют экстремумы (последовательность монотонна)')
    exit(0)


# Ввод номера экстремума
while True:
    try:
        k = int(input(f'Введите номер экстремума (от 1 до {e_count}): '))
        if 1 <= k <= e_count:
            break
        else:
            print(pint_inpt_err_msg + f' (от 1 до {e_count})')
    except ValueError:
        print(pint_inpt_err_msg)


# Вывод экстремума
print(f'Значение {k}-го экстремума равно {extremums[k - 1]}')
