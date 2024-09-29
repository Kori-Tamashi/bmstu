using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Matrix<T> where T : struct, IConvertible, IComparable<T>
    {
        protected int rows;
        protected int columns;
        protected List<List<T>> _matrix;
        
        public int Rows
        {
            get { return rows;}
        }

        public int Columns
        {
            get { return columns; }
        }

        public Matrix() 
        {
            rows = 0;
            columns = 0;
            _matrix = new List<List<T>>();
        }

        public Matrix(int row, int col) 
        {
            rows = row;
            columns = col;

            _matrix = new List<List<T>>(row);
            for (int i = 0; i < row; i++)
            {
                _matrix.Add(new List<T>(col));
                for (int j = 0; j < col; j++)
                    _matrix[i].Add((T)Convert.ChangeType(0, typeof(T)));       
            }
        }


        public Matrix(int row, int col, T fill)
        {
            rows = row;
            columns = col;

            _matrix = new List<List<T>>(row);
            for (int i = 0; i < row; i++)
            {
                _matrix.Add(new List<T>(col));
                for (int j = 0; j < col; j++)
                    _matrix[i].Add((T)Convert.ChangeType(fill, typeof(T)));
            }
        }

        public Matrix(List<List<T>> matrix)
        {
            rows = matrix.Count;
            columns = matrix[0].Count;
            _matrix = matrix;
        }

        public Matrix(Matrix<T> matrix)
        {
            rows = matrix.Rows;
            columns = matrix.Columns;
            _matrix = new List<List<T>>(matrix._matrix);
        }

        public Matrix(Point3D point)
        {
            rows = 1;
            columns = 4;

            _matrix = new List<List<T>>(1);
            _matrix.Add(new List<T>(4));
            for (int i = 0; i < columns; i++)
                _matrix[0].Add((T)Convert.ChangeType(0, typeof(T)));

            _matrix[0][0] = (T)Convert.ChangeType(point.X, typeof(T));
            _matrix[0][1] = (T)Convert.ChangeType(point.Y, typeof(T));
            _matrix[0][2] = (T)Convert.ChangeType(point.Z, typeof(T));  
            _matrix[0][3] = (T)Convert.ChangeType(1, typeof(T));
        }

        public float Determinant()
        {
            if (rows != columns)
            {
                throw new InvalidOperationException("Determinant can only be calculated for square matrices.");
            }

            if (rows == 1)
            {
                return (dynamic)_matrix[0][0];
            }

            if (rows == 2)
            {
                return (dynamic)_matrix[0][0] * _matrix[1][1] - (dynamic)_matrix[0][1] * _matrix[1][0];
            }

            float determinant = 0;
            for (int i = 0; i < rows; i++)
            {
                T sign = (i % 2 == 0) ? (T)Convert.ChangeType(1, typeof(T)) : (T)Convert.ChangeType(-1, typeof(T));

                determinant = determinant + (sign * (dynamic)_matrix[0][i] * Cofactor(0, i));
            }

            return determinant;
        }

        private float Cofactor(int row, int col)
        {
            if (rows == 1)
            {
                return (dynamic)_matrix[0][0];
            }

            Matrix<T> submatrix = SubMatrix(row, col);

            return submatrix.Determinant();
        }

        private Matrix<T> SubMatrix(int row, int col)
        {
            Matrix<T> submatrix = new Matrix<T>(rows - 1, columns - 1);

            int subRow = 0;
            for (int i = 0; i < rows; i++)
            {
                if (i != row)
                {
                    int subCol = 0;
                    for (int j = 0; j < columns; j++)
                    {
                        if (j != col)
                        {
                            submatrix._matrix[subRow][subCol] = _matrix[i][j];
                            subCol++;
                        }
                    }
                    subRow++;
                }
            }

            return submatrix;
        }

        public Matrix<T> Inverse()
        {
            // Проверка, является ли матрица квадратной
            if (rows != columns)
            {
                throw new InvalidOperationException("Матрица должна быть квадратной, чтобы найти её обратную.");
            }

            float det = Determinant();

            if (Determinant() == 0)
            {
                throw new InvalidOperationException("Матрица должны быть не вырожденной, чтобы найти её обратную.");
            }

            // Создание копии
            Matrix<T> matrixCopy = new Matrix<T>(this);


            // Создание единичной матрицы того же размера
            Matrix<T> identity = new Matrix<T>(rows, columns);
            for (int i = 0; i < rows; i++)
            {
                identity[i, i] = (T)Convert.ChangeType(1, typeof(T));
            }

            // Использование метода Гаусса для преобразования матрицы в единичную, 
            // применяя те же операции к единичной матрице, чтобы найти обратную.
            for (int i = 0; i < rows; i++)
            {
                // Нахождение опорного элемента
                T pivot = matrixCopy[i, i];

                // Деление строки на опорный элемент
                for (int j = 0; j < columns; j++)
                {
                    matrixCopy[i, j] = (T)Convert.ChangeType(
                        Convert.ToDouble(matrixCopy[i, j]) / Convert.ToDouble(pivot), 
                        typeof(T)
                    );

                    identity[i, j] = (T)Convert.ChangeType(
                        Convert.ToDouble(identity[i, j]) / Convert.ToDouble(pivot), 
                        typeof(T)
                    );
                }

                // Устранение других элементов в столбце
                for (int k = 0; k < rows; k++)
                {
                    if (k != i)
                    {
                        T factor = matrixCopy[k, i];
                        for (int j = 0; j < columns; j++)
                        {
                            matrixCopy[k, j] = (T)Convert.ChangeType(
                                Convert.ToDouble(matrixCopy[k, j]) - Convert.ToDouble(factor) * Convert.ToDouble(matrixCopy[i, j]), 
                                typeof(T)
                            );
                            
                            identity[k, j] = (T)Convert.ChangeType(
                                Convert.ToDouble(identity[k, j]) - Convert.ToDouble(factor) * Convert.ToDouble(identity[i, j]), 
                                typeof(T)
                            );
                        }
                    }
                }
            }

            return identity;
        }

        public bool IsDegenerate()
        {
            return Math.Abs(Determinant()) < 1e-6;
        }

        public T this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= rows || col < 0 || col >= columns)
                    throw new ArgumentOutOfRangeException();
                return _matrix[row][col];
            }
            set
            {
                if (row < 0 || row >= rows || col < 0 || col >= columns)
                    throw new ArgumentOutOfRangeException();
                _matrix[row][col] = value;
            }
        }

        public static Matrix<T> operator* (Matrix<T> a, Matrix<T> b)
        {
            if (a.columns != b.rows)
            {
                throw new ArgumentException("Матрицы нельзя перемножить, так как количество столбцов первой матрицы не равно количеству строк второй матрицы.");
            }

            Matrix<T> result = new Matrix<T>(a.rows, b.columns);

            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.columns; j++)
                {
                    for (int k = 0; k < a.columns; k++) 
                    {
                        result[i, j] += (dynamic)a[i, k] * (dynamic)b[k, j];
                    }
                }
            }

            return result;
        }

        public static Matrix<T> operator *(float k, Matrix<T> a)
        {
            Matrix<T> result = new Matrix<T>(a);

            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.columns; j++)
                    result[i, j] = (dynamic)a[i, j] * k;
            }

            return result;
        }
    }
}
