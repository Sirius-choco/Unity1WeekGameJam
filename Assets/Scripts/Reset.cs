using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    [SerializeField] GameObject player; // Reference to the player GameObject
    [SerializeField] GameObject Grid;
    public PlayerController playerController; // Reference to the PlayerController script
    
    public static Vector3 startpos;

    void Start()
    {
        Instance();
        DontDestroyOnLoad(Camera.main.gameObject);
    }

    void Instance()
    {
        startpos = new Vector3(Camera.main.transform.position.x-5f, Camera.main.transform.position.y-3f, 0f);
        Instantiate(player, startpos, Quaternion.identity); // Instantiate the player at the start position
        Instantiate(Grid, Vector3.zero, Quaternion.identity);
    }
    // Start is called before the first frame updat
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && !Cameramove.instance.isMoving_horizontal && !Cameramove.instance.isMoving_vertical && !(Setting.isSettingOpen || Goal.isGoal))
        {
            startpos = new Vector3(Camera.main.transform.position.x-5f, Camera.main.transform.position.y-3f, 0f); // Reset player position to start position
            // Reset the scene or perform any other reset logic here
            UnityEngine.SceneManagement.SceneManager.LoadScene("Stage_choco"); // Replace \"GameScene\" with the name of your scene
        }
    }
}
