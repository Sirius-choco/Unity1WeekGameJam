using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject keylockPrefab; // Assign the key prefab in the Inspector

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(keylockPrefab); // Destroy the key lock object when the player collides with it
            Destroy(this.gameObject); // Destroy the key lock object itself
        }
    }
}
