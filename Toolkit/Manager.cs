#if UNITY_EDITOR
using UnityEditor;
#endif

using Toolkit.Movement;

using UnityEngine;

namespace Toolkit
{
	public class Manager : MonoBehaviour
	{
		
	}

	#if UNITY_EDITOR
	[CustomEditor ( typeof (Manager) )]
	public class ManagerEditor : Editor
	{
		[ MenuItem ( "GameObject/Manager/Manager",false,0 ) ]
		public static void CreateManager ()
		{
			var x = GameObject.Find ( "Manager" );
			if ( x == null)  
				x = new GameObject ( "Manager" );

			var t = new GameObject ( "InputManager",typeof(MInput) );
			t.transform.SetParent ( x.transform );
			Selection.activeObject = t;
		}
		
	}
	#endif
}