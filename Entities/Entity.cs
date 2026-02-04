using System;
using UnityEngine;
using Noba.ObjectPool;
using Noba.Health;

/// <summary>
/// Basic example of an entity using the PooledMonoBehaviour.
/// </summary>
/// 

namespace Noba
{
    [Serializable]
    public enum ENTITY_FRICTION_BEHAVIOUR
    {
        ALWAYS_APPLY,
        APPLY_ABOVE_MAX_SPEED,
        NEVER_APPLY
    }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(HealthComponent))]

    public class Entity : PooledMonoBehaviour
    {
        [Header("Entity")]
        [SerializeField] protected Rigidbody rigidbody;
        [SerializeField] protected HealthComponent healthComponent;
        [SerializeField] protected float frictionPercentageOfMaxSpeed = 2f;
        [SerializeField] protected ENTITY_FRICTION_BEHAVIOUR frictionBehaviour = ENTITY_FRICTION_BEHAVIOUR.APPLY_ABOVE_MAX_SPEED;

        protected Vector3 linearVelocity;
        protected float linearVelocityMaxSpeed;
        public float CurrentSpeed => linearVelocity.magnitude;

        protected override void Awake()
        {
            InitialiseHealthComponent();

            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        protected override void Update()
        {

        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (rigidbody)
            {
                rigidbody.linearVelocity = linearVelocity;
            }

            switch (frictionBehaviour)
            {
                case ENTITY_FRICTION_BEHAVIOUR.ALWAYS_APPLY:
                    linearVelocity = Vector3.MoveTowards(linearVelocity, Vector3.zero, CurrentSpeed * frictionPercentageOfMaxSpeed * Time.fixedDeltaTime);

                    break;

                case ENTITY_FRICTION_BEHAVIOUR.APPLY_ABOVE_MAX_SPEED:
                    if (CurrentSpeed > linearVelocityMaxSpeed)
                    {
                        linearVelocity = Vector3.MoveTowards(linearVelocity, Vector3.zero, CurrentSpeed * frictionPercentageOfMaxSpeed * Time.fixedDeltaTime);
                    }

                    break;

                case ENTITY_FRICTION_BEHAVIOUR.NEVER_APPLY:

                    break;
            }
        }

        protected virtual void Move(Vector3 velocityChange)
        {
            linearVelocity += velocityChange;
        }

        protected virtual void Knockback(Entity entity)
        {
            Vector3 normal = (transform.position - entity.gameObject.transform.position).normalized;

            linearVelocity += normal * entity.CurrentSpeed;

            Debug.Log(String.Format("Knockback affecting {0} from {1}", gameObject.name, entity.gameObject.name));
        }

        protected virtual void OnDestroy()
        {
            if (!healthComponent)
            {
                return;
            }

            healthComponent.OnHit -= this.OnHit;
            healthComponent.OnDeath -= this.OnDeath;
            healthComponent.OnHeal -= this.OnHeal;
            healthComponent.OnInvincibleEnd -= this.OnInvincibleEnd;
            healthComponent.OnInvincibleStart -= this.OnInvincibleStart;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Entity>(out Entity entity))
            {
                entity.Knockback(this);
            }
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            if (healthComponent)
            {
                healthComponent.SetHealth(healthComponent.MaxHealth);
            }

            linearVelocity = Vector3.zero;
        }

        protected virtual void InitialiseHealthComponent()
        {
            if (!healthComponent)
            {
                return;
            }

            healthComponent.OnHit += this.OnHit;
            healthComponent.OnDeath += this.OnDeath;
            healthComponent.OnHeal += this.OnHeal;
            healthComponent.OnInvincibleEnd += this.OnInvincibleEnd;
            healthComponent.OnInvincibleStart += this.OnInvincibleStart;
        }

        protected virtual void OnHit(Damage damage)
        {

        }

        protected virtual void OnHeal()
        {

        }

        protected virtual void OnDeath()
        {
            ReturnToPool();
        }

        protected virtual void OnInvincibleEnd()
        {

        }

        protected virtual void OnInvincibleStart()
        {

        }
    }
}