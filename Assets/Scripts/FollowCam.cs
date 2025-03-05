using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float SmoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    private float CamFloorHeight = 0;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, Mathf.Max(CamFloorHeight, target.position.y), transform.position.z);
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }
}
