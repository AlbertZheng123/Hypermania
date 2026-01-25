using UnityEngine;

public class DJ_CameraControl : MonoBehaviour
{
    private GameObject MainCamera;
    private Camera Camera;
    private bool Zoom = false;

    [SerializeField]
    private float ZoomOffset = 0.5f;

    [SerializeField]
    private float CameraOffset = 0.5f;
    private Vector3 center = new Vector3(0, 0, 0);

    void Start()
    {
        MainCamera = gameObject;
        Camera = MainCamera.GetComponent<Camera>();
    }

    void Update()
    {
        // Replace with hitstop call later on
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleZoom();
        }
    }

    void ToggleZoom()
    {
        if (Zoom == false)
        {
            Camera.orthographicSize -= ZoomOffset;
            MainCamera.transform.position -= new Vector3(0, CameraOffset, 0);
            Zoom = true;
        }
        else
        {
            Camera.orthographicSize += ZoomOffset;
            MainCamera.transform.position += new Vector3(0, CameraOffset, 0);
            Zoom = false;
        }
    }
}
