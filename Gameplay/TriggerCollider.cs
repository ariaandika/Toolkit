using System;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit
{
    public class TriggerCollider : MonoBehaviour
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;
        public event Action<Collider> OnStay;

        public UnityEvent<Collider> OnEnterUnity; 
        public UnityEvent<Collider> OnExitUnity; 
        public UnityEvent<Collider> OnStayUnity; 
        
        void OnTriggerEnter ( Collider other )
        {
            OnEnter?.Invoke ( other );
            OnEnterUnity?.Invoke ( other );
        }

        void OnTriggerExit ( Collider other )
        {
            OnExit?.Invoke ( other );
            OnExitUnity?.Invoke ( other );
        }

        void OnTriggerStay ( Collider other )
        {
            OnStay?.Invoke ( other );
            OnStayUnity?.Invoke ( other );
        }
    }
}
