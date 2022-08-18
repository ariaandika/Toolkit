using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toolkit
{
	public static class Common
	{
		public static void Set ( this (VertexHelper vh, UIVertex v) x, Vector3 value)
		{
			var (vh, v) = x;
			v.position = value;
			vh.AddVert ( v );
		}
		public static void Set ( this (VertexHelper vh, UIVertex v) x, IEnumerable<Vector3> value)
		{
			foreach ( var v in value ){
				x.v.position = v;
				x.vh.AddVert ( x.v );
			}
		}
		public static void Tri ( this VertexHelper vh, int[] tri, int triOffset = 0 )
		{
			var Count = tri.Length; 
			for ( var i = 0; i < Count; i+=3 ) vh.AddTriangle ( tri[i] + triOffset, tri[i + 1] + triOffset, tri[i + 2] + triOffset );
		}
	}
	
}
