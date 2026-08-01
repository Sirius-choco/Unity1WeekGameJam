using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanjiManager : MonoBehaviour
{
    public GameObject Kanji;

    private Ray ray;
    private RaycastHit2D hit;
    // Start is called before the first frame update

    [SerializeField]
    private AudioClip kanjiClickSound;

    void ClickKanji()
    {
        // 漢字をクリックしたときの効果音
        if (SoundManager.instance != null &&kanjiClickSound != null)
            SoundManager.instance.PlaySE(kanjiClickSound);
       
        Destroy(this.gameObject);
        TileRemover.instance.MeltIce();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                ClickKanji();
            }

        }
        // Optional: Add any logic you want to execute every frame
    }
}
