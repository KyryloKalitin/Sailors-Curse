using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsService : IDamageable
{
    public event Action OnPlayerDied;

    public float HP { get; private set; }
    public List<TypedHit> TypedHitsList { get; private set; }    

    public PlayerStatsService()
    {
        HP = 100f;

        TypedHitsList = new List<TypedHit>();
    }
    public PlayerStatsService(GameProgressData.PlayerStatsData data)
    {
        HP = data.HP;
        TypedHitsList = data.typedHitsList;
    }

    public void ApplyDamage(float damage, DamageType damageType)
    {
        HP -= damage;

        if (HP <= 0)
        {
            OnPlayerDied?.Invoke();
            Debug.Log("Player died");
        }
        else
        {
            Debug.Log(HP);
        }

        if(damageType != DamageType.None)
            TypedHitsList.Add(new TypedHit(damage, damageType));
    }
}

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