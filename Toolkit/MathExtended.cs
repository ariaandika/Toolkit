using System;
using UnityEngine;
using Random = System.Random;

namespace Toolkit
{
	public static class VectorExtended
	{
		public static Vector2 up (this float x) => new Vector2(0,x);
		public static Vector2 up (this float y, float x) => new Vector2(x,y);
		public static Vector2 right (this float x) => new Vector2(x,0);
		public static Vector2 right (this float x, float y) => new Vector2(x,y);
		public static Vector2 sp ( this float x ) => new Vector2 ( x, x);
		
		public static Vector3 v ( this float x ) => new Vector3 ( x, x, x );
		public static Vector3 u (this float x) => new Vector3(0,x);
		public static Vector3 u (this float y, float x) => new Vector3(x,y);
		public static Vector3 r (this float x) => new Vector3(x,0);
		public static Vector3 r (this float x, float y) => new Vector3(x,y);
		public static Vector3 f (this float z) => new Vector3(0,0,z);
		public static Vector3 f (this float z, float x) => new Vector3(x,0,z);
		public static Vector3 f (this float z, float x, float y) => new Vector3(x,y,z);
		
		// Vector Angle
		public static float VecToAng ( this Vector2 v ) => ((float)Math.Atan2 ( v.y, v.x )).ToDeg ();
		public static Vector2 AngToVec ( this float deg ) => new Vector2(deg.Cos (), deg.Sin ());
		
		// Utility vec2
		public static float Angle ( this Vector2 from, Vector2 to ) => (from.VecToAng () - to.VecToAng ()).Abs ();

		public static Vector2 LerpDirection ( this Vector2 from, Vector2 to, float t ) => t.Lerp ( from.VecToAng (), to.VecToAng () ).AngToVec (); 
        public static Vector2 Round(this Vector2 x) => new Vector2(x.x.Round (), x.y.Round ());
		public static Vector2 Convert ( this Vector3 x ) => new Vector2(x.x, x.y);
		public static Vector2 Z2Y(this Vector3 x) => new Vector2(x.x,x.z);
		
		// Vector Direction
		/// <summary>
		/// 0 = -->, CounterClockwise 
		/// </summary>
		public static Vector2 Rotate (this Vector2 v, float angle) => (v.VecToAng () + angle).AngToVec ();
		public static Vector2 ClampDirection(this Vector2 x) => x.sqrMagnitude > 1 ? x.normalized : x;
		public static Vector2 CrossCounter ( this Vector2 dir ) => new Vector2(-dir.normalized.y,dir.normalized.x);
		public static Vector2 CrossClockwise ( this Vector2 dir ) => new Vector2(dir.normalized.y,-dir.normalized.x);
		public static Vector2 Dir ( this Vector2 from, Vector2 to ) => (to - from).normalized;
		public static float Dist ( this Vector2 from, Vector2 to ) => (to - from).magnitude;
		
		// Individual component
		public static Vector2 ClampX ( this Vector2 v, float min, float max ) => new Vector2(v.x.Clamp(min,max),v.y);
		public static Vector2 ClampY ( this Vector2 v, float min, float max ) => new Vector2(v.x,v.y.Clamp(min, max));
		public static Vector2 GetX (this Vector2 v) => new Vector2(v.x,0);
		public static Vector2 GetY (this Vector2 v) => new Vector2(0,v.y);
		public static Vector2 DoX (this Vector2 v, Func<float,float> callback) => new Vector2(callback.Invoke(v.x),v.y);
		public static Vector2 DoY (this Vector2 v, Func<float,float> callback) => new Vector2(v.x,callback.Invoke(v.y));

		
		// Utility vec3
		public static float Dist ( this Vector3 from, Vector3 to ) => (to - from).magnitude;
        public static Vector3 Round(this Vector3 x) => new Vector3(x.x.Round (), x.y.Round (), x.z.Round ());
		public static Vector3 Convert ( this Vector2 x, float z = 0 ) => new Vector3(x.x, x.y, z);
		public static Vector3 Lerp ( this float t, Vector3 a, Vector3 b ) => new Vector3 ( a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t );

		public static Vector3 MoveToward ( this Vector3 current, Vector3 target, float speed )
		{
			var o = current.Dir ( target ) * speed + current;
			return current.Dir ( o, false ).sqrMagnitude > current.Dir ( target, false ).sqrMagnitude ? target : o;
		}
		/// <summary>
		/// True if still moving, false when arrive at target
		/// </summary>
		public static bool MoveToward ( this Vector3 current, Vector3 target, out Vector3 result, float speed )
		{
			var o = current.Dir ( target ) * speed + current;
			if ( current.Dir ( o, false ).sqrMagnitude > current.Dir ( target, false ).sqrMagnitude ){
				result = target;
				return false;
			}
			result = o;
			return true;
		}

		// Vector direction
		public static Vector3 Dir ( this Vector3 from, Vector3 to, bool normalized = true ) => normalized ? (to - from).normalized : to - from;
		public static Vector3 ClampDirection(this Vector3 x) => x.sqrMagnitude > 1 ? x.normalized : x;
		public static Vector3 ClampMagnitude(this Vector3 x, float max) => x.sqrMagnitude > max * max ? x.normalized * max : x;
		public static Vector3 ClampMagnitude2(this Vector3 x, float min, float max) => x.magnitude.Clamp ( min , max ) * x.normalized;

		public static Vector3 LocalLegacy ( this Vector3 v, Vector3 direction, Vector3 axisTarget, float length = 0.2f ) => v + Quaternion.LookRotation ( direction ) * axisTarget * length;
		public static Vector3 LocalBezier ( this (Vector3 pos,Vector3 tan)v, Vector3 axisTarget, float length = 0.2f ) => v.pos + Quaternion.LookRotation ( v.tan ) * axisTarget * length;
		public static Vector3 Local ( this Vector3 v, Vector3 axisTarget, float length = 0.2f ) => v + Quaternion.LookRotation ( v ) * axisTarget * length;
		public static Vector3 Local ( this Vector3 v, Vector3 direction, Vector3 axisTarget, float length = 0.2f ) => v + Quaternion.LookRotation ( direction - v ) * axisTarget * length;

		// Individual component
		public static Vector3 DoX (this Vector3 v, Func<float,float> callback) => new Vector3(callback.Invoke(v.x),v.y,v.z);
		public static Vector3 DoY (this Vector3 v, Func<float,float> callback) => new Vector3(v.x,callback.Invoke(v.y),v.z);
		public static Vector3 DoZ (this Vector3 v, Func<float, float> callback) =>new Vector3(v.x,v.y,callback.Invoke(v.z));
		public static Vector3 DoX (this Vector3 v, float value) => new Vector3(value,v.y,v.z);
		public static Vector3 DoY (this Vector3 v, float value) => new Vector3(v.x,value,v.z);
		public static Vector3 DoZ (this Vector3 v, float value) =>new Vector3(v.x,v.y,value);
		
		public static Vector3 Y2Z(this Vector2 x) => new Vector3(x.x,0,x.y);
		
		public static Vector3 XtoY(this Vector3 x) => new Vector3(0,x.x,x.y);
		public static Vector3 XtoZ(this Vector3 x) => new Vector3(0,x.y,x.x);
		public static Vector3 YtoZ(this Vector3 x) => new Vector3(x.x,0,x.y);
		
		
	}
	public static class MathExtended
	{
		public static float Clamp ( this float x) => x < 0 ? 0 : x > 1 ? 1 : x;
		public static float Clamp ( this float x, float min, float max ) => x > max ? max : x < min ? min : x;
		public static float Clamp ( this float x,float max) => x > max ? max : x;
		public static float ClampMin ( this float x, float min ) => x < min ? min : x;
		public static float ClampNegative ( this float x) => x < 0 ? 0 : x;
		public static float Abs ( this float x ) => x < 0 ? x * -1 : x;
		
		public static int Clamp ( this int x) => x < 0 ? 0 : x > 1 ? 1 : x;
		public static int Clamp ( this int x,int max) => x > max ? max : x;
		public static int Clamp ( this int x, int min, int max ) => x > max ? max : x < min ? min : x;
		public static int ClampNegative ( this int x) => x < 0 ? 0 : x;

		public static bool InBetween ( this float x, float size) => !(x > size) && !(x < -size);
		public static bool InBetween ( this float x, float min, float max ) => !(x > max) && !(x < min);
		public static bool InBetween ( this Vector2 x, Vector2 min, Vector2 max ) => x.x.InBetween ( min.x, max.x ) && x.y.InBetween ( min.y,max.y );
		public static bool InBetween ( this Vector3 x, Vector3 min, Vector3 max ) => x.x.InBetween ( min.x, max.x ) && x.y.InBetween ( min.y,max.y ) && x.z.InBetween ( min.z,max.z );

		public static float Lerp ( this float t, float a, float b ) => a + (b - a) * t;
		public static float InverseLerp ( this float value, float a, float b ) => ((value - a) / (b - a)).Clamp();
		public static float InverseLerpUnclamped ( this float t, float a, float b ) => (t - a) / (b - a);
		public static float Remap ( this float value, float inputa, float inputb, float outa, float outb ) => Lerp ( InverseLerp ( value, inputa, inputb ), outa, outb );

		public static float Round ( this float x ) => x > 0.5 ? x.Ceil () : x.Floor ();
		public static float Floor ( this float x ) => (int)x;
		public static float Ceil ( this float x ) => (int)x + (x < 0 ? -1 : 1);
		public static int FloorToInt ( this float x ) => (int)x;
		public static int CeilToInt ( this float x ) => (int)x + (x < 0 ? -1 : 1);

		public static float Cos ( this float deg ) => (float)Math.Cos ( deg.ToRad () );
		public static float Sin ( this float deg ) => (float)Math.Sin ( deg.ToRad () );

		public static float ToDeg ( this float radian ) => radian * 57.29578f;
		public static float ToRad ( this float deg ) => deg * 0.01745329f;
		
		public static float MoveToward ( this float current, float target, float speed ) => (current - target).Abs () < speed ? target : current + speed;
		public static float MoveToward ( this float current, float target, float speed, Action OnFinish )
		{
			if ( !((current - target).Abs () < speed) ) return current + speed;
			OnFinish?.Invoke ();
			return target;
		}
		
	}
	public static class Tool
	{
		public static int Wrap ( this int x, int max = 1) => x % max;

		public static int Wrap ( this int x, int min, int max) => (x - min) % max + min;
		public static (int result, int fill) Wrap2 ( this int x, int max = 1)
		{
			var xx = 0;
			var _fill = 0;
			while ( x > max ){
				if (xx > 6000) break;
				x -= max;
				_fill += max;
				xx++;
			}
			return (x,_fill);
		}
		
		public static bool Close ( this float a, float b, float threshold = 0.01f ) => (a - b).Abs () <= threshold;
		public static bool Close ( this Vector2 a, Vector2 b, float threshold = 0.01f ) => (a.sqrMagnitude - b.sqrMagnitude).Abs () <= threshold * threshold;
		public static bool Close ( this Vector3 a, Vector3 b, float threshold = 0.01f ) => (a.sqrMagnitude - b.sqrMagnitude).Abs () <= threshold * threshold;
		
		public static float ToValue ( this bool v ) => v ? 1 : 0;
		public static float ToValue ( this bool v, bool negativeCondition ) => v ? 1 : negativeCondition ? -1 : 0;

		public static bool IsEven ( this int i ) => i % 2 == 0;

		public static float Wrap ( this float x, float max = 1) => x % max;
		public static (float result, float fill) Wrap2 ( this float x, float max = 1)
		{
			var xx = 0;
			var _fill = 0f;
			while ( x > max ){
				if (xx > 6000) break;
				x -= max;
				_fill += max;
				xx++;
			}
			return (x,_fill);
		}
		public static float WrapBounce ( this float x ) => x.FloorToInt ().IsEven () ? 1 - x.Wrap () : x.Wrap ();
		public static float WrapLegacy ( this float x ) => x - x.Floor ();
		public static float WrapLegacy2 ( this float x, float max = 1 )
		{
			var xx = 0;
			while ( x > max ){
				if (xx > 6000) break;
				x -= max;
				xx++;
			}
			return x;
		}
	}
}