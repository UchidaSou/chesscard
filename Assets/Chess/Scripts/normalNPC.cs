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
    private bool flg = false;
    public override GameObject selectedChess()
    {
        flg = false;
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        bool check = checker.isCheck(this.getColor());
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
        bool[,] checkBoardArray = new bool[maxI,maxJ];
        System.Array.Copy(boardState.chessBoardArray,boardArray,boardArray.Length); 
        //まだ未完成
        System.Array.Copy(boardState.checkBoardArray,checkBoardArray,checkBoardArray.Length);
        this.ReturnMethod(myChesses,enemeyChesses,2,boardArray,max);
        if(check){
            if(checker.inFlg){
                Debug.Log("inFlg");
                switch(this.getColor()){
                    case "white":
                        return GameObject.Find("White King(Clone)");
                    case "black":
                        return GameObject.Find("Black King(Clone)");
                }
            }else{
                Debug.Log("not inFlg");
                GameObject king = new GameObject();
                GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
                switch(this.getColor()){
                    case "white":
                        king = GameObject.Find("White King(Clone)");
                        break;
                    case "black":
                        king = GameObject.Find("Black King(Clone)");
                        break;
                }
                Vector3 vector = king.transform.position + new Vector3(-16,0,16);
                int i = (int)-vector.x / 4;
                int j = (int)vector.z / 4;
                int cell = i*8+j;
                List<Vector3> canMoveList = king.GetComponent<King>().canMovePosition(cell);
                if(canMoveList.Count != 0){
                    Debug.Log("king can move");
                    this.selectedObject = king;
                }else{
                    Debug.Log("king can not move");
                    GameObject checkObject = checker.checkObject;
                    Vector3 checkObjectPosition = checkObject.transform.position + new Vector3(-16,0,16);
                    int ci = (int)-checkObjectPosition.x / 4;
                    int cj = (int)checkObjectPosition.z / 4;
                    int cc = ci*8+cj;
                    int coI = 0,coJ = 0,ccC = 0,ccmI=0,ccmJ=0,ccmC=0;
                    foreach(GameObject co in chesses){
                        Vector3 coVec = co.transform.position + new Vector3(-16,0,16);
                        coI = (int)-coVec.x / 4;
                        coJ = (int)coVec.z / 4;
                        ccC = coI*8+coJ;
                        List<Vector3> vectors = co.GetComponent<Chess>().canMovePosition(ccC);
                        if(vectors.Count == 0){
                            continue;
                        }
                        foreach(Vector3 ccmVec in vectors){
                            ccmI = (int)-(ccmVec.x - 16) / 4;
                            ccmJ = (int)(ccmVec.z + 16) / 4;
                            ccmC = ccmI * 8 + ccmJ;
                            if(ccmC == ccC){
                                Debug.Log(co.name + " i:" + coI + " j:" + coJ);
                                flg = true;
                                this.selectedObject = co;
                            }
                        }
                    }
                }
            }
        }
        return this.selectedObject;
    }

    public override Vector3 selectedMovePosition()
    {
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        bool check = checker.isCheck(this.getColor());
        Debug.Log("NPC:"+check + " flg:"+this.flg);
        if(check){
            if(this.flg){
                return checker.checkObject.transform.position;
            }else{
                GameObject king = new GameObject();
                switch(this.getColor()){
                    case "white":
                        king = GameObject.Find("White King(Clone)");
                        break;
                    case "black":
                        king = GameObject.Find("Black King(Clone)");
                        break;
                }
                Chess chess = king.GetComponent<Chess>();
                Vector3 vector = king.transform.position + new Vector3(-16,0,16);
                int i = (int)-vector.x / 4;
                int j = (int)vector.z / 4;
                List<Vector3> vectors = chess.canMovePosition(i*8+j);
                int r = Random.Range(0,vectors.Count);
                if(vectors.Count != 0){
                    this.selectedPosition = vectors[r];
                }
            }
        }
        return this.selectedPosition;
    }

    private bool ReturnMethod(List<GameObject> mylist,List<GameObject> enemeyList,int count,GameObject[,] board,int max){
        int score,myScore=0,enemeyScore=0;
        int objectI=0,objectJ=0,vi=0,vj=0,index=0,I=0,J=0;
        List<Vector3> vectors;
        GameObject enemey;
        bool flg = false;
        int size = 5;
        if(mylist.Count <= 4){
            size = mylist.Count;
        }
        GameObject[] objects = new GameObject[size];
        for(int i=0;i<objects.Length;i++){
            objects[i] = mylist[UnityEngine.Random.Range(0,mylist.Count)];
        }
        List<GameObject> mylist2,enemeyList2;
        for(int i=0;i<objects.Length;i++){
            objectI = (int)-(objects[i].transform.position.x - 16) / 4;
            objectJ = (int)(objects[i].transform.position.z + 16) / 4;
            vectors = objects[i].GetComponent<Chess>().canMovePosition(objectI*8+objectJ);
            foreach(Vector3 vector in vectors){
                mylist2 = mylist;
                enemeyList2 = enemeyList;
                if(count == 2){
                    firstSelect = objects[i];
                    selectedPosition = vector;
                }
                vi = (int)-(vector.x - 16) / 4;
                vj = (int)(vector.z + 16) / 4;
                enemey = board[vi,vj];
                if(enemey != null && !enemey.tag.Equals(objects[i].tag)){
                    index = enemeyList2.IndexOf(enemey);
                    enemeyList2.RemoveAt(index);
                    board[vi,vj] = objects[i];
                }
                flg = false;
                if(count == 0){
                    myScore = 0;
                    enemeyScore = 0;
                    foreach(GameObject myObject in mylist2){
                        I = (int)-(myObject.transform.position.x - 16) / 4;
                        J = (int)(myObject.transform.position.z + 16) / 4;
                        myScore += myObject.GetComponent<Chess>().getScore(I,J);
                    }
                    foreach(GameObject enemeyObject in enemeyList2){
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
                    flg = ReturnMethod(enemeyList2,mylist2,count-1,board,max);
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
        this.setState(2);
        board = GameObject.Find("Board");
        this.boardState = board.GetComponent<BoardState>();
        this.card = this.gameObject.GetComponent<Card>();
    }  

    public override void UseCard()
    {
        int x = UnityEngine.Random.Range(0,20);
        //int x = 2;
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
