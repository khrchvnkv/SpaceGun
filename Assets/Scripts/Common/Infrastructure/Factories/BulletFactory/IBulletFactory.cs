using Common.UnityLogic.Bullet;
using UnityEngine;

namespace Common.Infrastructure.Factories.BulletFactory
{
    public interface IBulletFactory
    {
        BulletPhysic Spawn(Transform parent);
        void Despawn(BulletPhysic bulletPhysic);
    }
}