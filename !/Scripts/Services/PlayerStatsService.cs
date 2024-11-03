using System;
using System.Collections.Generic;

public class PlayerStatsService : IDamageable
{
    public event Action OnPlayerDied;

    public float HP { get; private set; } = 100f;
    public List<TypedHit> TypedHitsList { get; private set; } = new();

    public PlayerStatsService()
    {

    }

    public void DeserializeFromData(PlayerStatsData data)
    {
        HP = data.HP;
        TypedHitsList = data.typedHitsList;
    }

    public void ApplyDamage(float damage, DamageType damageType)
    {
        if (HP - damage <= 0)
        {
            HP = 0;
            OnPlayerDied?.Invoke();
        }
        else
        {
            HP -= damage;
        }

        if (damageType != DamageType.None)
            TypedHitsList.Add(new TypedHit(damage, damageType));
    }
}
