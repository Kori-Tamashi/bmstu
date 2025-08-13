# <имя> <фамилия> <группа>
# Программа создания массива строк и поиска не содержащей цифр строки наибольшей длины

import random
import string
from config import *

# Ввод количества элементов массива
while True:
    try:
        n = int(input('Введите количество строк в исходном массиве: '))
        if n > 0:
            break
        else:
            print(pint_inpt_err_msg)
    except ValueError:
        print(pint_inpt_err_msg)


# Создание массива
is_manual_input = (input('Хотите ли вы произвести ввод строк массива вручную? [+/-]: ') == '+')

if is_manual_input:
    print("Выбран ручной ввод строк массива")
else:
    print("Выбрано автоматическое создание массива")

arr = [''] * n
if is_manual_input:
    for i in range(n):
        arr[i] = str(input(f"Введите {i}-ю строку массива: "))
else:
    characters = string.ascii_letters + string.digits
    for i in range(n):
        length = random.randint(min_length, max_length)
        arr[i] = ''.join(random.choice(characters) for _ in range(length))


# Вывод созданного массива
print(f"Созданный массив: {arr}")

# Поиск строки наибольшей длины, не содержащей цифр
max_str = ''
for string in arr:
    if not any(char.isdigit() for char in string) and len(string) > len(max_str):
        max_str = string


# Вывод результата
if max_str != '':
    print(f'Не содержащая цифр строка наибольшей длины: {max_str}')
else:
    print('Невозможно получить результат: все строки массива содержат цифры')
