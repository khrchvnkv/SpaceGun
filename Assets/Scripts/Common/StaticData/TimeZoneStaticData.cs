using UnityEngine;

namespace Common.StaticData
{
    [CreateAssetMenu(menuName = nameof(TimeZoneStaticData), fileName = nameof(TimeZoneStaticData), order = 0)]
    public sealed class TimeZoneStaticData : ScriptableObject
    {
        [field: SerializeField, Range(0.1f, 1.0f)] public float TimeZoneScale { get; private set; }
    }
}