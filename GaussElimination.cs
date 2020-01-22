using System;

namespace CublicSplineInterpolation
{
	public class GaussElimination
	{ 

		private void ZeroColumn(Matrix matrix, Matrix vector, int i)
		{
			for (int j = i + 1; j < matrix.Rows; j++)
			{
				double m = matrix.Fields[j, i] / matrix.Fields[i, i];

				for (int k = 0; k < matrix.Columns; k++)
					matrix.Fields[j, k] -= (dynamic)matrix.Fields[i, k] * m;
				vector.Fields[j, 0] -= (dynamic)vector.Fields[i, 0] * m;
			}
		}

		public Matrix EliminateWithPartialPivoting(Matrix matrix, Matrix vector)
		{
			var matrixClone = Matrix.Clone(matrix);
			var vectorClone = Matrix.Clone(vector);

			for (int i = 0; i < matrixClone.Rows; i++)
			{
				PartialPivot(matrixClone, vectorClone, i);
				ZeroColumn(matrixClone, vectorClone, i);
			}

			return GetResults(matrixClone, vectorClone);
		}


		private Matrix GetResults(Matrix matrix, Matrix vector)
		{
			var results = new Matrix(matrix.Rows, 1);

			for (int i = matrix.Rows - 1; i >= 0; i--)
			{
				double sum = 0;

				for (int j = i + 1; j < matrix.Rows; j++)
					sum += matrix.Fields[i, j] * results.Fields[j, 0];
				results.Fields[i, 0] = (vector.Fields[i, 0] - sum) / matrix.Fields[i, i];
			}

			return results;
		}


		private void PartialPivot(Matrix matrix, Matrix vector, int p)
		{
			for (int j = p; j < matrix.Rows; j++)
				if (Math.Abs(matrix.Fields[p, p]) < Math.Abs(matrix.Fields[j, p]))
				{
					matrix.SwitchRows(p, j);
					vector.SwitchRows(p, j);
				}
		}
	}
}

