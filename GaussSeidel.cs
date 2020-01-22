using System;
using System.Collections.Generic;
using System.Text;

namespace CublicSplineInterpolation
{
	class GaussSeidel
	{
		private Matrix Matrix { get; set; }
		private Matrix Vector { get; set; }

		public GaussSeidel(Matrix matrix, Matrix vector)
		{
			Matrix = matrix;
			Vector = vector;
		}

		public Matrix Calculate()
		{
			Matrix last = new Matrix(Vector.Rows, 1);
			Matrix current = new Matrix(Vector.Rows, 1);

			double value;

			do
			{
				for (int i = 0; i < Matrix.Rows; i++)
				{
					double sum = 0;
					sum += Vector.Fields[i, 0];

					for (int j = 0; j < Matrix.Columns; j++)
					{
						if (i != j)
						{
							sum -= Matrix.Fields[i, j] * current.Fields[j, 0];
						}
					}

					current.Fields[i, 0] = sum / Matrix.Fields[i, i];
				}

				value = (current - last).GetNorm();
				last = Matrix.Clone(current);
			}
			while (value > 0.001);

			return current;
		}
	}
}
