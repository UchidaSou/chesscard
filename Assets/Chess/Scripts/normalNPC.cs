using System;
using System.Collections.Generic;
using System.Linq;
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
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        bool check = checker.isCheck(this.gameObject.GetComponent<Player>().getColor());
        Debug.Log("NPC:"+check);
        if(check && checker.checkCount >= 1 && checker.inFlg){
            return checker.checkObject.transform.position;
        }
        return this.selectedPosition;
    }

    private bool ReturnMethod(List<GameObject> mylist,List<GameObject> enemeyList,int count,GameObject[,] board,int max){
        int score,myScore=0,enemeyScore=0;
        int objectI=0,objectJ=0,vi=0,vj=0,index=0,I=0,J=0;
        List<Vector3> vectors;
        GameObject enemey;
        bool flg = false;
        GameObject[] objects = new GameObject[5];
        for(int i=0;i<5;i++){
            objects[i] = mylist[UnityEngine.Random.Range(0,mylist.Count)];
        }
        for(int i=0;i<5;i++){
            objectI = (int)-(objects[i].transform.position.x - 16) / 4;
            objectJ = (int)(objects[i].transform.position.z + 16) / 4;
            vectors = objects[i].GetComponent<Chess>().canMovePosition(objectI*8+objectJ);
            foreach(Vector3 vector in vectors){
                if(count == 2){
                    firstSelect = objects[i];
                    selectedPosition = vector;
                }
                vi = (int)-(vector.x - 16) / 4;
                vj = (int)(vector.z + 16) / 4;
                enemey = board[vi,vj];
                if(enemey != null && !enemey.tag.Equals(objects[i].tag)){
                    index = enemeyList.IndexOf(enemey);
                    enemeyList.RemoveAt(index);
                    board[vi,vj] = objects[i];
                }
                flg = false;
                if(count == 0){
                    myScore = 0;
                    enemeyScore = 0;
                    foreach(GameObject myObject in mylist){
                        I = (int)-(myObject.transform.position.x - 16) / 4;
                        J = (int)(myObject.transform.position.z + 16) / 4;
                        myScore += myObject.GetComponent<Chess>().getScore(I,J);
                    }
                    foreach(GameObject enemeyObject in enemeyList){
                        I = (int)-(enemeyObject.transform.position.x - 16) / 4;
                        J = (int)(enemeyObject.transform.position.z + 16) / 4;
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

    public override void UseCard()
    {
        int x = UnityEngine.Random.Range(0,20);
        switch(x){
            case 0:
             GameObject.Find("Game").GetComponent<Game>().Resurrection();
             break;
            case 1:
             GameObject.Find("Game").GetComponent<Game>().TurnReverse();
             break;
            case 2:
             GameObject.Find("Game").GetComponent<Game>().setMine();
             break;
             case 3:
             GameObject.Find("Game").GetComponent<Game>().twiceMove();
             break;
            case 4:
             GameObject.Find("Game").GetComponent<Game>().canntMove();
             break;
            case 5:
             GameObject.Find("Game").GetComponent<Game>().notUseCard();
             break;
            default:
             break;
        }
    }
}
