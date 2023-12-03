using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    public GameObject[,] chessBoardArray;
    public bool[,] checkBoardArray;
    public List<GameObject> blackRetired;
    public List<GameObject> whiteRetired;
    public float[,] imbalance;
    // Start is called before the first frame update
    void Start()
    {
        int mode = PlayerPrefs.GetInt("Mode",0);
        if(mode == 0){
            checkBoardArray = new bool[8,8];
            chessBoardArray = new GameObject[8,8];
            imbalance = new float[8,8];
            for(int i=0;i<8;i++){
                for(int j=0;j<8;j++){
                    checkBoardArray[i,j] = false;
                    imbalance[i,j] = Random.Range(0f,2.0f);
                }
            }
        }else{
            chessBoardArray = new GameObject[6,5];
            imbalance = new float[6,5];
            for(int i=0;i<6;i++){
                for(int j=0;j<5;j++){
                    checkBoardArray[i,j] = false;
                    imbalance[i,j] = Random.Range(0f,2.0f);
                }
            }
        }
        blackRetired = new List<GameObject>();
        whiteRetired = new List<GameObject>();
    }
}
