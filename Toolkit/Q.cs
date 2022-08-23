using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Toolkit
{
	/// <summary>
	/// Data:
	/// Raycast use vector direction as second parameter
	/// Use more vector direction rather than Quaternion
	///
	/// ScreenCoordinate:
	/// "cam.ScreenToViewportPoint"	is vector2 from 0 (down left) to 1 (top right)
	/// "cam.ScreenToWorldPoint"	is world vector3 relative to camera position; TIP: input z with "cam.nearClipPlane + offset" 
	/// "Input.mouseInput"			is vector2 with "viewport origin" in pixel
	/// 
	/// </summary>
	#region CoreKeyword

	public static class CoreKeyword
	{
		public static void Do ( this bool condition, Action True, Action False = default )
		{if (condition) True?.Invoke (); else False?.Invoke ();}
		public static void DoNot ( this bool condition, Action True, Action False = default )
		{if (!condition) True?.Invoke (); else False?.Invoke ();}
		public static void Do (this int iteration, Action<int> action, bool lequal = false ,int step = 1)
		{for ( var i = 0; i < iteration + (lequal ? 1 : 0); i+=step ) action.Invoke ( i );}
		public static void Do (this int iteration, Action<int,float> action, bool lequal = true)
		{for ( var i = 0; i < iteration + lequal.ToValue(); i++ ) action.Invoke ( i, (float) i / iteration );}
		public static void Do<T> (this List<T> x, Action<int, T> action)
		{for ( var i = 0; i < x.Count; i++ ) action.Invoke ( i,x[i] );}
		public static void DoNext<T> (this List<T> x, Action<T, T> action)
		{for ( var i = 0; i < x.Count - 1; i++ ) action.Invoke ( x[i],x[i + 1] );}
		public static void DoNext<T> (this List<T> x, Action<T, T, int> action)
		{for ( var i = 0; i < x.Count - 1; i++ ) action.Invoke ( x[i],x[i + 1],i );}
		
		public static List<O> DoEach<T,O> (this IEnumerable<T> x, Func<T,O> action)
		{
			var enumerable = x.ToList ();
			var counter = enumerable.Count; var o = new List<O> ();
			for ( var i = 0; i < counter; i++ ) o.Add ( action ( enumerable[i] ));
			return o;
		}
		public static IEnumerable<T> DoEach<T> (this IEnumerable<T> x, Action<T> action)
		{
			var numer = x as T[] ?? x.ToArray ();
			foreach ( var i in numer ){
				action ( i );
			}
			return numer;
		}

		public static void p ( this string x ) => Debug.Log ( x );
	}

	#endregion

	#region Spline

	public static class SplineExtended
	{
		public static (Vector3 point,Vector3 tangent) GetBezier (this float _t, List<Vector3> points)
		{
			if (points.Count != 4) return default;
			
			var len = points.Count;
			var outerPoints = new Vector3[len - 1];
			var innerPoints = new Vector3[len - 2];

			for ( var i = 0; i < len - 1; i++ )
				outerPoints[i] = Vector3.Lerp ( points[i], points[i + 1], _t );

			for ( var i = 0; i < len - 2 ; i++ )
				innerPoints[i] = Vector3.Lerp ( outerPoints[i], outerPoints[i + 1], _t );

			return (Vector3.Lerp ( innerPoints[0], innerPoints[1], _t ), innerPoints[1] - innerPoints[0]);
		}

		public static (Vector3 point, Vector3 tangent) GetCubicBezier (this float t, List<Vector3> points)
		{
			var pa = t.Lerp ( t.Lerp ( points[0], points[1] ), t.Lerp ( points[1], points[2] ) );
			var pb = t.Lerp ( t.Lerp ( points[1], points[2] ), t.Lerp ( points[1], points[3] ) );
			return (t.Lerp ( pa, pb ), pb - pa);
		}

		public static (Vector3 point, Vector3 tangent) GetQuadraticBezier ( this float t, List<Vector3> points )
		{
			var p1 = t.Lerp ( points[0], points[1] );
			var p2 = t.Lerp ( points[1], points[2] );
			return (t.Lerp ( p1, p2 ),p2 - p1);
		}
	}

	#endregion

	
	#region Linq Extended
	public static class LinqExtended
	{
		public static Queue<T> ToQueue<T> ( this IEnumerable<T> x )
		{
			var o = new Queue<T>(); foreach ( var q in x ) o.Enqueue ( q ); return o;
		}

		public static T Last<T> ( this List<T> x, int idx = 0 ) => x[x.Count + (- idx - 1).Clamp(-x.Count, -1)];
		public static bool CheckEach<T> ( this IEnumerable<T> x, Func<T, bool> action, out T t )
		{
			t = default;
			foreach ( var v in x ){
				if ( !action.Invoke ( v ) ) continue;
				t = v;
				return true;
			}
			return false;
		}
		public static bool CheckEach<T> ( this IEnumerable<T> x, Func<T,bool> action )
		{
			return x.Any ( action.Invoke );
		}

		public static float Max ( this List<float> collection )
		{
			var o = collection[0];
			collection.DoEach ( x => { o = x > o ? x : o; } );
			return o;
		}
		public static int Max ( this List<int> collection )
		{
			var o = collection[0];
			collection.DoEach ( x => { o = x > o ? x : o; } );
			return o;
		}
		
		public static IEnumerable<Transform> GetChildren( this Transform t)
		{
			var x = new List<Transform> ();
			foreach ( Transform child in t ) 
				x.Add ( child );
			return x;
		}

		public static List<T> ToList<T> ( this T v, int range )
		{
			var o = new List<T>();
			range.Do ( x => o.Add ( v ));
			return o;
		} 
	}
	#endregion

	#region CameraExtended

	public static class CameraExtended
	{
		public static bool Cast ( this Camera cam, out RaycastHit hit, LayerMask layerMask,float distance = Mathf.Infinity ) => Physics.Raycast ( cam.ScreenPointToRay ( Input.mousePosition ), out hit, distance, layerMask );
		public static Vector2 ScreenCenter(this Camera cam) => new Vector2((float)Screen.width/2,(float)Screen.height/2);
	} 
	#endregion

	#region Wait

	public static class Wait
	{
		public static void WaitSecond( this MonoBehaviour x,float time, Action callback) => x.StartCoroutine(IESecond(time, callback));
        static IEnumerator IESecond(float time, Action callback )
        {
	        yield return new WaitForSecondsRealtime(time);
	        callback.Invoke ();
        }
	}
	#endregion


	#region Mesh

	public class MeshTemp
	{
		readonly List<Vector3> vert;
		readonly List<int> tri;
		List<Color> col;
		
		public int vertCount => vert.Count;
		public int colCount => col.Count;
		public int TriOffset => tri.Max () + 1;

		public void Vert (Vector3 _vert) => vert.Add ( _vert );
		public void Col (Color _col) => col.Add ( _col );
		public void Vert (IEnumerable<Vector3> _vert) => _vert.DoEach ( x => vert.Add ( x ) );
		public void Col ( IEnumerable<Color> _col ) => _col.DoEach ( x => col.Add ( x ) );
		
		public void Tri (IEnumerable<int> _tri, int offset = 0) => _tri.DoEach ( x => tri.Add ( x + offset ) );
		/// <summary>
		/// try use this at the end, so the length match
		/// </summary>
		public void ColSingle (Color _col) => col = _col.ToList(vertCount);
		
		public MeshTemp (Mesh mesh = default)
		{
			if ( mesh != null ) {
				vert = mesh.vertices.ToList ();
				tri = mesh.triangles.ToList ();
				col = mesh.colors.ToList ();
			}
			else{
				vert = new List<Vector3>();
				tri = new List<int>();
				col = new List<Color>();
			}
		}
		
		public void Set ( Mesh _m )
		{
			_m.vertices = vert.ToArray ();
			_m.triangles = tri.ToArray ();
			_m.colors = col.ToArray ();
		}

		public int Merge (Mesh _m, int triOffset = 0, Vector3 _offset = default)
		{
			var tempVert = _m.vertices.ToList();
			var tempTri = _m.triangles.ToList();
			var tempCol = _m.colors.ToList ();
			
			var _tri = tri;
			_tri.DoEach ( x => x + triOffset );
			
			var _vert = vert;
			_vert.DoEach ( x => x + _offset );

			var _col = col;
			
			tempVert.AddRange ( _vert );
			tempTri.AddRange ( _tri );
			tempCol.AddRange( _col );

			_m.vertices = tempVert.ToArray ();
			_m.triangles = tempTri.ToArray ();
			_m.colors = tempCol.ToArray ();
			return tempTri.Max () + 1;
		}
	}

	#endregion
}