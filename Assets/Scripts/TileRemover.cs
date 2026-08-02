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

    //効果音関連
    [SerializeField]
    private AudioClip normalRemovalSound;

    [SerializeField]
    private AudioClip bombExplosionSound;
    [SerializeField]
    private AudioClip iceMeltSound;
    [SerializeField]
    private AudioClip iceReflectionSound;
    [SerializeField]
    private AudioClip reflectionSound;

    public Sprite[] sprites;

    public Sprite Melt;
    private bool isRemoving = false;
    public static TileRemover instance;

    private bool BomeExploded = false;

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
        if (Input.GetMouseButtonDown(0) && !Setting.isSettingOpen)
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
                    if(iceEffectPrefab!= null)
                    {
                        Instantiate(iceEffectPrefab, 
                            tilemap.GetCellCenterWorld(new Vector3Int(cellPosition.x + i - 8, cellPosition.y + j - 5, 0)),
                            Quaternion.identity);

                    }
                    PlayRemovalSound(iceTile);
                    tilemap.SetTile(new Vector3Int(cellPosition.x + i - 8, cellPosition.y + j -5, 0), null);

                } 
            }
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

        // 氷ブロックは消さず、反射音だけ鳴らす
        if (clickedTile == iceTile)
        {
            if (SoundManager.instance != null &&
                iceReflectionSound != null)
            {
                SoundManager.instance.PlaySE(iceReflectionSound);
            }

            return;
        }

        // 爆弾なら周囲8マスも消す
        if (clickedTile == bombTile)
        {
            ExplodeBomb(cellPosition);
            return;
        }

        // 消去不能Tile、またはspritesに入っていないSprite
        if (!IsRemovable(clickedTile) ||
            !IsRemovableSprite(cellPosition))
        {
            PlayReflectionSound();
            return;
        }

        

        // 通常タイルを1個だけ消す
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
        PlayRemovalSound(bombTile);
        tilemap.SetTile(bombPosition, null);
        BomeExploded = true;
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
        BomeExploded = false;
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
        Debug.Log(tilemap.GetSprite(cellPosition));
        foreach (var sprite in sprites)
        {
            if (tilemap.GetSprite(cellPosition) == sprite || BomeExploded)
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
        
        PlayRemovalSound(targetTile);
        tilemap.SetTile(cellPosition, null);
        isRemoving = false; // Reset the flag for the next tile removal
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

    private void PlayRemovalSound(TileBase targetTile)
    {
        AudioClip sound = normalRemovalSound;

        if (targetTile == bombTile)
        {
            sound = bombExplosionSound;
        }
        else if (targetTile == iceTile)
        {
            sound = iceMeltSound;
        }

        if (SoundManager.instance != null && sound != null)
        {
            SoundManager.instance.PlaySE(sound);
        }
    }

    private bool IsRemovableSprite(Vector3Int cellPosition)
    {
        Sprite targetSprite = tilemap.GetSprite(cellPosition);

        if (targetSprite == null ||
            sprites == null ||
            sprites.Length == 0)
        {
            return false;
        }

        foreach (Sprite removableSprite in sprites)
        {
            if (targetSprite == removableSprite)
            {
                return true;
            }
        }

        return false;
    }

    private void PlayReflectionSound()
    {
        if (SoundManager.instance != null &&
            reflectionSound != null)
        {
            SoundManager.instance.PlaySE(reflectionSound);
        }
    }
}