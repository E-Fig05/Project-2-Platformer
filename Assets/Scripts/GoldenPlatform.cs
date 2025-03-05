using System.Net;
using UnityEngine;

public class GoldenPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject self;

    [SerializeField]
    private Vector3 FinishPosition = Vector3.zero;
    [SerializeField]
    private float speed = 0.5f;

    private Vector3 StartPosition;
    [SerializeField]
    private float trackPercent = 0;
    private int direction = 1;

    void Start()
    {
        StartPosition = transform.position;
        enabled = false;
        self.SetActive(false);
    }


    void Update()
    {
        trackPercent += direction * speed * Time.deltaTime;

        float x = (FinishPosition.x - StartPosition.x) * trackPercent + StartPosition.x;
        float y = (FinishPosition.y - StartPosition.y) * trackPercent + StartPosition.y;
        
        transform.position = new Vector3 (x, y, StartPosition.z);

        if(direction == 1 && trackPercent > 0.9f || direction == -1 && trackPercent < 0.1f)
        {
            speed = 0;
            enabled = false;
            //direction *= -1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (FinishPosition != null)
        {
            
            Gizmos.DrawLine(transform.position, FinishPosition);

            /*
            Gizmos.DrawWireSphere(endPoint.position, circleRadius);*/
        }
    }
}
