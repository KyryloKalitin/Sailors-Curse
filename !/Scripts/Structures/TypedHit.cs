using System;

[Serializable]
public struct TypedHit
{
    public float damage;
    public DamageType damageType;

    public TypedHit(float damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
    }
}