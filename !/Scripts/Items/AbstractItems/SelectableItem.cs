using UnityEngine;

[RequireComponent(typeof(Collider))]
[SelectionBase]
public abstract class SelectableItem : MonoBehaviour
{
    private Material _material;
    private const string _EMISSION_MULTIPLIER = "_EmissionMultiplier";

    protected virtual void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>() != null ? GetComponent<MeshRenderer>() 
                                                                         : GetComponentInChildren<MeshRenderer>();        

        if (meshRenderer != null)
            _material = meshRenderer.material;
        else
            Debug.LogError("Object has no material");
    }

    public void SetSelectionState(bool state)
    {
        if (state)
            _material.SetFloat(_EMISSION_MULTIPLIER , 0.8f);
        else
            _material.SetFloat(_EMISSION_MULTIPLIER , 0);
    }
}
