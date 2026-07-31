using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 1.2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}