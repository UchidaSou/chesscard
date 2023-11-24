using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class normalNPC : Player
{
    private Vector3 selectedPosition;
    public GameObject board;
    private BoardState boardState;
    private GameObject selectedObject;
    private GameObject firstSelect;
    public override GameObject selectedChess()
    {
        int mode = PlayerPrefs.GetInt("Mode",0);
        int maxI,maxJ;
        if(mode == 0){
            maxI = 8;
            maxJ = 8;
        }else{
            maxI = 6;
            maxJ = 5;
        }
        //int x = UnityEngine.Random.Range(0,100);
        List<GameObject> myChesses = GameObject.FindGameObjectsWithTag(this.getColor()).ToList();
        List<GameObject> enemeyChesses = new List<GameObject>();
        if(this.getColor().Equals("white")){
            enemeyChesses = GameObject.FindGameObjectsWithTag("black").ToList();
        }else{
            enemeyChesses = GameObject.FindGameObjectsWithTag("white").ToList();
        }
        int myScore=0,enemeyScore=0;
        foreach(GameObject  my in myChesses){
            myScore = myScore + my.GetComponent<Chess>().getMaterial();
        }
        foreach(GameObject enemey in enemeyChesses){
            enemeyScore = enemeyScore + enemey.GetComponent<Chess>().getMaterial();
        }
        int max = -1;
        this.selectedObject = null;
        GameObject[,] boardArray = new GameObject[maxI,maxJ];
        Array.Copy(boardState.chessBoardArray,boardArray,boardArray.Length); 
        this.ReturnMethod(myChesses,enemeyChesses,2,boardArray,max);
        return this.selectedObject;
    }

    public override Vector3 selectedMovePosition()
    {
        return this.selectedPosition;
    }

    private bool ReturnMethod(List<GameObject> mylist,List<GameObject> enemeyList,int count,GameObject[,] board,int max){
        int score;
        GameObject[] objects = new GameObject[5];
        for(int i=0;i<5;i++){
            objects[i] = mylist[UnityEngine.Random.Range(0,mylist.Count)];
        }
        for(int i=0;i<5;i++){
            int objectI = (int)-(objects[i].transform.position.x - 16) / 4;
            int objectJ = (int)(objects[i].transform.position.z + 16) / 4;
            List<Vector3> vectors = objects[i].GetComponent<Chess>().canMovePosition(objectI*8+objectJ);
            foreach(Vector3 vector in vectors){
                if(count == 2){
                    firstSelect = objects[i];
                    selectedPosition = vector;
                }
                int vi = (int)-(vector.x - 16) / 4;
                int vj = (int)(vector.z + 16) / 4;
                GameObject enemey = board[vi,vj];
                if(enemey != null && !enemey.tag.Equals(objects[i].tag)){
                    int index = enemeyList.IndexOf(enemey);
                    enemeyList.RemoveAt(index);
                    board[vi,vj] = objects[i];
                }
                bool flg = false;
                if(count == 0){
                    int myScore = 0,enemeyScore = 0;
                    foreach(GameObject myObject in mylist){
                        int I = (int)-(myObject.transform.position.x - 16) / 4;
                        int J = (int)(myObject.transform.position.z + 16) / 4;
                        myScore += myObject.GetComponent<Chess>().getScore(I,J);
                    }
                    foreach(GameObject enemeyObject in enemeyList){
                        int I = (int)-(enemeyObject.transform.position.x - 16) / 4;
                        int J = (int)(enemeyObject.transform.position.z + 16) / 4;
                        enemeyScore += enemeyObject.GetComponent<Chess>().getScore(I,J);
                    }
                    score = myScore - enemeyScore;
                    if(max<score){
                        max = score;
                        flg = true;
                    }
                }else{
                    flg = ReturnMethod(enemeyList,mylist,count-1,board,max);
                }
                if(flg){
                    selectedObject = firstSelect;
                }
            }
        }
        return true;
    }

    void Start()
    {
        this.setPlayerState(this.GetComponent<PlayerState>(),2);
        board = GameObject.Find("Board");
        this.boardState = board.GetComponent<BoardState>();
        this.card = this.gameObject.GetComponent<Card>();
    }  
}
