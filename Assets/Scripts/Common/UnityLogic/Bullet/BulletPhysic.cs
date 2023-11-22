using UnityEngine;

namespace Common.UnityLogic.Bullet
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class BulletPhysic : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        
        private void OnValidate() => Rigidbody ??= gameObject.GetComponent<Rigidbody2D>();

        public void AddForce(Vector3 direction, float force)
        {
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.AddForce(direction * force, ForceMode2D.Force);
        }
    }
}