using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRemover : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Camera mainCamera;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RemoveClickedTile();
        }
    }

    private void RemoveClickedTile()
    {
        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int cellPosition =
            tilemap.WorldToCell(mouseWorldPosition);

        if (tilemap.HasTile(cellPosition))
        {
            tilemap.SetTile(cellPosition, null);
        }
    }
}