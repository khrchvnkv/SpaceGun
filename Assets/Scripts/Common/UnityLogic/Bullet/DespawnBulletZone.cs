using Common.Infrastructure.Factories.BulletFactory;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Bullet
{
    public sealed class DespawnBulletZone : MonoBehaviour
    {
        private IBulletFactory _bulletFactory;

        [Inject]
        private void Construct(IBulletFactory bulletFactory) => _bulletFactory = bulletFactory;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out BulletPhysic bullet))
            {
                DespawnBullet(bullet);
            }
        }
        private void DespawnBullet(in BulletPhysic bulletPhysic) => _bulletFactory.Despawn(bulletPhysic);
    }
}