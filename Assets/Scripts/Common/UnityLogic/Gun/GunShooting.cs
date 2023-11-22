using System;
using Common.Infrastructure.Factories.BulletFactory;
using Common.Infrastructure.Services.StaticData;
using Common.StaticData;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Gun
{
    public sealed class GunShooting : MonoBehaviour
    {
        [SerializeField] private Transform _bulletSpawnPoint;
        
        private IBulletFactory _bulletFactory;
        private BulletStaticData _bulletStaticData;

        [Inject]
        private void Construct(IBulletFactory bulletFactory, IStaticDataService staticDataService)
        {
            _bulletFactory = bulletFactory;
            _bulletStaticData = staticDataService.GameStaticData.BulletStaticData;
        }

        private void SpawnBullet()
        {
            var bullet = _bulletFactory.Spawn(_bulletSpawnPoint);
            bullet.AddForce(_bulletSpawnPoint.up, _bulletStaticData.ForceSpeed);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnBullet();
            }
        }
    }
}