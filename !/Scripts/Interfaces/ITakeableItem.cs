
using UnityEngine;

public interface ITakeableItem
{
    public abstract void Interact(PlayerInventoryService player, Collider playerCollider);
}
