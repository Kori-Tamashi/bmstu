using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Matrix
    {
        protected int rows;
        protected int columns;
        protected float[,] _matrix;

        public Matrix() 
        {
            rows = 0;
            columns = 0;
            _matrix = new float[0, 0];
        }

        public Matrix(int row, int col) 
        {
            rows = row;
            columns = col;

            _matrix = new float[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                    _matrix[i, j] = 0;
            }
        }

        public Matrix(float[,] matrix)
        {
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
            _matrix = matrix;
        }

        public Matrix(Point3D point)
        {
            rows = 1;
            columns = 4;

            _matrix = new float[1, 4];
            _matrix[0, 0] = point.X;
            _matrix[0, 1] = point.Y;
            _matrix[0, 2] = point.Z;
            _matrix[0, 3] = 1;
        }

        public float this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= rows || col < 0 || col >= columns)
                    throw new ArgumentOutOfRangeException();
                return _matrix[row, col];
            }
            set
            {
                if (row < 0 || row >= rows || col < 0 || col >= columns)
                    throw new ArgumentOutOfRangeException();
                _matrix[row, col] = value;
            }
        }

        public static Matrix operator* (Matrix a, Matrix b)
        {
            if (a.columns != b.rows)
            {
                throw new ArgumentException("Матрицы нельзя перемножить, так как количество столбцов первой матрицы не равно количеству строк второй матрицы.");
            }

            Matrix result = new Matrix(a.rows, b.columns);

            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.columns; j++)
                {
                    for (int k = 0; k < a.columns; k++) 
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            return result;
        }


    }
}
