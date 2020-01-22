using System;
using System.Collections.Generic;

namespace CublicSplineInterpolation
{
	class Program
	{
		static List<Point> samplePoints = new List<Point>
			{
			new Point(1, 2),
			new Point(2, 3),
			new Point(3, 5),
			new Point(4, 1),
			new Point(5, 6),
			new Point(7, 4),
			new Point(8, 9)
			};

		static void Main(string[] args)
		{
			CubicSplineInterpolation cubicSplineInterpolation = new CubicSplineInterpolation(samplePoints);
			cubicSplineInterpolation.SetMVectorFromGaussSeidel();
			cubicSplineInterpolation.Print(100);
		}
	}
}
