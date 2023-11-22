using Common.StaticData;
using Common.UnityLogic.Bullet;
using UnityEngine;

namespace Common.Infrastructure.Services.AssetsManagement
{
    public interface IAssetProvider
    {
        GameStaticData LoadGameStaticData();
        BulletPhysic LoadBullet();
        GameObject Load(in string path);
    }
}