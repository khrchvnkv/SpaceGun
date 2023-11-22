using UnityEngine;

namespace Common.StaticData
{
    [CreateAssetMenu(menuName = nameof(BulletStaticData), fileName = nameof(BulletStaticData), order = 0)]
    public class BulletStaticData : ScriptableObject
    {
        [field: SerializeField, Min(0.1f)] public float ForceSpeed { get; private set; }
    }
}