using System;
using System.Collections;
using UnityEngine;

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
    public static class Tool
    {
		public static bool Approx ( this float a, float b, float threshold = 0.01f ) => Mathf.Abs(a - b) <= threshold;
		public static bool Approx ( this Vector2 a, Vector2 b, float threshold = 0.01f ) => Mathf.Abs(a.sqrMagnitude - b.sqrMagnitude) <= threshold * threshold;
		public static bool Approx ( this Vector3 a, Vector3 b, float threshold = 0.01f ) => Mathf.Abs(a.sqrMagnitude - b.sqrMagnitude) <= threshold * threshold;
		
		public static float ToValue ( this bool v ) => v ? 1 : 0;
		public static float ToValue ( this bool v, bool negativeCondition ) => v ? 1 : negativeCondition ? -1 : 0;

		public static void Fetch (this MonoBehaviour monoBehaviour, ref Rigidbody c) => c = monoBehaviour.GetComponent<Rigidbody> ();
		public static void Fetch (this MonoBehaviour monoBehaviour, ref CharacterController c) => c = monoBehaviour.GetComponent<CharacterController> ();
    }

	public static class Vector
	{
		public static Vector2 VecByAng ( float angle ) => new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        public static Vector2 Round(this Vector2 x) => new Vector2(Mathf.Round(x.x), Mathf.Round(x.y));
		public static Vector2 Convert ( this Vector3 x ) => new Vector2(x.x, x.y);
		public static Vector2 ZtoY(this Vector3 x) => new Vector2(x.x,x.z);
		public static Vector2 Clamp01(this Vector2 x) => x.sqrMagnitude > 1 ? x.normalized : x;

		public static Vector3 Round(this Vector3 x) => new Vector3(Mathf.Round(x.x), Mathf.Round(x.y), Mathf.Round(x.z));
		public static Vector3 Convert ( this Vector2 x ) => new Vector3(x.x, x.y);
		public static Vector3 FloatToVector(this float[] x) => new Vector3(x[0], x[1], x[2]);
		public static Vector3 YtoZ(this Vector2 x) => new Vector3(x.x,0,x.y);
		public static Vector3 ZtoY(this Vector3 x,bool is3, float y = 0) => new Vector3(x.x,y,x.y);
		public static Vector3 Clamp01(this Vector3 x) => x.sqrMagnitude > 1 ? x.normalized : x;
    }

	public static class Runtime
	{
		public static Vector2 ScreenCenter(this UnityEngine.Camera cam) => new Vector2((float)Screen.width/2,(float)Screen.height/2); 
	} 
	
	public static class Wait
	{
		public static void WaitSecond( this MonoBehaviour x,float time, Action callback) => x.StartCoroutine(IESecond(time, callback));
        static IEnumerator IESecond(float time, Action callback )
        {
	        yield return new WaitForSecondsRealtime(time);
	        callback.Invoke ();
        }
	}
	
	
}