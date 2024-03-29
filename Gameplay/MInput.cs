using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.InputManager
{
	public class MInput : MonoBehaviour
	{
		// Singleton
		public static MInput i;
		public InputData inputData;

		/// <summary>
		/// Main action that trigger with specified key
		/// </summary>
		public Dictionary<string, Action> input = new Dictionary<string, Action>();
		
		public UnityEvent OnStartReadingKeyState;
		public UnityEvent<KeyCode> OnStopReadingKeyState;
		
		bool readingKey;
		string tempKey;

		void Awake ()
		{
			// Singleton
			i = this;
			
			// Warning if lenght of keycodes and string different / data missing
			if (inputData == null) {Debug.LogError ( "Input data missing" );return;}
			if (inputData.keycodes.Length != inputData.keys.Length) Debug.LogError ( "Keycode and string length are different" );
			
			// Supply
			foreach ( var key in inputData.keys ) input.Add ( key, () => { } );
		}

		void Update ()
		{
			if ( readingKey )
			{
				if ( !Input.anyKeyDown ) return;
				StartReadInputState ( tempKey );
				return;
			}
			
			foreach ( var k in inputData.keycodes )
				if ( Input.GetKeyDown ( k ) )
					input[ inputData.keys[Array.IndexOf ( inputData.keycodes, k )] ]?.Invoke ();
		}

		/// <summary>
		/// Manually change keycode with specified key
		/// </summary>
		public void SetKey (string _key, KeyCode _keyCode) => inputData.keycodes[Array.IndexOf ( inputData.keys, _key )] = _keyCode;
		/// <summary>
		/// Return keycode with specified key
		/// </summary>
		public KeyCode GetKey ( string _key ) => inputData.keycodes[Array.IndexOf ( inputData.keys, _key )];

		/// <summary>
		/// Start Reading Key State 
		/// The next input will assign to inputed key string
		/// Warning: do not invoke this method using keyboard input, use button instead 
		/// </summary>
		public void StartReadInputState (string _key)
		{
			// Start reading key state
			if ( !readingKey )
			{
				readingKey = true;
				tempKey = _key;
				OnStartReadingKeyState?.Invoke ();
				return;
			}
			
			// Set key on reading key state
			foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
			{
				if ( !Input.GetKey ( kcode ) ) continue;
				SetKey ( tempKey, kcode);
				OnStopReadingKeyState?.Invoke (kcode);
			}
			readingKey = false;
		}
	}
}