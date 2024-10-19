using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LevelManager LevelManager;

    [Space]
    [SerializeField] private float Zoom = 5f;

    private const float BaseZoomStep = 0.5f;
    private float ZoomStep;

    private const float MinZoom = 2f;
    private const float MaxZoom = 20f;

    [Space]
    [SerializeField] private Vector3 LastMousePosition = Vector3.zero;

    [SerializeField] private float BasePanSensitivity = 2f;
    [SerializeField] private float PanSensitivity;

    [Space]
    private Camera Camera;

    

    void Start()
    {
        Camera = Camera.main;

        Zoom = 5f;

        ZoomStep = BaseZoomStep + (Zoom * 0.05f);

        MoveCameraToCentre();
    }

    void Update()
    {
        ZoomStep = BaseZoomStep + (Zoom * 0.05f);

        PanSensitivity = Zoom * 0.45f;

        ZoomCamera();

        PanCamera();
    }

    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y > 0f)
        {
            if (Zoom - ZoomStep >= MinZoom)
            {
                Zoom -= ZoomStep;
            }

            else if (Zoom - ZoomStep <= MinZoom)
            {
                Zoom = MinZoom;
            }
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            if (Zoom + ZoomStep <= MaxZoom)
            {
                Zoom += ZoomStep;
            }

            else if (Zoom + ZoomStep >= MaxZoom)
            {
                Zoom = MaxZoom;
            }
        }

        Camera.orthographicSize = Zoom;
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            LastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 _delta = Input.mousePosition - LastMousePosition;

            Camera.transform.position -= new Vector3(_delta.x, _delta.y, 0f) * PanSensitivity * Time.deltaTime;

            LastMousePosition = Input.mousePosition;
        }

        ClampCameraPosition();
    }

    private void ClampCameraPosition()
    {
        Vector3Int _minBounds = new Vector3Int(LevelManager.LevelBounds.x, LevelManager.LevelBounds.y, (int) Camera.transform.position.z);

        Vector3Int _maxBounds = new Vector3Int(LevelManager.LevelBounds.xMax, LevelManager.LevelBounds.yMax, (int)Camera.transform.position.z);

        float _clampedX = Mathf.Clamp(Camera.transform.position.x, _minBounds.x, _maxBounds.x);

        float _clampedY = Mathf.Clamp(Camera.transform.position.y, _minBounds.y, _maxBounds.y);

        Camera.transform.position = new Vector3(_clampedX, _clampedY, Camera.transform.position.z);
    }

    private void MoveCameraToCentre() 
    {
        Vector3 _centreOfLevel = new Vector3( LevelManager.LevelBounds.center.x, LevelManager.LevelBounds.center.y, -1f);

        Camera.transform.position = _centreOfLevel;
    }
}
