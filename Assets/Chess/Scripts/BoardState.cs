using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    public GameObject[,] chessBoardArray;
    public List<GameObject> blackRetired;
    public List<GameObject> whiteRetired;
    public float[,] imbalance;
    // Start is called before the first frame update
    void Start()
    {
        chessBoardArray = new GameObject[6,5];
        //chessBoardArray = new GameObject[8,8];
        blackRetired = new List<GameObject>();
        whiteRetired = new List<GameObject>();
        imbalance = new float[8,8];
        for(int i=0;i<8;i++){
            for(int j=0;j<8;j++){
                imbalance[i,j] = Random.Range(0f,2.0f);
            }
        }
    }

}
