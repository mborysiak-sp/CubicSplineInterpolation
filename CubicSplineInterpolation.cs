using System;
using System.Collections.Generic;
using System.Text;

namespace CublicSplineInterpolation
{
	class CubicSplineInterpolation
	{
		private List<Point> Points { get; set; }
		private Matrix Vector { get; set; }
		private Matrix MVector { get; set; }
		private Matrix Matrix { get; set; }

		public CubicSplineInterpolation(List<Point> points)
		{
			Points = points;
			Matrix = new Matrix(points.Count);
			Vector = new Matrix(points.Count, 1);

			Fill();
		}

		private double D(int i)
		{
			return 6 / (H(i) + H(i + 1)) *
				((Points[i + 1].Y - Points[i].Y) / H(i + 1) -
				(((Points[i].Y - Points[i - 1].Y)) / H(i)));
		}

		private double L(int i)
		{
			return H(i + 1) / (H(i) + H(i + 1));
		}

		private double M(int i)
		{
			return H(i) / (H(i) + H(i + 1));
		}

		private double H(int i)
		{
			return Points[i].X - Points[i - 1].X;
		}

		private void Fill()
		{
			Matrix.Fields[0, 0] = 2;

			for (int i = 1; i < Matrix.Rows - 1; i++)
			{
				Vector.Fields[i, 1] = D(i);
				Matrix.Fields[i, i - 1] = M(i);
				Matrix.Fields[i, i] = 2;
				Matrix.Fields[i, i + 1] = L(i);
			}

			Matrix.Fields[Matrix.Rows - 1, Matrix.Rows - 1] = 2;
		}

		public void SetMFromGaussElimination()
		{
			MVector = new GaussElimination().EliminateWithPartialPivoting(Matrix, Vector);
		}
	}
}
