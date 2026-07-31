using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Controller");

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
