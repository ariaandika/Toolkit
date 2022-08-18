using UnityEngine;

namespace Toolkit.Movement
{
	[RequireComponent(typeof(CharacterController))]
	public class Move3D : MonoBehaviour
	{
		CharacterController c;
		Transform t;

		[Header("Movement")]
		public float speed = 4;
		[Range(1,14)]
		public float acceleration = 5;
		[Range(1,14)]
		public float deceleration = 5;
		public float apexModifier = 1.6f;

		[Header ( "Jump" )] 
		public KeyCode jumpKeyCode = KeyCode.Space;
		public float jumpHeight = 10.5f;
		public float gravityScale = 3.4f;
		public float jumpBuffer = 0.1f;

		Vector3 input;
		Vector3 move;
		
		bool jumpInput;
		float bufferJump;
		const float gravity = -9.81f;
		float y;
		
		bool isGrounded => c.isGrounded;

		

		
		void Awake ()
		{
			t = transform;
			c = GetComponent<CharacterController> ();
		}

		void Update ()
		{
			input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
			
			Movement ();
			Jump ();
		}

		void Movement ()
		{
			// collect velocity
			var _target = input.z * t.forward + input.x * t.right; // Default input
			if ( _target.sqrMagnitude > 1 ) _target.Normalize(); // prevent diagonal speed

			
			var _isMoving = move.sqrMagnitude <= _target.sqrMagnitude; // acceleration vs deceleration
			var _apexModifier = isGrounded ? 1 : apexModifier; // apply apex modifier 
						
			// apply
			move = _isMoving ? Vector3.MoveTowards ( move, _target, acceleration * Time.deltaTime ) : Vector3.MoveTowards ( move, _target, deceleration * Time.deltaTime );
			c.Move ( Time.deltaTime * speed * _apexModifier * move );
		}

		public void Jump ()
		{
			// Fix to problem with inconsistent of jumping is: put c.Move BEFORE checking isGrounded
			c.Move ( Time.deltaTime * y * Vector2.up );
			
			// GetJumpInput
			if ( Input.GetKeyDown ( jumpKeyCode ) )
			{
				if ( isGrounded )
					jumpInput = true;
				else
					bufferJump = jumpBuffer;

			}
			if ( isGrounded )
			{
				if ( jumpInput || bufferJump > 0 )
				{
					y = jumpHeight;
					
					bufferJump = 0;
					jumpInput = false;
					return;
				}
				y = -2;
			}
			else
				y += gravity * Time.deltaTime * gravityScale;

			if ( bufferJump > 0 ) bufferJump -= Time.deltaTime;
		}

		// External Hooks
		public void Teleport (Vector3 _pos)
		{
			c.enabled = false;
			transform.position = _pos;
			// ReSharper disable once Unity.InefficientPropertyAccess
			c.enabled = true;
		}

	}
	
}
