using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public int checkCount = 0;
    public GameObject checkObject = null;
    public bool inFlg = false;
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
        List<Vector3> kingCanMove = king.GetComponent<King>().canMovePosition(kingCellNumber);
        if(kingCanMove.Count == 0){
            return false;
        }
        BoardState boardState = GameObject.Find("Board").GetComponent<BoardState>();
        int count = kingCanMove.Count;
        List<Vector3> removeList = new List<Vector3>();
        foreach(GameObject gameObject in objects){
            Chess chess = gameObject.GetComponent<Chess>();
            Vector3 position = gameObject.transform.position + new Vector3(-16,0,16);
            int i = (int)-position.x/4;
            int j = (int)position.z/4;
            int cellNumber = i*8+j;
            List<Vector3> list = chess.canMovePosition(cellNumber);
            foreach(Vector3 kingVec in kingCanMove){
                Vector3 kingCanMovePosition = kingVec + new Vector3(-16,0,16);
                int KI = (int)-kingCanMovePosition.x/4;
                int KJ = (int)kingCanMovePosition.z/4;
                int kingCanMoveCellNumber = KI*8+KJ;
                foreach(Vector3 vector in list){
                    Vector3 canMovePosition = vector + new Vector3(-16,0,16);
                    int I = (int)-canMovePosition.x / 4;
                    int J = (int)canMovePosition.z/4;
                    int Cell = I*8+J;
                    if(Cell == kingCanMoveCellNumber){
                        if(removeList.Exists(x => x.Equals(vector))){
                            removeList.Add(vector);
                        }
                    }
                }
            }
        }
        foreach (Vector3 vector in removeList)
        {
            kingCanMove.Remove(vector);            
        }
        if(kingCanMove.Count == 0){
            return true;
        }else{
            return false;
        }
    }

    public bool isCheck(string color){
        inFlg = false;
        checkCount = 0;
        GameObject king;
        GameObject[] objects;
        GameObject board = GameObject.Find("Board");
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("black");
        }else{
            king = GameObject.Find("Black King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("white");
        }
        for(int x=0;x<king.GetComponent<Chess>().getMaxI();x++){
            for(int y=0;y<king.GetComponent<Chess>().getMaxJ();y++){
                boardState.checkBoardArray[x,y] = false;
            }
        }
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        int kingCellNumber = kingI*8 + kingJ;
        List<Vector3> kingMovePosition = king.GetComponent<Chess>().canMovePosition(kingCellNumber);
        if(kingMovePosition.Count == 0){
            return false;
        }
        foreach(GameObject gameObject in objects){
            if(gameObject.name.Equals(king.name)){
                continue;
            }
            Chess chess = gameObject.GetComponent<Chess>();
            Vector3 position = gameObject.transform.position + new Vector3(-16,0,16);
            int i = (int)-position.x/4;
            int j = (int)position.z/4;
            int cellNumber = i*8+j;
            List<Vector3> list = chess.canMovePosition(cellNumber);
            if(list.Count > 0){
                foreach(Vector3 vector in list){
                    Vector3 canMovePosition = vector + new Vector3(-16,0,16);
                    int I = (int)-canMovePosition.x / 4;
                    int J = (int)canMovePosition.z/4;
                    boardState.checkBoardArray[I,J] = true;
                    int Cell = I*8+J;
                    if(Cell == kingCellNumber){
                        foreach(Vector3 move in kingMovePosition){
                            int moveI = (int) - (move.x - 16) / 4;
                            int moveJ = (int) (move.z + 16) /4;
                            int moveCell = moveI*8 + moveJ;
                            if(moveCell == cellNumber){
                                Debug.Log(gameObject);
                                checkObject = gameObject;
                                inFlg = true;
                                break;
                            }
                        }
                        checkCount++;
                    }
                }
            }
        }
        if(checkCount >= 1 || inFlg){
            return true;
        }else{
            return false;
        }
    }
    
}
