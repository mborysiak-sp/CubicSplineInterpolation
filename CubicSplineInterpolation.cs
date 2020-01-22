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

		private double Delta(int i)
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

		private double A(int i)
		{
			return Points[i].Y;
		}

		private double B(int i)
		{
			return (Points[i + 1].Y - Points[i].Y) / H(i + 1) - (2 * GetMVector(i) + GetMVector(i + 1)) / 6 * H(i + 1);
		}

		private double C(int i)
		{
			return GetMVector(i) / 2;
		}

		private double D(int i)
		{
			return (GetMVector(i + 1) - GetMVector(i)) / 6 * H(i + 1);
		}

		private double GetMVector(int i)
		{
			return MVector.Fields[i, 0];
		}

		private double S(double x)
		{
			int i = GetIndexX(x);

			double diff = x - Points[i].X;

			double value = A(i) + B(i) * diff + C(i) * Math.Pow(diff, 2) + D(i) * Math.Pow(diff, 3);

			return value;
		}

		public void Print(int i)
		{
			var start = Points[0].X;
			var stop = Points[Points.Count - 1].X;
			var jump = (stop - start) / i;

			for (double x = start; x < stop; x += jump)
			{
				Console.WriteLine(new Point(x, S(x)));
			}
		}

		private int GetIndexX(double x)
		{
			for (int i = 1; i < Points.Count; i++)
			{
				if (x <= Points[i].X)
				{
					return i - 1;
				}
			}
			throw new Exception("Value out of bonds");
		}

		private void Fill()
		{
			Matrix.Fields[0, 0] = 2;
			for (int i = 1; i < Matrix.Rows - 1; i++)
			{
				Vector.Fields[i, 0] = Delta(i);
				Matrix.Fields[i, i - 1] = M(i);
				Matrix.Fields[i, i] = 2;
				Matrix.Fields[i, i + 1] = L(i);
			}

			Matrix.Fields[Matrix.Rows - 1, Matrix.Rows - 1] = 2;
		}

		public void SetMVectorFromGaussElimination()
		{
			MVector = new GaussElimination().EliminateWithPartialPivoting(Matrix, Vector);
		}

		public void SetMVectorFromGaussSeidel()
		{
			MVector = new GaussSeidel(Matrix, Vector).Calculate();
		}

		public void SetMVectorFromJacobi()
		{
			MVector = new Jacobi(Matrix, Vector).Calculate();
		}
	}
}
