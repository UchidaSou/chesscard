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
        Vector3 instantiatePosition;
        for(int k=1;k<3;k++){
            if(i+k>=8 || j-(3-k)<0){
                continue;
            }
            instantiatePosition = ChessUiEngine.ToWorldPoint((i+k)*8+(j-(3-k)));
            canMoveList.Add(instantiatePosition);
        }
        for(int k=1;k<3;k++){
            if(i-k<0 || j-(3-k)<0){
                continue;
            }
            instantiatePosition = ChessUiEngine.ToWorldPoint((i-k)*8+(j-(3-k)));
            canMoveList.Add(instantiatePosition);
        }
        for(int k=1;k<3;k++){
            if(j+(3-k)>=8 || i+k>=8){
                continue;
            }
            instantiatePosition = ChessUiEngine.ToWorldPoint((i+k)*8+(j+(3-k)));
            canMoveList.Add(instantiatePosition);
        }
        for(int k=1;k<3;k++){
            if(j+(3-k)>=8 || i-k<0){
                continue;
            }
            instantiatePosition = ChessUiEngine.ToWorldPoint((i-k)*8+(j+(3-k)));
            canMoveList.Add(instantiatePosition);
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
