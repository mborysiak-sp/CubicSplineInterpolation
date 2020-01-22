using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CublicSplineInterpolation
{
	[Serializable]
	public class Matrix
	{

		public double[,] Fields { get; set; }
		public int Rows { get; set; }
		public int Columns { get; set; }

		public Matrix(int size)
		{
			Rows = size;
			Columns = size;
			Fields = new double[Rows, Columns];
		}

		public Matrix(int rows, int columns)
		{
			Rows = rows;
			Columns = columns;
			Fields = new double[Rows, Columns];
		}

		public Matrix Multiply(Matrix matrix)
		{

			if (Columns != matrix.Rows)
			{
				throw new System.ArgumentException("Can't multiply matrices with wrong sizes");
			}
			else
			{
				Matrix results = new Matrix(Rows, matrix.Columns);

				for (int i = 0; i < Rows; i++)
				{
					for (int j = 0; j < matrix.Columns; j++)
					{
						double sum = new double();

						for (int k = 0; k < matrix.Rows; k++)
							sum += (dynamic)Fields[i, k] * (dynamic)matrix.Fields[k, j];

						results.Fields[i, j] = sum;
					}
				}
				return results;
			}
		}

		public Matrix ConcatenateWithVector(Matrix vector)
		{
			var result = new Matrix(Rows, Columns + 1);

			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns + 1; j++)
				{
					if (j == Columns)
						result.Fields[i, j] = vector.Fields[i, 0];
					else
						result.Fields[i, j] = Fields[i, j];
				}
			}

			return result;
		}

		public Matrix Difference(Matrix matrix)
		{
			var results = Clone(this);
			for (int i = 0; i < Rows; i++)
				for (int j = 0; j < Columns; j++)
					results.Fields[i, j] = (dynamic)Fields[i, j] - (dynamic)matrix.Fields[i, j];
			return results;
		}

		public void SwitchRows(int row1, int row2)
		{
			for (int i = 0; i < Columns; i++)
			{
				double temp;

				temp = Fields[row1, i];

				Fields[row1, i] = Fields[row2, i];

				Fields[row2, i] = temp;
			}
		}

		public void SwitchColumns(int col1, int col2)
		{
			for (int i = 0; i < Rows; i++)
			{
				double temp;

				temp = Fields[i, col1];

				Fields[i, col1] = Fields[i, col2];

				Fields[i, col2] = temp;
			}
		}

		public override string ToString()
		{
			string result = "";

			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
					result += $"{Fields[i, j]} \t";

				result += "\n";
			}

			return result;
		}

		public static Matrix operator +(Matrix matrix) => matrix;

		public static Matrix operator -(Matrix matrix)
		{
			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					matrix.Fields[i, j] = -matrix.Fields[i, j];
				}
			}
			return matrix;
		}

		public static Matrix operator +(Matrix a, Matrix b)
		{
			if (a.Rows != b.Rows || a.Columns != b.Columns)
			{
				throw new System.ArgumentException("Can't add matrices with different sizes");
			}

			Matrix result = new Matrix(a.Rows);

			for (int i = 0; i < a.Rows; i++)
			{
				for (int j = 0; j < b.Columns; j++)
				{
					result.Fields[i, j] = a.Fields[i, j] + b.Fields[i, j];
				}
			}
			return result;
		}

		public static Matrix operator -(Matrix a, Matrix b) => a + (-b);

		public void Abs()
		{
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					Fields[i,j] = Math.Abs(Fields[i, j]);
				}
			}
		}

		public static Matrix Clone(Matrix source)
		{
			if (!typeof(Matrix).IsSerializable)
			{
				throw new ArgumentException("The type must be serializable.", nameof(source));
			}

			if (ReferenceEquals(source, null))
			{
				return default;
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (Matrix)formatter.Deserialize(stream);
			}
		}
	}
}
