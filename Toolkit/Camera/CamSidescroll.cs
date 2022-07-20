using UnityEngine;

namespace Toolkit.Camera
{
    public class CamSidescroll : MonoBehaviour
    {
        public static UnityEngine.Camera cam;
        [SerializeField] Transform target;

        [Space(10)][Range(0,10)]
        [SerializeField] float smooth = 0.3f;

        Vector3 y;

        // External Hook
        public void SetTarget(Transform camTarget) => target = camTarget;

        void Awake() => cam = FindObjectOfType<UnityEngine.Camera>();
        void Update() => transform.position = Vector3.SmoothDamp(transform.position, target.position, ref y, smooth);
    }
}