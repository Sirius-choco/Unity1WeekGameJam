using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    public GameObject seSlider;
    static public bool isSettingOpen = false;
    void Start()
    {
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !Goal.isGoal)
        {
            canvas.enabled = !canvas.enabled;
            isSettingOpen = !isSettingOpen;
            seSlider.SetActive(isSettingOpen);
        }
    }
}
