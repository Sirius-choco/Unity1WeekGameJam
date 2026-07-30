using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRemover : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject disappearEffectPrefab;

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
        Debug.Log("TileRemover: RemoveClickedTile called at cell position " + cellPosition);
        if (!tilemap.HasTile(cellPosition))
        {
            return;
        }

        Vector3 effectPosition =
            tilemap.GetCellCenterWorld(cellPosition);

        Instantiate(
            disappearEffectPrefab,
            effectPosition,
            Quaternion.identity
        );

        tilemap.SetTile(cellPosition, null);
    }
}