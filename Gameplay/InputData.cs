using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD:Gameplay/InputData.cs

namespace Toolkit.InputManager
=======
// input owng
namespace Toolkit
>>>>>>> 1906d227fd17ce73416c2c2323134a1431f9f7ca:Toolkit/InputData.cs
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
