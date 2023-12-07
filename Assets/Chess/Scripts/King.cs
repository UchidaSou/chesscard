using System.Collections.Generic;
using UnityEngine;

public class King : Chess
{
    public List<GameObject> mylist;
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        mylist.Clear();
        int maxI = this.getMaxI();
        int maxJ = this.getMaxJ();
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        int move = this.getMove();
        GameObject gameObject;
        for(int k=j-move;k<=j+move;k++){
            for(int l=i-move;l<=i+move;l++){
                if(k>=maxJ||k<0){
                    continue;
                }
                if(l>=maxI||l<0){
                    continue;
                }
                if(k==j&&l==i){
                    continue;
                }
                gameObject = boardState.chessBoardArray[l,k];
                if(gameObject != null && gameObject.tag.Equals(this.tag)){
                    mylist.Add(gameObject);
                    continue;
                }
                if(boardState.checkBoardArray[l,k]){
                    continue;
                }
                canMoveList.Add(ChessUiEngine.ToWorldPoint(l*8+k));
            }
        }
        return canMoveList;
    }

    void Start(){
        this.setMove(1);
        this.setMaterial(0);
        mylist = new List<GameObject>();
    }
}
