using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Chess
{
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        GameObject gameObject;
        for(int k=1;j+k<8;k++){
            gameObject = boardState.chessBoardArray[i,j+k];
            if(gameObject != null){
                if(!gameObject.tag.Equals(this.tag)){
                    canMoveList.Add(ChessUiEngine.ToWorldPoint(i*8 + (j+k)));
                }
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint(i*8 + (j+k)));
            }
        }
        for(int k=1;i+k<8;k++){
            gameObject = boardState.chessBoardArray[i+k,j];
            if(gameObject != null){
                if(!gameObject.tag.Equals(this.tag)){
                    canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8 + j));
                }
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8 + j));
            }
        }
        for(int k=1;j-k>=0;k++){
            gameObject = boardState.chessBoardArray[i,j-k];
            if(gameObject != null){
                if(!gameObject.tag.Equals(this.tag)){
                    canMoveList.Add(ChessUiEngine.ToWorldPoint(i*8 + (j-k)));
                }
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint(i*8 + (j-k)));
            }
        }
        for(int k=1;i-k>=0;k++){
            gameObject = boardState.chessBoardArray[i-k,j];
            if(gameObject != null){
                if(!gameObject.tag.Equals(this.tag)){
                    canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8 + j));
                }
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8 + j));
            }
        }
        return canMoveList;
    }

    void Start(){
        this.setMaterial(5);
    }
}
