using UnityEngine;

public abstract class WeaponSO : _ItemSO
{
    public int damage;

    public Vector3 holdingPosition;
    public Quaternion holdingRotation;

    public Vector3 hiddenPosition;
    public Quaternion hiddenRotation;
}
