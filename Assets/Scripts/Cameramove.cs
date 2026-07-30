using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramove : MonoBehaviour
{
    GameObject mainCamera;

    bool isMoving_horizontal = false;
    bool isMoving_vertical = false;
    int moveDirection_horizontal = 0; // 1 for right, -1 for left
    int moveDirection_vertical = 0; // 1 for up, -1 for down
    Vector3 cameraPos;
    // Start is called before the first frame update

    public static Cameramove instance;

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
    }

    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    public void MoveCamera_horizon(float player_x)
    {
        if(isMoving_horizontal || isMoving_vertical) return; // Prevent starting a new move while already moving
        isMoving_horizontal = true;
        cameraPos = Camera.main.transform.position;
        Debug.Log("MoveCamera_horizon called with player_x: " + player_x);
        if(player_x > mainCamera.transform.position.x)
        {
            moveDirection_horizontal = 1;
        }
        else if(player_x < mainCamera.transform.position.x)
        {
            moveDirection_horizontal = -1;
        }
    }

        public void MoveCamera_vertical(float player_y)
    {
        if(isMoving_vertical || isMoving_horizontal) return; // Prevent starting a new move while already moving
        isMoving_vertical = true;
        cameraPos = Camera.main.transform.position;
        Debug.Log("MoveCamera_vertical called with player_y: " + player_y);
        if(player_y > mainCamera.transform.position.y)
        {
            moveDirection_vertical = 1;
        }
        else if(player_y < mainCamera.transform.position.y)
        {
            moveDirection_vertical = -1;
        }
    }




    void Update()
    {
        if(isMoving_horizontal)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(mainCamera.transform.position.x + (17 * moveDirection_horizontal), mainCamera.transform.position.y, mainCamera.transform.position.z), 0.001f);
            if(Mathf.Abs(mainCamera.transform.position.x - (cameraPos.x + (17 * moveDirection_horizontal))) < 0.01f)
            {
                mainCamera.transform.position = new Vector3(cameraPos.x + (17 * moveDirection_horizontal), mainCamera.transform.position.y, mainCamera.transform.position.z);
                isMoving_horizontal = false;
            }
        }
        if (isMoving_vertical)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + (10 * moveDirection_vertical), mainCamera.transform.position.z), 0.001f);
            if(Mathf.Abs(mainCamera.transform.position.y - (cameraPos.y + (10 * moveDirection_vertical))) < 0.01f)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraPos.y + (10 * moveDirection_vertical), mainCamera.transform.position.z);
                isMoving_vertical = false;
            }
        }

    }

}
