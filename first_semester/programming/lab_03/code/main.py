# <имя> <фамилия> <группа>
# Программа вычисления длин сторон треугольника и длины его биссектрисы, проведенной из
# наибольшего угла, проверки является ли данный треугольник прямоугольным, а также
# для определения принадлежности произвольной точки к этому треугольнику

EPS = 1e-8

# Ввод координат точки треугольника A
while True:
    try:
        x_A = float(input('Введите значение координаты X точки треугольника A: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        y_A = float(input('Введите значение координаты Y точки треугольника A: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        z_A = float(input('Введите значение координаты Z точки треугольника A: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

print('\n')


# Ввод координат точки треугольника B
while True:
    try:
        x_B = float(input('Введите значение координаты X точки треугольника B: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        y_B = float(input('Введите значение координаты Y точки треугольника B: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        z_B = float(input('Введите значение координаты Z точки треугольника B: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

print('\n')


# Ввод координат точки треугольника C
while True:
    try:
        x_C = float(input('Введите значение координаты X точки треугольника C: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        y_C = float(input('Введите значение координаты Y точки треугольника C: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        z_C = float(input('Введите значение координаты Z точки треугольника C: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

print('\n')


# Вычисление координат векторов треугольника
vector_AB_x = x_B - x_A
vector_AB_y = y_B - y_A
vector_AB_z = z_B - z_A

vector_AC_x = x_C - x_A
vector_AC_y = y_C - y_A
vector_AC_z = z_C - z_A

vector_BC_x = x_C - x_B
vector_BC_y = y_C - y_B
vector_BC_z = z_C - z_B


# Проверка существования треугольника
determinant = ((vector_AB_x * vector_AC_y - vector_AB_y * vector_AC_x) +
               (vector_AB_y * vector_AC_z - vector_AB_z * vector_AC_y) +
               (vector_AB_z * vector_AC_x - vector_AB_x * vector_AC_z))

if abs(determinant) < EPS:
    print('Ошибка: треугольник не существует (точки лежат на одной прямой)')
    exit(1)


# Вычисление длин сторон треугольника
d_AB = (vector_AB_x ** 2 + vector_AB_y ** 2 + vector_AB_z ** 2) ** 0.5
d_AC = (vector_AC_x ** 2 + vector_AC_y ** 2 + vector_AC_z ** 2) ** 0.5
d_BC = (vector_BC_x ** 2 + vector_BC_y ** 2 + vector_BC_z ** 2) ** 0.5


# Проверка треугольника на статус прямоугольного
is_rectangular = (abs(d_AB ** 2 - (d_AC ** 2 + d_BC ** 2)) < EPS or
                  abs(d_AC ** 2 - (d_AB ** 2 + d_BC ** 2)) < EPS or
                  abs(d_BC ** 2 - (d_AB ** 2 + d_AC ** 2)) < EPS)


# Поиск длины биссектрисы наибольшего угла
max_d = max(d_AB, d_AC, d_BC)
half_perimeter = (d_AB + d_AC + d_BC) / 2

if max_d == d_AB:
    bisector = 2 * (d_BC * d_AC * half_perimeter * (half_perimeter - d_AB)) ** 0.5 / (d_BC + d_AC)
    max_angle = 'С'
elif max_d == d_AC:
    bisector = 2 * (d_AB * d_BC * half_perimeter * (half_perimeter - d_AC)) ** 0.5 / (d_AB + d_BC)
    max_angle = 'B'
elif max_d == d_BC:
    bisector = 2 * (d_AB * d_AC * half_perimeter * (half_perimeter - d_BC)) ** 0.5 / (d_AB + d_AC)
    max_angle = 'A'


# Ввод координат точки K
while True:
    try:
        x_K = float(input('Введите значение координаты X произвольной точки K: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        y_K = float(input('Введите значение координаты Y произвольной точки K: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

while True:
    try:
        z_K = float(input('Введите значение координаты Z произвольной точки K: '))
        break
    except ValueError:
        print('Ошибка: введите вещественное числовое значение')

print('\n')


# Проверка находится ли точка K внутри треугольника
vector_AK_x = x_K - x_A
vector_AK_y = y_K - y_A
vector_AK_z = z_K - z_A

vector_BK_x = x_K - x_B
vector_BK_y = y_K - y_B
vector_BK_z = z_K - z_B

vector_CK_x = x_K - x_C
vector_CK_y = y_K - y_C
vector_CK_z = z_K - z_C

vector_CA_x = -vector_AC_x
vector_CA_y = -vector_AC_y
vector_CA_z = -vector_AC_z

cp_AB_AK = ((vector_AB_y * vector_AK_z - vector_AB_z * vector_AK_y) -
            (vector_AB_x * vector_AK_z - vector_AB_z * vector_AK_x) -
            (vector_AB_x * vector_AK_y - vector_AB_y * vector_AK_x))

cp_BC_BK = ((vector_BC_y * vector_BK_z - vector_BC_z * vector_BK_y) -
            (vector_BC_x * vector_BK_z - vector_BC_z * vector_BK_x) -
            (vector_BC_x * vector_BK_y - vector_BC_y * vector_BK_x))

cp_CA_CK = ((vector_CA_y * vector_CK_z - vector_CA_z * vector_CK_y) -
            (vector_CA_x * vector_CK_z - vector_CA_z * vector_CK_x) -
            (vector_CA_x * vector_CK_y - vector_CA_y * vector_CK_x))

is_inside = ((cp_AB_AK > 0 and cp_BC_BK > 0 and cp_CA_CK > 0) or
             (cp_AB_AK < 0 and cp_BC_BK < 0 and cp_CA_CK < 0))


# Вывод результатов
print(f'Длина стороны AB равна {d_AB:.5g}')
print(f'Длина стороны BC равна {d_BC:.5g}')
print(f'Длина стороны AC равна {d_AC:.5g}')
print(f'Наибольшим углом является угол {max_angle}')
print(f'Длина биссектрисы {max_angle}M равна {bisector:.5g}')

if is_rectangular:
    print('Треугольник ABC является прямоугольным')
else:
    print('Треугольник ABC не является прямоугольным')

if is_inside:
    print('Точка K принадлежит треугольнику ABC')
else:
    print('Точка K не принадлежит треугольнику ABC')
