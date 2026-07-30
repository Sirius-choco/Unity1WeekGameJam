using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentCamera : MonoBehaviour
{
    void Awake()
    {
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
