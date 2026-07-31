using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MoveBlock;
    private Ray ray;
    private RaycastHit2D hit;

    [Header("Sound")]
    [SerializeField]
    private AudioClip blockGenerateSound;

    void BlockGenerate()
    {
        if (SoundManager.instance != null && blockGenerateSound != null)
            SoundManager.instance.PlaySE(blockGenerateSound);

        Instantiate(MoveBlock, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                BlockGenerate();
            }

        }
    }
}
