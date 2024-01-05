using System.Collections.Generic;
using UnityEngine;

public class Rook : Chess
{
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        int maxI = this.getMaxI();
        int maxJ = this.getMaxJ();
        List<Vector3> canMoveList = new List<Vector3>();
        if(!this.canMove){
            return canMoveList;
        }
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        GameObject gameObject;
        for(int k=1;j+k<maxJ;k++){
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
        for(int k=1;i+k<maxI;k++){
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
