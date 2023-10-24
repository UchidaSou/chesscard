using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chess
{
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        GameObject gameObject;
        for(int k=1;k<3;k++){
            if(i+k>=8 || j-(3-k)<0){
                continue;
            }
            gameObject = boardState.chessBoardArray[i+k,j-(3-k)];
            if(gameObject == null || !gameObject.tag.Equals(this.tag)){
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8+(j-(3-k))));
            }
        }
        for(int k=1;k<3;k++){
            if(i-k<0 || j-(3-k)<0){
                continue;
            }
            gameObject = boardState.chessBoardArray[i-k,j-(3-k)];
            if(gameObject == null || !gameObject.tag.Equals(this.tag)){
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8+(j-(3-k))));
            }
        }
        for(int k=1;k<3;k++){
            if(j+(3-k)>=8 || i+k>=8){
                continue;
            }
            gameObject = boardState.chessBoardArray[i+k,j+(3-k)];
            if(gameObject == null || !gameObject.tag.Equals(this.tag)){
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i+k)*8+(j+(3-k))));
            }
        }
        for(int k=1;k<3;k++){
            if(j+(3-k)>=8 || i-k<0){
                continue;
            }
            gameObject = boardState.chessBoardArray[i-k,j+(3-k)];
            if(gameObject == null || !gameObject.tag.Equals(this.tag)){
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i-k)*8+(j+(3-k))));
            }
        }
        return canMoveList;
    }

    void Start(){
        this.setMaterial(3);
    }
}
