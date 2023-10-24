using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
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
        int count = 0;
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        int kingCellNumber = kingI*8 + kingJ;
        List<Vector3> kingCanMove = king.GetComponent<King>().canMovePosition(kingCellNumber);
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
                        count++;
                    }
                }
            }
        }
        if(count >= kingCanMove.ToArray().Length){
            return true;
        }else{
            return false;
        }
    }

    public bool isCheck(string color){
        GameObject king;
        GameObject[] objects;
        if(color.Equals("white")){
            king = GameObject.Find("White King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("black");
        }else{
            king = GameObject.Find("Black King(Clone)");
            objects = GameObject.FindGameObjectsWithTag("white");
        }
        Vector3 kingPosition = king.transform.position + new Vector3(-16,0,16);
        int kingI = (int)-kingPosition.x/4;
        int kingJ = (int)kingPosition.z/4;
        int kingCellNumber = kingI*8 + kingJ;
        Vector3 now = ChessUiEngine.ToWorldPoint(kingCellNumber);
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
                    int Cell = I*8+J;
                    if(Cell == kingCellNumber){
                        Debug.Log(gameObject.name+" color:"+color);
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
}
