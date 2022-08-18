using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.MeshGenerator
{
	public static class MeshGenerator
	{
		
		#region HDPlane

		public static Mesh HDPlane (this int _res, float _width = 1, float _height = 1)
		{
			var vert = new List<Vector3>();
			var norm = new List<Vector3>();
			var tri = new List<int>();
			var uv1 = new List<Vector2>();

			var offset = 1f / _res;

			for ( var x = 0; x < _res; x++ )
			{
				for ( var i = 0; i < _res; i++ )
				{
					vert.AddRange ( new []
					{
						new Vector3(offset * (i + 0f) * _width, offset * (x + 0f) * _height).YtoZ (),
						new Vector3(offset * (i + 1f) * _width, offset * (x + 0f) * _height).YtoZ (),
						new Vector3(offset * (i + 0f) * _width, offset * (x + 1f) * _height).YtoZ (),
						new Vector3(offset * (i + 1f) * _width, offset * (x + 1f) * _height).YtoZ ()
					} );
					for ( var j = 0; j < 4; j++ )					
						norm.Add ( Vector3.up );
					
					uv1.AddRange ( new []
					{
						new Vector2(offset * i,offset * x),
						new Vector2(offset * i,offset * x),
						new Vector2(offset * i,offset * x),
						new Vector2(offset * i,offset * x)
					} );
				}
			}
			for ( var i = 0; i < _res * _res; i++ )
			{
				tri.AddRange ( new []
				{
					0 + 4 * i, 3 + 4 * i, 1 + 4 * i,
					0 + 4 * i, 2 + 4 * i, 3 + 4 * i
				} );
			}
			return new Mesh
			{
				name = "HD Plane",
				vertices = vert.ToArray (),
				normals = norm.ToArray (),
				triangles = tri.ToArray (),
				uv = uv1.ToArray ()
				//uv2 = uv2.ToArray ()
			};
		}
		
		public static Mesh HDPlaneConnected (this int _res , float _width = 1, float _height = 1)
		{
			var vert = new List<Vector3>();
			var norm = new List<Vector3>();
			var tri = new List<int>();
			var uv1 = new List<Vector2>();

			var offset = 1f / _res;

			for ( var x = 0; x < _res; x++ )
			{
				for ( var i = 0; i < _res; i++ )
				{
					// Vertices
					vert.Add ( new Vector3(offset * i,offset * x).YtoZ () );
					// Normal & UV
					norm.Add ( Vector3.up );
					uv1.Add ( new Vector2 ( offset * i, offset * x ) );
				}
			}

			for ( var j = 0; j < _res - 1; j++ )
			{
				for ( var i = 0; i < _res - 1; i++ )
				{
					tri.AddRange ( new []
					{
						i + _res * j, _res * (j + 1).ClampNegative () + i, _res * (j + 1).ClampNegative () + i + 1,
						i + _res * j, _res * (j + 1).ClampNegative () + i + 1, _res * j + i + 1
					} );
				}
			}
			return new Mesh
			{
				name = "HD Plane",
				vertices = vert.ToArray (),
				normals = norm.ToArray (),
				triangles = tri.ToArray (),
				uv = uv1.ToArray ()
				//uv2 = uv2.ToArray ()
			};
		}
		#endregion

	}
}
