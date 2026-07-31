using UnityEngine;

public class HarricaneSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private Renderer tornadoRenderer;

    private void Awake()
    {
        tornadoRenderer = GetComponent<Renderer>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void Update()
    {
        if (tornadoRenderer == null || audioSource == null)
        {
            return;
        }

        // カメラに映っている間だけ再生
        if (tornadoRenderer.isVisible)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}