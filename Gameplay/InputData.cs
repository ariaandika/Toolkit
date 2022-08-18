using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.InputManager
// input owng
namespace Toolkit
{
	[CreateAssetMenu(fileName = "InputData",menuName = "InputData",order = 1)]
	public class InputData : ScriptableObject
	{
		
		public KeyCode[] keycodes = {
			KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.C
		};
		public string[] keys = {
			"Q", "E", "R", "C"
		};
	}
}
