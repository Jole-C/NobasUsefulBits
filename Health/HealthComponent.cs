using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

/// <summary>
/// A health component that can handle damage, healing, and invincibility.
/// Supports built in immunity to damage types, and events that can be returned for objects to respond to damage.
/// Requires https://github.com/ayellowpaper/SerializedDictionary
/// Requires https://github.com/akbiggs/UnityTimer
/// </summary>
/// 

namespace Noba.Health
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action<Damage> OnHit;
        public event Action OnHeal;
        public event Action OnDeath;
        public event Action OnInvincibleStart;
        public event Action OnInvincibleEnd;

        [Header("Behaviour")]
        [field: SerializeField] public int MaxHealth { get; private set; } = 3;
        [SerializeField] float invincibilityTime = 0.2f;
        [SerializeField] SerializedDictionary<DAMAGE_TYPES, bool> damageImmunities = new SerializedDictionary<DAMAGE_TYPES, bool>();
        [SerializeField] bool triggerEventsOnly = false;

        [Header("Debug")]
        [SerializeField] bool debugInvincible = false;
        [SerializeField] bool debugText = false;

        public float Health { get; private set; }
        public bool Invincible { get; private set; } = false;


        public void SetHealth(float health)
        {
            Health = health;
        }

        public bool Heal(float healAmount)
        {
            if (Health >= MaxHealth)
            {
                return false;
            }

            Health += healAmount;

            Health = Mathf.Clamp(Health, 0, MaxHealth);

            OnHeal?.Invoke();

            if (debugText)
            {
                Debug.LogWarning(String.Format("Healing game object: {0} for {1} health.", gameObject.name, healAmount));
            }

            return true;
        }

        public float TryHit(Damage damage)
        {
            if (debugInvincible || Invincible || CheckDamageTypeImmunity(damage))
            {
                return Health;
            }

            return Health - damage.DamageAmount;
        }

        public bool Hit(Damage damage)
        {
            if (((debugInvincible || Invincible) && !damage.IgnoreCurrentInvulnerabilityState) || CheckDamageTypeImmunity(damage))
            {
                return false;
            }

            float oldHealth = Health;

            if (!triggerEventsOnly)
            {
                Health -= damage.DamageAmount;

                if (Health != oldHealth)
                {
                    OnHit?.Invoke(damage);
                }

                if (Health <= 0)
                {
                    OnDeath?.Invoke();

                    if (debugText)
                    {
                        Debug.LogWarning(String.Format("Game object {0} destroyed.", gameObject.name));
                    }
                }

                if (damage.SetInvulnerability)
                {
                    SetInvincible(invincibilityTime);
                }
            }
            else
            {
                OnHit?.Invoke(damage);
            }

            if (debugText)
            {
                Debug.LogWarning(String.Format("Damaging game object: {0} for {1} health.", gameObject.name, damage.DamageAmount));
            }

            return true;
        }

        public void SetInvincible(float invincibilityTime)
        {
            Invincible = true;

            this.AttachTimer(invincibilityTime, this.DisableInvincibility);
            OnInvincibleStart?.Invoke();
        }

        public bool CheckDamageTypeImmunity(Damage damage)
        {
            if (damageImmunities.Count <= 0)
            {
                return false;
            }

            if (damageImmunities.TryGetValue(damage.DamageType, out bool immune))
            {
                return immune;
            }
            else
            {
                Debug.LogError("Damage immunity entry does not exist.");
            }

            return true;
        }

        void DisableInvincibility()
        {
            Invincible = false;
            OnInvincibleEnd?.Invoke();
        }

        void Start()
        {
            Health = MaxHealth;
        }
    }
}
