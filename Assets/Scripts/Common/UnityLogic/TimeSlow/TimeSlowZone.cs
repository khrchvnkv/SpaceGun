using System.Collections.Generic;
using Common.Infrastructure.Services.StaticData;
using Common.StaticData;
using Common.UnityLogic.Bullet;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.TimeSlow
{
    public sealed class TimeSlowZone : MonoBehaviour
    {
        private readonly Dictionary<Rigidbody2D, BodyData> _bodyInfos = new();

        private TimeZoneStaticData _timeZoneStaticData;

        private float TimeFactor => _timeZoneStaticData.TimeZoneScale;

        private class BodyData
        {
            public readonly float Magnitude;
            public Vector2 UnscaledVelocity { get; set; }
            public Vector2? LastVelocity { get; set; }

            public BodyData(Rigidbody2D rigidbody)
            {
                UnscaledVelocity = rigidbody.velocity;
                LastVelocity = null;
                Magnitude = UnscaledVelocity.magnitude;
            }
        }

        [Inject]
        private void Construct(IStaticDataService staticDataService) =>
            _timeZoneStaticData = staticDataService.GameStaticData.TimeZoneStaticData;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out BulletPhysic bulletPhysic) && !_bodyInfos.ContainsKey(bulletPhysic.Rigidbody))
            {
                var info = new BodyData(bulletPhysic.Rigidbody);
                bulletPhysic.Rigidbody.gravityScale = TimeFactor;
                _bodyInfos.Add(bulletPhysic.Rigidbody, info);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out BulletPhysic bulletPhysic) && _bodyInfos.ContainsKey(bulletPhysic.Rigidbody))
            {
                var rb = bulletPhysic.Rigidbody;
                var info = _bodyInfos[rb];
                bulletPhysic.Rigidbody.gravityScale = 1;
                rb.velocity = rb.velocity.normalized * info.UnscaledVelocity.magnitude;
                _bodyInfos.Remove(rb);
            }
        }
        
        private void FixedUpdate()
        {
            foreach (var pair in _bodyInfos)
            {
                var rb = pair.Key;
                var info = pair.Value;
 
                if (info.LastVelocity.HasValue)
                {
                    if (Vector2.Dot(rb.velocity, info.UnscaledVelocity) < 0.0f)
                    {
                        var magnitude = info.UnscaledVelocity.magnitude;
                        info.UnscaledVelocity = rb.velocity.normalized * magnitude;
                    }

                    var acceleration = (rb.velocity - info.LastVelocity.Value).normalized *
                                       (TimeFactor * info.Magnitude * Time.fixedDeltaTime);
                    
                    info.UnscaledVelocity += acceleration;
                }

                info.LastVelocity = rb.velocity = info.UnscaledVelocity * TimeFactor;
                Debug.DrawRay(rb.position, rb.position + info.UnscaledVelocity, Color.green);
            }
        }
    }
}