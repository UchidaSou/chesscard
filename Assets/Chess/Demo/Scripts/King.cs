using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chess
{
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        Vector3 instantiatePosition;
        int move = this.getMove();
        for(int k=j-move;k<=j+move;k++){
            for(int l=i-move;l<=i+move;l++){
                if(k>8||k<0){
                    continue;
                }
                if(l>=8||l<0){
                    continue;
                }
                if(k==j&&l==i){
                    continue;
                }
                instantiatePosition = ChessUiEngine.ToWorldPoint(l*8+k);
                canMoveList.Add(instantiatePosition);
            }
        }
        return canMoveList;
    }

    public void OnTriggerEnter(Collider collider){
        this.destoryChess(collider);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
