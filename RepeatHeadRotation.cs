using UnityEngine;

public class RepeatHeadRotation : MonoBehaviour
{
    [SerializeField]private Transform headTransform;

    private void LateUpdate()
    {
        transform.rotation = headTransform.localRotation;
    }
}
