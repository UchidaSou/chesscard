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
        GameObject[] objects;
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("black");
        }else{
            king = GameObject.Find("Black King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("white");
        }
        if(king.gameObject.tag.Equals("Retired")){
            return true;
        }
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        int kingCellNumber = kingI*8 + kingJ;
        King king1 = king.GetComponent<King>();
        List<Vector3> kingCanMove = king1.canMovePosition(kingCellNumber);
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
        GameObject[] objects;
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("black");
        }else{
            king = GameObject.Find("Black King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("white");
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
        this.checkCount = 0;
        this.inFlg = false;
        this.checkObject = null;
        GameObject king;
        GameObject[] objects;
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("black");
        }else{
            king = GameObject.Find("Black King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("white");
        }
        if(king == null){
            return;
        }
        for(int x=0;x<king.GetComponent<Chess>().getMaxI();x++){
            for(int y=0;y<king.GetComponent<Chess>().getMaxJ();y++){
                boardState.checkBoardArray[x,y] = false;
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
            foreach(Vector3 vector in chess.canMovePosition(cellNumber)){
                vi = (int)-(vector.x - 16) / 4;
                vj = (int)(vector.z + 16) / 4;
                boardState.checkBoardArray[vi,vj] = true;
                if(boardState.chessBoardArray[vi,vj] == king){
                    this.checkObject = gameObject;
                    if(Math.Abs(kingI - i) <= 1  && Math.Abs(kingJ - j) <= 1){
                        Debug.Log(checkObject);
                        this.inFlg = true;
                        this.checkCount++;
                    }
                }
            }
        }
    }
}
