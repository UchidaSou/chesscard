using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public int checkCount = 0;
    public GameObject checkObject = null;
    public bool inFlg = false;
    public GameObject board;
    public bool isCheckMate(string color){
        GameObject king;
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
        }else{
            king = GameObject.Find("Black King(Clone)");
        }
        if(king.gameObject.tag.Equals("Retired")){
            return true;
        }
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        King king1 = king.GetComponent<King>();
        int move = king1.getMove();
        int maxI = king1.getMaxI();
        int maxJ = king1.getMaxJ();
        BoardState boardState = board.GetComponent<BoardState>();
        int count = 0;
        int checkCount=0;
        for(int k=kingJ-move;k<=kingJ+move;k++){
            for(int l=kingI-move;l<=kingI+move;l++){
                if(k>=maxJ||k<0){
                    continue;
                }
                if(l>=maxI||l<0){
                    continue;
                }
                if(k==kingJ&&l==kingI){
                    continue;
                }
                count++;
                if(boardState.checkBoardArray[l,k]){
                    checkCount++;
                }
            }
        }
        if(count == checkCount){
            return true;
        }else{
            return false;
        }
    }

    public bool isCheck(string color){
        checkCount = 0;
        GameObject king;
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
        }else{
            king = GameObject.Find("Black King(Clone)");
        }
        if(king.tag.Equals("Retired")){
            return true;
        }
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        if(boardState.checkBoardArray[kingI,kingJ]){
            return true;
        }else{
            return false;
        }
    }
    
    public void setCheck(string color){
        Debug.Log("setCheck");
        this.checkCount = 0;
        this.inFlg = false;
        this.checkObject = null;
        GameObject king;
        List<GameObject> objects = new List<GameObject>();
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
            //objects = GameObject.FindGameObjectsWithTag("black");
            color = "black";
        }else{
            king = GameObject.Find("Black King(Clone)");
            //objects = GameObject.FindGameObjectsWithTag("white");
            color = "white";
        }
        if(king == null){
            return;
        }
        for(int x=0;x<king.GetComponent<Chess>().getMaxI();x++){
            for(int y=0;y<king.GetComponent<Chess>().getMaxJ();y++){
                boardState.checkBoardArray[x,y] = false;
                if(boardState.chessBoardArray[x,y] == null){
                    continue;
                }else if(boardState.chessBoardArray[x,y].tag.Equals(color)){
                    objects.Add(boardState.chessBoardArray[x,y]);
                }
            }
        }
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        Chess chess;
        int i=0,j=0,cellNumber=0;
        int vi=0,vj=0;
        foreach(GameObject gameObject in objects){
            chess = gameObject.GetComponent<Chess>();
            i = (int)- (gameObject.transform.position.x -16) / 4;
            j = (int)(gameObject.transform.position.z + 16) / 4;
            cellNumber = i*8+j;
            Debug.Log(gameObject.name + " i:" + i + " j:" + j);
            foreach(Vector3 vector in chess.canMovePosition(cellNumber)){
                vi = (int)-(vector.x - 16) / 4;
                vj = (int)(vector.z + 16) / 4;
               // Debug.Log("vi:"+vi+" vj:"+vj);
                boardState.checkBoardArray[vi,vj] = true;
                if(boardState.chessBoardArray[vi,vj] == king){
                    this.checkObject = gameObject;
                    if(Math.Abs(kingI - i) <= 1  && Math.Abs(kingJ - j) <= 1){
                        //Debug.Log(checkObject);
                        this.inFlg = true;
                        this.checkCount++;
                    }
                }
            }
        }
        /*
        for(i=0;i<king.GetComponent<Chess>().getMaxI();i++){
            for(j=0;j<king.GetComponent<Chess>().getMaxJ();j++){
                Debug.Log(boardState.chessBoardArray[i,j] + " i:"+i+" j:"+j);
                Debug.Log(boardState.checkBoardArray[i,j] + " i:"+i+" j:"+j);
            }
        }*/
        Debug.Log("inflg "+this.inFlg);
    }
}
