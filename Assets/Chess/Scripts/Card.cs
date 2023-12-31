using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool resurrection = true;
    public bool turnreverse = true;
    public bool setmine = true;
    public bool twicemove = true;
    public bool canntmove = true;

    public GameObject mine;

    public void Resurrection(string color){
        GameObject retired = GameObject.Find(color+"Retired");
        int current = retired.transform.childCount;
        int r = Random.Range(0,current);
        Debug.Log(r);
        Transform currentTransform = retired.transform.GetChild(r);
        GameObject currentObject = currentTransform.gameObject;
        Chess chess = currentObject.GetComponent<Chess>();
        currentObject.transform.position = chess.getFirstVector();
        currentObject.tag = color;
        Debug.Log("resurrection " + currentObject.name);
        this.resurrection = false;
        currentObject.transform.parent = null;
    }

    public void turnReverse(GameObject beforeMoveObject){
        Debug.Log("Reverse");
        Chess chess = beforeMoveObject.GetComponent<Chess>();
        Vector3 vector = chess.getBeforeVector();
        beforeMoveObject.transform.position = vector;
        this.turnreverse = false;
    }

    public void setMine(string color){
        int cellNumber = Random.Range(14,41);
        Vector3 vector = ChessUiEngine.ToWorldPoint(cellNumber);
        Debug.Log("setMine " + cellNumber);
        Instantiate(mine,vector,Quaternion.Euler(0,0,0));
        mine.tag = color;
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
        return objects[r];
    }

    public void nullBtn(){

    }
}
