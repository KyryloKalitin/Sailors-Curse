public interface IDamageable
{
    public void ApplyDamage(float damage, DamageType damageType);
    public float HP { get; }
}

