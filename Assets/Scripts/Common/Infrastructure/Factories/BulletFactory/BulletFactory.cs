using Common.Infrastructure.Factories.Zenject;
using Common.Infrastructure.Services.AssetsManagement;
using Common.UnityLogic.Bullet;
using UnityEngine;

namespace Common.Infrastructure.Factories.BulletFactory
{
    public sealed class BulletFactory : IBulletFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IZenjectFactory _zenjectFactory;

        private BulletPhysic _bulletPhysicPrefab;

        public BulletFactory(IAssetProvider assetProvider, IZenjectFactory zenjectFactory)
        {
            _assetProvider = assetProvider;
            _zenjectFactory = zenjectFactory;
        }

        public BulletPhysic Spawn(Transform parent)
        {
            LoadBulletPrefab();
            var bullet = _zenjectFactory.Instantiate(_bulletPhysicPrefab, parent);
            return bullet;
        }
        public void Despawn(BulletPhysic bulletPhysic)
        {
            // TODO: Add pooling
            Object.Destroy(bulletPhysic.gameObject);
        }

        private void LoadBulletPrefab() => _bulletPhysicPrefab ??= _assetProvider.LoadBullet();
    }
}