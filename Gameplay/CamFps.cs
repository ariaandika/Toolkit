using System;
using UnityEngine;

namespace Toolkit.CameraSystem
{
    public class CamFps : MonoBehaviour
    {
        /// <summary>
        /// First Person View
        /// Put this in camera holder
        /// </summary>
        
        [Header("Camera")]
        public float sensitivity = 4;
        public static Camera cam;
        public bool invertX;
        public bool invertY;
        
        Transform playerBody;
        Transform t;
        
        float xRotation;

        [Header ( "Interaction, use ICameraInteract on object" )] 
        public float interactionDistance = 1;

        public KeyCode interactKeycode = KeyCode.Mouse0;
        public KeyCode interactKeycode2 = KeyCode.Q;
        public KeyCode interactKeycode3 = KeyCode.E;
        
        void Start()
        {
            t = transform;
            playerBody = t.parent;
            
            cam = GetComponentInChildren<UnityEngine.Camera>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            #region Look Around
            var mouseX = Input.GetAxis("Mouse X") * sensitivity * 100 * Time.deltaTime * (invertX ? -1 : 1);
            var mouseY = Input.GetAxis("Mouse Y") * sensitivity * 100 * Time.deltaTime * (invertY ? -1 : 1);
            
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            t.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
            #endregion

            if ( Input.GetKeyDown ( interactKeycode ) )
                Interact ( TargetFunction.Func1 );
            else if ( Input.GetKeyDown ( interactKeycode2 ) )
                Interact ( TargetFunction.Func2 );
            else if ( Input.GetKeyDown ( interactKeycode3 ) )
                Interact ( TargetFunction.Func3 );
        }

        /// <summary>
        /// Cast ray from mouse position and return RaycastHit
        /// </summary>
        public bool RawInteract (out RaycastHit hitInfo)
        {
            var ray = cam.ScreenPointToRay ( Input.mousePosition );
            return Physics.Raycast ( ray, out hitInfo, interactionDistance );
        }

        /// <summary>
        /// Cast ray from mouse position and detect ICameraInteract
        /// </summary>
        public void Interact (TargetFunction target)
        {
            if ( !RawInteract ( out var x ) ) return;

            if ( !x.transform.TryGetComponent ( out ICameraInteract i ) ) return;
            
            switch ( target )
            {
                case TargetFunction.Func1: i.OnCameraInteract ();
                    break;
                case TargetFunction.Func2: i.OnCameraInteract1 ();
                    break;
                case TargetFunction.Func3: i.OnCameraInteract2 ();
                    break;
                default:
                    throw new ArgumentOutOfRangeException ( nameof (target), target, null );
            }
        }
        
    }
    public enum TargetFunction
    {
        Func1,
        Func2,
        Func3
    }
    public interface ICameraInteract
    {
        void OnCameraInteract();
        void OnCameraInteract1();
        void OnCameraInteract2();
    }
}