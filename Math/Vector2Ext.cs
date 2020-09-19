using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace zxc {
    public static class Vector2Ext
	{
		/// <summary>
		/// temporary workaround to Vector2.Normalize screwing up the 0,0 vector
		/// </summary>
		/// <param name="vec">Vec.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Normalize(ref Vector2 vec)
        {
			var magnitude = Mathf.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
			if (magnitude > Mathf.Epsilon)
				vec /= magnitude;
			else
				vec.X = vec.Y = 0;
		}


		/// <summary>
		/// temporary workaround to Vector2.Normalize screwing up the 0,0 vector
		/// </summary>
		/// <param name="vec">Vec.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Normalize(Vector2 vec)
		{
			var magnitude = Mathf.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
			if (magnitude > Mathf.Epsilon)
				vec /= magnitude;
			else
				vec.X = vec.Y = 0;

			return vec;
		}


		/// <summary>
		/// rounds the x and y values
		/// </summary>
		/// <param name="vec">Vec.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Round(this Vector2 vec)
		{
			return new Vector2(Mathf.Round(vec.X), Mathf.Round(vec.Y));
		}


		/// <summary>
		/// rounds the x and y values in place
		/// </summary>
		/// <param name="vec">Vec.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Round(ref Vector2 vec)
		{
			vec.X = Mathf.Round(vec.X);
			vec.Y = Mathf.Round(vec.Y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Floor(ref Vector2 val)
		{
			val.X = (int) val.X;
			val.Y = (int) val.Y;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Floor(Vector2 val)
		{
			return new Vector2((int) val.X, (int) val.Y);
		}


		/// <summary>
		/// returns a 0.5, 0.5 vector
		/// </summary>
		/// <returns>The vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 HalfVector()
		{
			return new Vector2(0.5f, 0.5f);
		}


		/// <summary>
		/// compute the 2d pseudo cross product Dot( Perp( u ), v )
		/// </summary>
		/// <param name="u">U.</param>
		/// <param name="v">V.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cross(Vector2 u, Vector2 v)
		{
			return u.Y * v.X - u.X * v.Y;
		}


		/// <summary>
		/// returns the vector perpendicular to the passed in vectors
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(ref Vector2 first, ref Vector2 second)
		{
			return new Vector2(-1f * (second.Y - first.Y), second.X - first.X);
		}


		/// <summary>
		/// returns the vector perpendicular to the passed in vectors
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(Vector2 first, Vector2 second)
		{
			return new Vector2(-1f * (second.Y - first.Y), second.X - first.X);
		}


		/// <summary>
		/// flips the x/y values and inverts the y to get the perpendicular
		/// </summary>
		/// <param name="original">Original.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(Vector2 original)
		{
			return new Vector2(-original.Y, original.X);
		}


		/// <summary>
		/// returns the angle between the two vectors in degrees
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(Vector2 from, Vector2 to)
		{
			Normalize(ref from);
			Normalize(ref to);
			return Mathf.Acos(Mathf.Clamp(Vector2.Dot(from, to), -1f, 1f)) * Mathf.Rad2Deg;
		}


		/// <summary>
		/// returns the angle between left and right with self being the center point in degrees
		/// </summary>
		/// <param name="self">Self.</param>
		/// <param name="left">V left.</param>
		/// <param name="right">V right.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float AngleBetween(this Vector2 self, Vector2 left, Vector2 right)
		{
			var one = left - self;
			var two = right - self;
			return Angle(one, two);
		}


		/// <summary>
		/// given two lines (ab and cd) finds the intersection point
		/// </summary>
		/// <returns>The ray intersection.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="c">C.</param>
		/// <param name="d">D.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GetRayIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection)
		{
			var dy1 = b.Y - a.Y;
			var dx1 = b.X - a.X;
			var dy2 = d.Y - c.Y;
			var dx2 = d.X - c.X;

			if (dy1 * dx2 == dy2 * dx1)
			{
				intersection = new Vector2(float.NaN, float.NaN);
				return false;
			}

			var x = ((c.Y - a.Y) * dx1 * dx2 + dy1 * dx2 * a.X - dy2 * dx1 * c.X) / (dy1 * dx2 - dy2 * dx1);
			var y = a.Y + (dy1 / dx1) * (x - a.X);

			intersection = new Vector2(x, y);
			return true;
		}

		/// <summary>
		/// rounds and converts a Vector2 to a Point
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="vec">Vec.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point RoundToPoint(this Vector2 vec)
		{
			var roundedVec = vec.Round();
			return new Point((int) roundedVec.X, (int) roundedVec.Y);
		}


		/// <summary>
		/// converts a Vector2 to a Vector3 with a 0 z-position
		/// </summary>
		/// <returns>The vector3.</returns>
		/// <param name="vec">Vec.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this Vector2 vec)
		{
			return new Vector3(vec, 0);
		}


		/// <summary>
		/// checks if a triangle is CCW or CW
		/// </summary>
		/// <returns><c>true</c>, if triangle ccw was ised, <c>false</c> otherwise.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="center">Center.</param>
		/// <param name="c">C.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsTriangleCCW(Vector2 a, Vector2 center, Vector2 c)
		{
			return Cross(center - a, c - center) < 0;
		}

		public static void Rotate(this ref Vector2 vec, float radians) {
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			float tx = vec.X;
			float ty = vec.Y;
			vec.X = (cos * tx) - (sin * ty);
			vec.Y = (sin * tx) + (cos * ty);
		}
	}
}
