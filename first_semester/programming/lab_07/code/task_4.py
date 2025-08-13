# <имя> <фамилия> <группа>
# Программа создания массива строк и замены всех заглавных гласных букв в строках на строчные

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


# Обработка массива
for i in range(len(arr)):
    arr[i] = ''.join(char.lower() if char in VOWELS else char for char in arr[i])


# Вывод обработанного массива
print(f"Обработанный массив: {arr}")
