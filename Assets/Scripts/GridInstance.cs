using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstance : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Grid;
    void Start()
    {
        Instantiate(Grid, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
}
