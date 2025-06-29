# <имя> <фамилия> <группа>
# Программа исследования метода Шелла

import random
import time
from config import *

# Реализация сортировки Шелла с подсчетом перестановок
def shell_sort(arr):
    n = len(arr)
    comparisons = 0
    swaps = 0
    gap = n // 2

    while gap > 0:
        for i in range(gap, n):
            temp = arr[i]
            j = i
            comparisons += 1

            while j >= gap and arr[j - gap] > temp:
                comparisons += 1
                arr[j] = arr[j - gap]
                swaps += 1
                j -= gap

            arr[j] = temp
            if j != i:
                swaps += 1

        gap //= 2

    return comparisons, swaps


# Генерация различных типов массивов
def generate_array(size, array_type):
    if array_type == "random":
        return [random.randint(min_element, max_element) for _ in range(size)]
    elif array_type == "sorted":
        return sorted(generate_array(size, "random"))
    elif array_type == "reversed":
        return sorted(generate_array(size, "random"), reverse=True)
    return []


# Ввод массива с клавиатуры
def input_array():
    arr = []
    print("\nВведите целочисленные элементы массива, завершите ввод пустой строкой:")
    while True:
        try:
            value = input(f"Элемент {len(arr)}: ")
            if value == "":
                if len(arr) == 0:
                    print("Ошибка: массив не может быть пустым!")
                    continue
                break
            arr.append(int(value))
        except ValueError:
            print(int_inpt_err_msg)
    return arr


# Вывод шапки исследования
print("\n" + "=" * 70)
print("Исследование метода сортировки Шелла".center(70))
print("=" * 70)


# Часть 1: Демонстрация работы алгоритма
print("\n[Часть 1: Демонстрация работы алгоритма]")
arr = input_array()

print("\nИсходный массив:")
print(arr)


# Создаем копию для сортировки
arr_sorted = arr.copy()
comparisons, swaps = shell_sort(arr_sorted)

print("\nОтсортированный массив:")
print(arr_sorted)
print(f"\nКоличество сравнений: {comparisons}")
print(f"Количество перестановок: {swaps}")


# Часть 2: Исследование производительности
print("\n" + "=" * 70)
print("[Часть 2: Исследование производительности]".center(70))
print("=" * 70)


# Ввод размеров массивов
sizes = []
for i in range(3):
    while True:
        try:
            size = int(input(f"Введите размер {i + 1}-го массива (N{i + 1}): "))
            if size <= 0:
                print(pint_inpt_err_msg)
            else:
                sizes.append(size)
                break
        except ValueError:
            print(pint_inpt_err_msg)


# Типы массивов для исследования
array_types = ["sorted", "random", "reversed"]
type_names = {
    "sorted": "Упорядоченный список",
    "random": "Случайный список",
    "reversed": "Упорядоченный в обратном порядке"
}


# Создаем таблицу результатов
results = {size: {atype: {"time": 0, "swaps": 0} for atype in array_types} for size in sizes}


# Проводим измерения
for size in sizes:
    for atype in array_types:
        arr = generate_array(size, atype)

        # Создаем копию для сортировки
        arr_copy = arr.copy()

        # Замер времени
        start_time = time.perf_counter()
        _, swaps = shell_sort(arr_copy)
        end_time = time.perf_counter()

        # Сохраняем результаты
        results[size][atype]["time"] = end_time - start_time
        results[size][atype]["swaps"] = swaps


# Вывод таблицы результатов
print("\n" + "=" * 142)
print("Результаты исследования работы".center(142))
print("=" * 142)

# Ширины колонок
type_width = 36
time_width = 14
swap_width = 14
block_width = time_width + swap_width + 3

# Заголовки столбцов
header = f"| {'Тип массива':<{type_width}} |"
subheader = f"| {'':<{type_width}} |"

for size in sizes:
    header += f" {f'N = {size}':^{block_width}} |"
    subheader += f" {'Время':^{time_width}} | {'Перестановки':^{swap_width}} |"

print(header)
print(subheader)
print("-" * 142)

# Заполнение таблицы
for atype in array_types:
    row = f"| {type_names[atype]:<{type_width}} |"
    for size in sizes:
        time_val = results[size][atype]["time"]
        swaps_val = results[size][atype]["swaps"]
        row += f" {time_val:>{time_width}.6f} | {swaps_val:>{swap_width}} |"
    print(row)

print("=" * 142)
