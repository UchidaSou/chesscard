using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;

public class Pawn : Chess
{
    public bool first = true;
    public GameObject Queen;

    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        /*int hitJ;
        int hitI;
        Vector3 origin;
        Vector3 direction;
        Vector3 instantiatePosition;
        Ray ray;*/
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
        for(int k=1;k<=move&&(i+k<8 || i-k>=0);k++){
            gameObject = boardState.chessBoardArray[i+pm*k,j];
            if(gameObject != null){
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm*k)*8 + j));
            }
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
        Vector3 vector = this.gameObject.transform.position+ new Vector3(-16,0,16);
        int i = (int)-vector.x/4;
        if(i == 7){
            Vector3 instantiatePosition = this.transform.position;
            instantiatePosition.y = 3.1f;
            Instantiate(Queen,instantiatePosition,Quaternion.Euler(90,0,0));
            Destroy(this.gameObject);
        }
    }

}
