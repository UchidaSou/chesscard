using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    public GameObject[,] chessBoardArray;
    public List<GameObject> blackRetired;
    public List<GameObject> whiteRetired;
    // Start is called before the first frame update
    void Start()
    {
        chessBoardArray = new GameObject[8,8];
        blackRetired = new List<GameObject>();
        whiteRetired = new List<GameObject>();
    }

}
