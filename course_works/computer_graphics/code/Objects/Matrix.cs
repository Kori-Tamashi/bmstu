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
        }

        public Matrix(float[,] matrix)
        {
            rows = matrix.GetLength(0);
            columns = matrix.GetLength(1);
            _matrix = matrix;
        }

        public Matrix(Point3D point)
        {
            rows = 3;
            columns = 1;

            _matrix = new float[3, 1];
            _matrix[0, 0] = point.X;
            _matrix[1, 1] = point.Y;
            _matrix[2, 2] = point.Z;
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
            Matrix result = new Matrix(a.rows, b.columns);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        result._matrix[i, j] += a._matrix[i, k] * b._matrix[k, j];
                    }
                }
            }

            return result;
        }

        
    }
}
