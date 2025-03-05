using UnityEngine;

public class BGControllerScript : MonoBehaviour
{
    [SerializeField] private GameObject Cam;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y, -1);
    }
}
