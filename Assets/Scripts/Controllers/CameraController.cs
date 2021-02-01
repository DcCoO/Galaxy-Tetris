using UnityEngine;
 
public class CameraController : MonoBehaviour, IReset
{
    [SerializeField] private float lookSpeedH;
    [SerializeField] private float lookSpeedV;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float dragSpeed;
     
    private float yaw = 0f;
    private float pitch = 0f;

    private Transform t;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        t = transform;
        originalPosition = t.position;
        originalRotation = t.rotation;
    }

    void Update ()
    {
        if (!GameController.instance.isPlaying) return;
        
        if (Input.GetMouseButton(1))
        {
            yaw += lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
            t.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
     
        if (Input.GetMouseButton(2))
        {
            t.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
        }
     
        t.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
    }

    public void Reset()
    {
        t.position = originalPosition;
        t.rotation = originalRotation;
    }
}