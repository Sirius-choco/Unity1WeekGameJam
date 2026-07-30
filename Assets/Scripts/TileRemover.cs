using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRemover : MonoBehaviour
{
    [Header("Tilemap / Camera")]
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Camera mainCamera;

    [Header("Special Tiles")]
    [SerializeField]
    private TileBase bombTile;

    [SerializeField]
    private TileBase iceTile;

    [Header("Cannot Be Removed")]
    [Tooltip("爆発でも消えないタイルを登録します")]
    [SerializeField]
    private TileBase[] unremovableTiles;

    [Header("Effect Prefabs")]
    [SerializeField]
    private GameObject normalEffectPrefab;

    [SerializeField]
    private GameObject bombEffectPrefab;

    [SerializeField]
    private GameObject iceEffectPrefab;

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
        if (tilemap == null || mainCamera == null)
        {
            Debug.LogError(
                "TilemapまたはMain Cameraが設定されていません。"
            );
            return;
        }

        // マウスの画面座標をゲーム内の座標へ変換
        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPosition.z = 0.0f;

        // ゲーム内座標をTilemapのマス座標へ変換
        Vector3Int cellPosition =
            tilemap.WorldToCell(mouseWorldPosition);

        TileBase clickedTile =
            tilemap.GetTile(cellPosition);

        // タイルがない場所なら何もしない
        if (clickedTile == null)
        {
            return;
        }
        // 爆弾なら周囲8マスも消す
        if (clickedTile == bombTile)
        {
            ExplodeBomb(cellPosition);
            return;
        }

        // 通常・氷タイルを1個だけ消す
        RemoveTile(cellPosition);

        
    }

    private void ExplodeBomb(Vector3Int bombPosition)
    {
        // 爆発エフェクトは中心に1回だけ表示
        Vector3 effectPosition =
            tilemap.GetCellCenterWorld(bombPosition);

        if (bombEffectPrefab != null)
        {
            Instantiate(
                bombEffectPrefab,
                effectPosition,
                Quaternion.identity
            );
        }

        // 爆弾本体を削除
        tilemap.SetTile(bombPosition, null);

        // 周囲8マスを調べる
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // 中央の爆弾マスは処理済み
                if (x == 0 && y == 0)
                {
                    continue;
                }

                Vector3Int targetPosition =
                    bombPosition + new Vector3Int(x, y, 0);

                RemoveTile(targetPosition);
            }
        }
    }

    private void RemoveTile(Vector3Int cellPosition)
    {
        TileBase targetTile =
            tilemap.GetTile(cellPosition);

        // タイルが存在しない
        if (targetTile == null)
        {
            return;
        }

        // 消せないタイルなら残す
        if (!IsRemovable(targetTile))
        {
            return;
        }

        Vector3 effectPosition =
            tilemap.GetCellCenterWorld(cellPosition);

        GameObject effectPrefab =
            GetEffectPrefab(targetTile);

        if (effectPrefab != null)
        {
            Instantiate(
                effectPrefab,
                effectPosition,
                Quaternion.identity
            );
        }

        tilemap.SetTile(cellPosition, null);
    }

    private bool IsRemovable(TileBase targetTile)
    {
        foreach (TileBase unremovableTile in unremovableTiles)
        {
            if (targetTile == unremovableTile)
            {
                return false;
            }
        }

        return true;
    }



    private GameObject GetEffectPrefab(TileBase clickedTile)
    {
        if (clickedTile == bombTile)
        {
            return bombEffectPrefab;
        }

        if (clickedTile == iceTile)
        {
            return iceEffectPrefab;
        }

        // 爆弾でも氷でもないタイル
        return normalEffectPrefab;
    }
}