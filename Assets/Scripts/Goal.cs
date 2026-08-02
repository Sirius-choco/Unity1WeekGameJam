using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public GameObject ScoreCanvas; // Reference to the player GameObject
    public Text text;
    public static bool isGoal = false;
    // Start is called before the first frame update
    void Start()
    {
        ScoreCanvas.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Goal");
            //ここにゴールしたときの処理を書く
            ScoreCanvas.SetActive(true);
            text.text = "Time: " + Time.time.ToString("F2") + "s";
            isGoal = true;
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isGoal)
        {
            isGoal = false;
            Camera.main.transform.position = new Vector3(0, 0, -100);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Stage_choco");
        }
    }
}
