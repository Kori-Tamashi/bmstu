using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class UnitMatrix : Matrix<float>
    {
        public UnitMatrix()
        {
            rows = 0;
            columns = 0;
            _matrix = new List<List<float>>();
        }

        public UnitMatrix(int row)
        {
            rows = row;
            columns = row;

            _matrix = new List<List<float>>(row);
            for (int i = 0; i < row; i++)
            {
                _matrix.Add(new List<float>(row));
                for (int j = 0; j < row; j++)
                {
                    if (i == j)
                        _matrix[i].Add(1);
                    else
                        _matrix[i].Add(0);
                }
            }
        }
    }
}
