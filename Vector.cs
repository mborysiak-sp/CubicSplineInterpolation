using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CublicSplineInterpolation
{
	class Vector
	{
		public double[] Fields { get; set; }

		public int Length { get; set; }

		public Vector(int length)
		{
			Fields = new double[length];
			Length = length;
		}

		public Vector(double[] vector)
		{
			Fields = vector;
			Length = vector.Length;
		}

		public static Vector operator +(Vector a) => a;

		public static Vector operator -(Vector a)
		{
			for (int i = 0; i < a.Length; i++)
			{
				a.Fields[i] = -a.Fields[i];
			}
			return a;
		}

		public static Vector operator +(Vector a, Vector b)
		{
			if (a.Length != b.Length)
			{
				throw new System.ArgumentException("Can't add vectors with different lengths");
			}

			Vector result = new Vector(a.Length);

			for (int i = 0; i < a.Length; i++)
			{
				result.Fields[i] = a.Fields[i] + b.Fields[i];
			}
			return result;
		}

		public static Vector operator -(Vector a, Vector b) => a + (-b);

		public void SwapRows(int a, int b)
		{
			double temp = Fields[a];
			Fields[a] = Fields[b];
			Fields[b] = temp;
		}

		public override string ToString()
		{
			string result = "[";
			foreach (var field in Fields)
			{
				result += field + ", ";
			}
			result += "]\n";

			return result;
		}

		public void Abs()
		{
			for (int i = 0; i < Length; i++)
			{
				Fields[i] = Math.Abs(Fields[i]);
			}
		}

		public static Vector Clone(Vector source)
		{
			if (!typeof(Vector).IsSerializable)
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
				return (Vector)formatter.Deserialize(stream);
			}
		}
	}
}
