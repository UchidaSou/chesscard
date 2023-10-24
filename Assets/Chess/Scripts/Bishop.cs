using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bishop : Chess
{
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        GameObject gameObject;
        bool upFlg=false,downFlg=false;
        for(int k=1;k<8-j;k++){
            if(!upFlg && i+k<8){
                gameObject = boardState.chessBoardArray[i+k,j+k];
                if(gameObject != null){
                    upFlg = true;
                    if(!gameObject.tag.Equals(this.tag)){
                        canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8 + (j+k)));
                    }
                }else{
                    canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8+(j+k)));
                }
            }
            if(!downFlg && i-k>=0){
                gameObject = boardState.chessBoardArray[i-k,j+k];
                if(gameObject != null){
                    downFlg = true;
                    if(!gameObject.tag.Equals(this.tag)){
                        canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8 + (j+k)));
                    }
                }else{
                    canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8+(j+k)));
                }
            }
            if(upFlg && downFlg){
                break;
            }
        }
        upFlg = false;
        downFlg = false;
        for(int k=1;k<j+1;k++){
            if(!upFlg && i+k<8){
                gameObject = boardState.chessBoardArray[i+k,j-k];
                if(gameObject != null){
                    upFlg = true;
                    if(!gameObject.tag.Equals(this.tag)){
                        canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8 + (j-k)));
                    }
                }else{
                    canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8+(j-k)));
                }
            }
            if(!downFlg && i-k>=0){
                gameObject = boardState.chessBoardArray[i-k,j-k];
                if(gameObject != null){
                    downFlg = true;
                    if(!gameObject.tag.Equals(this.tag)){
                        canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8 + (j-k)));
                    }
                }else{
                    canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8+(j-k)));
                }
            }
            if(upFlg && downFlg){
                break;
            }
        }
        return canMoveList;
    }  

    void Start(){
        this.setMaterial(4);
    }
}
