using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LevelManager LevelManager;

    void Start()
    {
        MoveCameraToCentre();
    }

    private void MoveCameraToCentre() 
    {
        Vector3 _centreOfLevel = new Vector3( LevelManager.LevelBounds.center.x, LevelManager.LevelBounds.center.y, -1f);

        Camera.main.transform.transform.position = _centreOfLevel;
    }
}
