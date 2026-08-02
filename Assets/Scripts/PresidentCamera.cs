using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentCamera : MonoBehaviour
{
    public static float time;
    public static float tmptime = 0;
    public static bool isloop = false;
    void Awake()
    {
        if(isloop && !Reset.isResetting)
        {
            time = Time.time;
        }
        else
        {
            time = tmptime;
            Reset.isResetting = false; // Reset the flag after the scene is loaded
        }
        // "MainCamera"タグを持つオブジェクトを検索
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");

        // 既にカメラが存在している場合は、このオブジェクト（新しく読み込まれたシーンのカメラ）を削除
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // 存在しない場合は、シーン遷移で破棄されないようにする
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
