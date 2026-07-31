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

    public Sprite[] sprites;

    public Sprite Melt;
    private bool isRemoving = false;
    public static TileRemover instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
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

    public void MeltIce()
    {
        Vector3Int cellPosition =
            tilemap.WorldToCell(mainCamera.transform.position);
        for(int i=0; i < 17; i++)
        {
            for(int j=0; j< 10; j++)
            {
                if(tilemap.GetSprite(new Vector3Int(cellPosition.x + i - 8, cellPosition.y + j -5, 0)) == Melt)
                {
                    tilemap.SetTile(new Vector3Int(cellPosition.x + i - 8, cellPosition.y + j -5, 0), null);
                } 
            }
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
        Debug.Log(tilemap.GetSprite(cellPosition));
        foreach (var sprite in sprites)
        {
            if (tilemap.GetSprite(cellPosition) == sprite)
            {
                isRemoving = true; // Do not remove the tile if it matches any of the specified sprites
            }
        }
        if (!isRemoving)
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
        isRemoving = false; // Reset the flag for the next tile removal
    }
}