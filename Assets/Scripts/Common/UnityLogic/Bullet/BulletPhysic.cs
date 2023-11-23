using UnityEngine;

namespace Common.UnityLogic.Bullet
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class BulletPhysic : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

        private Color _color;
        
        private void OnValidate() => Rigidbody ??= gameObject.GetComponent<Rigidbody2D>();

        public void AddForce(Vector3 direction, float force)
        {
            _lineRenderer.positionCount = 0;
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.AddForce(direction * force, ForceMode2D.Force);
        }

        private void FixedUpdate()
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, Rigidbody.position);
        }
    }
}