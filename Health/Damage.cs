using System;

/// <summary>
/// Basic Damage information struct used by the Health Component to simply define how damage is applied to health components.
/// DAMAGE_TYPES can be modified to change the types of damage available. 
/// </summary>
/// 

namespace Noba.Health
{
    public enum DAMAGE_TYPES
    {
        BITE,
        BULLET,
        DEFAULT
    }

    [Serializable]
    public struct Damage
    {
        public float DamageAmount;
        public bool SetInvulnerability;
        public DAMAGE_TYPES DamageType;
        public Entity Instigator;
        public bool IgnoreCurrentInvulnerabilityState;

        public Damage(float newDamage, bool setIframes = true, DAMAGE_TYPES newType = DAMAGE_TYPES.DEFAULT, Entity attacker = null, bool ignoreIframes = false)
        {
            DamageAmount = newDamage;
            SetInvulnerability = setIframes;
            DamageType = newType;
            Instigator = attacker;
            IgnoreCurrentInvulnerabilityState = ignoreIframes;
        }

        public Damage(Damage copyDamage)
        {
            DamageAmount = copyDamage.DamageAmount;
            SetInvulnerability = copyDamage.SetInvulnerability;
            DamageType = copyDamage.DamageType;
            Instigator = copyDamage.Instigator;
            IgnoreCurrentInvulnerabilityState = copyDamage.IgnoreCurrentInvulnerabilityState;
        }
    }
}
