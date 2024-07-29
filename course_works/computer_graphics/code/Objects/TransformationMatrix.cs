﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class TransformationMatrix : Matrix
    {
        public TransformationMatrix()
        {
            rows = 4;
            columns = 4;

            _matrix = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    _matrix[i, j] = 0;
            }
        }

        public TransformationMatrix(float[,] matrix)
        {
            if (matrix.GetLength(0) != 4 || matrix.GetLength(1) != 4)
                throw new ArgumentException();

            rows = 4;
            columns = 4;
            _matrix = matrix;
        }

        public static TransformationMatrix operator *(TransformationMatrix a, TransformationMatrix b)
        {
            TransformationMatrix result = new TransformationMatrix();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            return result;
        }
    }
}