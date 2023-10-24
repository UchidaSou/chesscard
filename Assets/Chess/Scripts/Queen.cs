using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : Chess
{
    private Rook rook;
    private Bishop bishop;

    public void setRook(Rook rook){
        this.rook = rook;
    }

    public void setBishop(Bishop bishop){
        this.bishop = bishop;
    }

 

    // Start is called before the first frame update
    void Start()
    {
        this.AddComponent<Rook>();
        this.AddComponent<Bishop>();
        setRook(this.gameObject.GetComponent<Rook>());
        rook.brightSquare = this.brightSquare;
        rook.board = this.board;
        rook.boardState = this.boardState;
        setBishop(this.gameObject.GetComponent<Bishop>());
        bishop.brightSquare = this.brightSquare;
        bishop.boardState = this.boardState;
        this.setMaterial(9);
    }


    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        foreach(Vector3 vector in rook.canMovePosition(cellNumber)){
            canMoveList.Add(vector);
        }
        foreach(Vector3 vector in bishop.canMovePosition(cellNumber)){
            canMoveList.Add(vector);
        }
        return canMoveList;
    }

}
