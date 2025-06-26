# <имя> <фамилия> <группа>
# Программа вычисления интеграла методом правых прямоугольников и трапеций

import math
from config import *

# Функции для интегрирования
def f(x):
    """Подынтегральная функция: x^2 * sin(x)"""
    return x**2 * math.sin(x)

def F(x):
    """Первообразная: (2 - x^2)*cos(x) + 2*x*sin(x)"""
    return (2 - x**2)*math.cos(x) + 2*x*math.sin(x)

# Метод правых прямоугольников
def right_rectangles(a, b, n):
    h = (b - a) / n
    integral = 0.0
    for i in range(1, n+1):
        x = a + i * h
        integral += f(x)
    return integral * h

# Метод трапеций
def trapezoidal(a, b, n):
    h = (b - a) / n
    integral = (f(a) + f(b)) / 2
    for i in range(1, n):
        x = a + i * h
        integral += f(x)
    return integral * h

# Ввод параметров с проверкой
def input_float(prompt):
    while True:
        try:
            value = float(input(prompt))
            return value
        except ValueError:
            print(float_inpt_err_msg)

# Вычисление погрешностей
def calc_errors(approx, exact):
    abs_error = abs(approx - exact)
    rel_error = abs_error / abs(exact) * 100 if abs(exact) > 1e-15 else float('inf')
    return abs_error, rel_error

def input_int(prompt):
    while True:
        try:
            value = int(input(prompt))
            if value <= 0:
                print(pint_inpt_err_msg)
            else:
                return value
        except ValueError:
            print(pint_inpt_err_msg)


print("\n" + "=" * 50)
print("Вычисление интеграла методами:")
print("1. Правых прямоугольников")
print("2. Трапеций")
print("=" * 50)


# Ввод данных
a = input_float("Введите начало отрезка интегрирования: ")
b = input_float("Введите конец отрезка интегрирования: ")
if a >= b:
    print("Ошибка: начало отрезка должно быть меньше конца!")
    exit()

N1 = input_int("Введите первое количество участков разбиения (N1): ")
N2 = input_int("Введите второе количество участков разбиения (N2): ")
epsilon = input_float("Введите точность вычисления (ε): ")
if epsilon <= 0:
    print("Ошибка: точность должна быть положительной!")
    exit()


# Точное значение интеграла
exact_value = F(b) - F(a)


# Вычисление интегралов
rr_n1 = right_rectangles(a, b, N1)
rr_n2 = right_rectangles(a, b, N2)
tr_n1 = trapezoidal(a, b, N1)
tr_n2 = trapezoidal(a, b, N2)


# Вывод таблицы результатов
print("\nРезультаты вычислений:")
print(f"{'Метод/Разбиение':<25} | {f'N1 = {N1}':>15} | {f'N2 = {N2}':>15}")
print("-" * 60)
print(f"{'Правых прямоугольников':<25} | {rr_n1:15.7g} | {rr_n2:15.7g}")
print(f"{'Трапеций':<25} | {tr_n1:15.7g} | {tr_n2:15.7g}")
print(f"{'Точное значение':<25} | {exact_value:15.7g} | {exact_value:15.7g}")


# Вычисление погрешностей
errors_rr_n1 = calc_errors(rr_n1, exact_value)
errors_rr_n2 = calc_errors(rr_n2, exact_value)
errors_tr_n1 = calc_errors(tr_n1, exact_value)
errors_tr_n2 = calc_errors(tr_n2, exact_value)


# Вывод таблицы погрешностей
print("\nАбсолютные и относительные погрешности:")
print(f"{'Метод/Разбиение':<25} | {'Абс. погр.':>15} | {'Отн. погр. (%):':>15}")
print("-"*60)
print(f"{'Прав.прям. (N1)':<25} | {errors_rr_n1[0]:15.7g} | {errors_rr_n1[1]:15.7g}")
print(f"{'Прав.прям. (N2)':<25} | {errors_rr_n2[0]:15.7g} | {errors_rr_n2[1]:15.7g}")
print(f"{'Трапеций (N1)':<25} | {errors_tr_n1[0]:15.7g} | {errors_tr_n1[1]:15.7g}")
print(f"{'Трапеций (N2)':<25} | {errors_tr_n2[0]:15.7g} | {errors_tr_n2[1]:15.7g}")


# Определение наиболее точного метода
min_abs_error = min(errors_rr_n1[0], errors_rr_n2[0], errors_tr_n1[0], errors_tr_n2[0])
if min_abs_error in (errors_rr_n1[0], errors_rr_n2[0]):
    precise_method = "Правых прямоугольников"
    other_method = "Трапеций"
    other_func = trapezoidal
else:
    precise_method = "Трапеций"
    other_method = "Правых прямоугольников"
    other_func = right_rectangles

print(f"\nНаиболее точный метод: {precise_method}")


# Итерационное уточнение для другого метода
print(f"\nИтерационное уточнение для метода {other_method} с ε={epsilon}")
n = min(N1, N2)
prev_value = other_func(a, b, n)
n *= 2
curr_value = other_func(a, b, n)
iter_count = 1

print(f"{'Итерация':>8} | {'Кол-во разбиений':>16} | {'Значение интеграла':>20} | {'Разница':>15}")
print("-" * 70)

while abs(curr_value - prev_value) >= epsilon:
    print(f"{iter_count:8d} | {n:16d} | {curr_value:20.7g} | {abs(curr_value - prev_value):15.7g}")
    prev_value = curr_value
    n *= 2
    curr_value = other_func(a, b, n)
    iter_count += 1


# Вывод результатов
print(f"{iter_count:8d} | {n:16d} | {curr_value:20.7g} | {abs(curr_value - prev_value):15.7g}")
print("-" * 70)
print(f"Достигнута требуемая точность ε = {epsilon} при {n} разбиениях")
print(f"Итоговое значение интеграла: {curr_value:.7g}")

abs_error_final = abs(curr_value - exact_value)
print(f"Абсолютная погрешность: {abs_error_final:.7g}")

if abs(exact_value) > 1e-15:
    rel_error_final = abs_error_final / abs(exact_value) * 100
    print(f"Относительная погрешность: {rel_error_final:.7g}%")
else:
    print("Относительная погрешность: 0%")
