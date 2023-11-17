using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chess
{
    public bool first = true;
    public GameObject Queen;

    public override List<Vector3> canMovePosition(int cellNumber)
    {
        int maxI = this.getMaxI();
        int maxJ = this.getMaxJ();
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        int pm;
        if(this.gameObject.tag.Equals("white")){
            pm = 1;
        }else{
            pm = -1;
        }
        int move = this.getMove();
        if(first){
            move = move * 2;
        }
        GameObject gameObject;
        for(int k=1;k<=move&&(i+k<maxI || i-k>=0);k++){
            gameObject = boardState.chessBoardArray[i+pm*k,j];
            if(gameObject != null){
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm*k)*8 + j));
            }
        }
        if(j+1<maxJ && boardState.chessBoardArray[i+pm,j+1] != null && !!boardState.chessBoardArray[i+pm,j+1].tag.Equals(this.tag)){
            canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm)*8 + j+1));
        }
        if(j-1>=0 && boardState.chessBoardArray[i+pm,j-1] != null && !boardState.chessBoardArray[i+pm,j-1].tag.Equals(this.tag)){
            canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm)*8 + j-1));
        }
        return canMoveList;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.setMove(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.tag == "Retired"){
            return;
        }
        Vector3 vector = this.gameObject.transform.position+ new Vector3(-16,0,16);
        int i = (int)-vector.x/4;
        if(i == this.getMaxI()){
            Vector3 instantiatePosition = this.transform.position;
            instantiatePosition.y = 3.1f;
            Instantiate(Queen,instantiatePosition,Quaternion.Euler(90,0,0));
            Destroy(this.gameObject);
        }
    }

}
