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

        // クリックしたタイルの中央座標
        Vector3 effectPosition =
            tilemap.GetCellCenterWorld(cellPosition);

        // タイルに対応するエフェクトを取得
        GameObject effectPrefab =
            GetEffectPrefab(clickedTile);

        // エフェクトを生成
        if (effectPrefab != null)
        {
            Instantiate(
                effectPrefab,
                effectPosition,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning(
                "対応するエフェクトPrefabが設定されていません。"
            );
        }

        // クリックしたタイルを削除
        tilemap.SetTile(cellPosition, null);
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