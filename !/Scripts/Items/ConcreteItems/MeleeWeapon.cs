using System.Collections;
using UnityEngine;
using Zenject;

public class MeleeWeapon : Weapon
{
    [SerializeField] private MeleeWeaponSO meleeWeaponSO;
    public override _ItemSO ItemSO { get => meleeWeaponSO; }

    public override bool IsAttacking { get; protected set; } = false;
    public override bool IsWaitingCooldown { get; protected set; } = false;

    private float _animationDuration = 1f;
    private float _animationDelay = 0.3f;

    private PlayerAttackZoneTriggerHandler _attackZone;

    private void OnTriggerEnter(Collider other)
    {
        if (_attackZone != null)
            return;

        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _attackZone = playerController.AttackZone;
            _attackZone.gameObject.SetActive(true);
            _attackZone.OnDamageableDetected += _attackZone_OnDamageableDetected;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_attackZone == null)
            return;

        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            _attackZone.OnDamageableDetected -= _attackZone_OnDamageableDetected;
            _attackZone = null;
        }
    }

    public override void Attack()
    {
        StartCoroutine(SetAttackState());
    }

    private void _attackZone_OnDamageableDetected(IDamageable enemy)
    {
        if (!IsAttacking)
            return;

        StartCoroutine(DelayDamage(enemy));

        _attackZone.gameObject.SetActive(false);
    }

    private IEnumerator DelayDamage(IDamageable enemy)
    {
        yield return new WaitForSeconds(_animationDelay);

        enemy.ApplyDamage(meleeWeaponSO.damage, DamageType.None);
    }
    private IEnumerator SetAttackState()
    {
        IsAttacking = true;

        yield return new WaitForSeconds(_animationDuration);

        IsAttacking = false;

        StartCoroutine(CooldownWaiting());
    }
    private IEnumerator CooldownWaiting()
    {
        IsWaitingCooldown = true;

        yield return new WaitForSeconds(meleeWeaponSO.coolDawn);

        IsWaitingCooldown = false;

        if(_attackZone != null)
            _attackZone.gameObject.SetActive(true);
    }
}
