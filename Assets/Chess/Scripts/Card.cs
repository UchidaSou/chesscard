using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool resurrection = true;
    public bool turnreverse = true;
    public bool setmine = true;
    public bool twicemove = true;
    public bool canntmove = true;

    public GameObject mine;
    public GameObject cantMoveEffect;
    public GameObject board;

    public void Resurrection(string color){
        List<GameObject> retiredList;
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            retiredList = boardState.whiteRetired;
        }else{
            retiredList = boardState.blackRetired;
        }
        int size = retiredList.Count;
        if(size == 0){
            return;
        }
        int r = Random.Range(0,size);
        GameObject resurrectionObject = retiredList[r];
        resurrectionObject.tag = color;
        Chess chess = resurrectionObject.GetComponent<Chess>();
        resurrectionObject.transform.position = chess.getFirstVector();
        retiredList.RemoveAt(r);
        Vector3 vector = chess.getFirstVector() + new Vector3(-16,0,16);
        int i = (int)-vector.x/4;
        int j  = (int)vector.z/4;
        boardState.chessBoardArray[i,j] = resurrectionObject;
        this.resurrection = false;
    }

    public void turnReverse(GameObject beforeMoveObject){
        Debug.Log("Reverse");
        int i = (int) -(beforeMoveObject.transform.position.x - 16) / 4;
        int j = (int) (beforeMoveObject.transform.position.z + 16) /4;
        BoardState boardState = board.GetComponent<BoardState>();
        boardState.chessBoardArray[i,j] = null;
        Chess chess = beforeMoveObject.GetComponent<Chess>();
        Vector3 vector = chess.getBeforeVector();
        beforeMoveObject.transform.position = vector;
        i = (int) -(vector.x - 16) / 4;
        j = (int) (vector.z + 16) / 4;
        boardState.chessBoardArray[i,j] = beforeMoveObject;
        this.turnreverse = false;
    }

    public void setMine(string color){
        int cellNumber = Random.Range(2*8,5*8+7);
        Vector3 vector = ChessUiEngine.ToWorldPoint(cellNumber);
        Debug.Log("setMine " + cellNumber);
        GameObject setmine = GameObject.Instantiate(mine,vector,Quaternion.Euler(0,0,0));
        setmine.GetComponent<Mine>().color = color;
        this.setmine = false;
    }

    public void twiceMove(string color){
        List<GameObject> objects = new List<GameObject>();
        GameObject[] cheesses = GameObject.FindGameObjectsWithTag(color);
        foreach(GameObject chess in cheesses){
            if(chess.name.Contains("Pawn") || chess.name.Contains("King")){
                objects.Add(chess);
            }
        }
        int r = Random.Range(0,objects.Count);
        objects[r].GetComponent<Chess>().setMove(2);
        Debug.Log("Twice " + objects[r].name);
        this.twicemove = false;
        }


    public GameObject canntMove(string color){
        GameObject[] objects;
        if(color.Equals("white")){
           objects = GameObject.FindGameObjectsWithTag("black");
        }else{
            objects = GameObject.FindGameObjectsWithTag("white");
        }
         
        int r = Random.Range(0,objects.Length);
        objects[r].GetComponent<Chess>().canMove = false;
        Debug.Log("cantMove " + objects[r].name);
        cantMoveEffect = Instantiate(cantMoveEffect,objects[r].transform.position,Quaternion.Euler(0,0,0));
        return objects[r];
    }

    public void nullBtn(){

    }

    void Start(){
        this.board = GameObject.Find("Board");
    }
}
