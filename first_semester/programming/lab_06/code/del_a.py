# <имя> <фамилия> <группа>
# Программа создания списка и добавления в него элемента по индексу

import random

min_element = -100      # минимальное значение элементов массива при его автоматическом заполнении
max_element = 100       # максимальное значение элементов массива при его автоматическом заполнении
int_err_msg = "Ошибка: введите целое числовое значение"
uint_err_msg = "Ошибка: введите натуральное числовое значение"
pint_err_msg = "Ошибка: введите неотрицальное целое числовое значение"

# Ввод количества элементов массива
while True:
    try:
        n = int(input('Введите количество элементов в исходном массиве: '))
        if n > 0:
            break
        else:
            print(uint_err_msg)
    except ValueError:
        print(uint_err_msg)


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
                print(int_err_msg)
else:
    for i in range(n):
        arr[i] = random.randint(min_element, max_element)


# Вывод созданного массива
print(f"Созданный массив: {arr}")


# Ввод элемента для добавления
while True:
    try:
        m = int(input('Введите элемент для добавления: '))
        break
    except ValueError:
        print(int_err_msg)


# Ввод индекса для добавления элемента
while True:
    try:
        index = int(input('Введите индекс добавляемого элемента: '))
        if index >= 0:
            break
        else:
            print(pint_err_msg)
    except ValueError:
        print(pint_err_msg)


# Обработка массива
arr.insert(index, m)


# Вывод обработанного массива
print(f"Обработанный массив: {arr}")
