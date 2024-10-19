using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables
    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private UIManager UIManager;

    #region Tilemaps
    [Space]
    [SerializeField] private Tilemap MainTileMap;
    #endregion

    #endregion

    void Update()
    {
        LeftClickCheck();

        RightClickCheck();

        EscapeClickedCheck();
    }

    private void LeftClickCheck()
    {
        if (Input.GetMouseButtonUp(0) && GameManager.CurrentGameState == GameState.Playing)
        {
            Vector3Int _mapPosition = GetHoveredTile();

            if (CheckTileIsValid(_mapPosition))
            {
                LevelManager.OnTileM1Click(_mapPosition);
            }
        }
    }

    private void RightClickCheck()
    {
        if (Input.GetMouseButtonUp(1) && GameManager.CurrentGameState == GameState.Playing)
        {
            Vector3Int _mapPosition = GetHoveredTile();

            if (CheckTileIsValid(_mapPosition))
            {
                LevelManager.OnTileM2Click(_mapPosition);
            }
        }
    }

    private void EscapeClickedCheck()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && GameManager.OnGoingGame && UIManager.MenuWindow.activeSelf == false)
        {
            if (UIManager.MenuWindow.activeSelf == false)
            {
                UIManager.EnableMenuWindow(true);
            }

            else
            {
                UIManager.EnableMenuWindow(false);
            } 
        }
    }

    private Vector3Int GetHoveredTile()
    {
        var _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int _mapPosition = MainTileMap.WorldToCell(_mousePosition);

        return _mapPosition;
    }

    private bool CheckTileIsValid(Vector3Int mapPosition)
    {
        return LevelManager.GridData.ContainsKey(mapPosition);
    }
}
