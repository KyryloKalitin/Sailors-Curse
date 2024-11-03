using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
