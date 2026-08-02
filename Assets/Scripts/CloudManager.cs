using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject harricanePrefab; // Assign the cloud prefab in the Inspector

    public GameObject whiteCloudPrefab; // Assign the cloud prefab in the Inspector

    [Header("Sound")]
    [SerializeField]
    private AudioClip cloudClickSound; // Assign the sound clip in the Inspector

    private Vector3 cloudPosition;

    private Ray ray;
    private RaycastHit2D hit;
    public static CloudManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        cloudPosition = this.transform.position;
        GameObject harricane = Instantiate(harricanePrefab, this.transform.position, Quaternion.identity);
        harricane.transform.parent = this.transform; // Set the cloud as a child of the CloudManager
        harricane.transform.position = cloudPosition + new Vector3(0, -4, 0);
    }

    public void ClickCloud()
    {
        if (SoundManager.instance != null && cloudClickSound != null)
            SoundManager.instance.PlaySE(cloudClickSound);
        Instantiate(whiteCloudPrefab, cloudPosition, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Setting.isSettingOpen)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                ClickCloud();
            }

        }
        // Optional: Add any logic you want to execute every frame
    }
}
