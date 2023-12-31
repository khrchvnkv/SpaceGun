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
            public float Magnitude { get; set; }
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
                AddBody(bulletPhysic.Rigidbody);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out BulletPhysic bulletPhysic) && _bodyInfos.ContainsKey(bulletPhysic.Rigidbody))
            {
                RemoveBody(bulletPhysic.Rigidbody);
            }
        }
        
        private void FixedUpdate()
        {
            foreach (var pair in _bodyInfos)
            {
                var rb = pair.Key;
                var info = pair.Value;
 
                UpdateBody(rb, info);
            }
        }

        private void AddBody(in Rigidbody2D rb)
        {
            var info = new BodyData(rb);
            rb.gravityScale = Mathf.Pow(TimeFactor, 2);
            _bodyInfos.Add(rb, info);
        }

        private void RemoveBody(in Rigidbody2D rb)
        {
            var info = _bodyInfos[rb];
            rb.gravityScale = 1;
            rb.velocity = info.UnscaledVelocity.normalized * info.Magnitude;
            _bodyInfos.Remove(rb);
        }

        private void UpdateBody(in Rigidbody2D rb, in BodyData info)
        {
            if (info.LastVelocity.HasValue)
            {
                var rbVelocity = rb.velocity;

                if (Vector2.Dot(rbVelocity, info.UnscaledVelocity) < 0.8f)
                {
                    info.Magnitude = rbVelocity.magnitude / TimeFactor;
                    info.UnscaledVelocity = rbVelocity.normalized * info.Magnitude;
                }

                var acceleration = (rbVelocity - info.LastVelocity.Value).normalized *
                                   (TimeFactor * info.Magnitude * Time.fixedDeltaTime);
                    
                info.UnscaledVelocity += acceleration;
            }

            info.LastVelocity = rb.velocity = info.UnscaledVelocity * TimeFactor;
            Debug.DrawRay(rb.position, rb.position + info.UnscaledVelocity, Color.green);
        }
    }
}