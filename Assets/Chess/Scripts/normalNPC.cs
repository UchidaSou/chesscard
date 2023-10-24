using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalNPC : Player
{
    private Vector3 selectedPosition;
    public override GameObject selectedChess()
    {
        GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
        int i=0,j=0,cellNumber=0;
        Chess chess;
        int maxMaterial = -1;
        Vector3 maxScoreVector = new Vector3();
        GameObject maxChessObject = null;
        GameObject enemeyObject = null;
        Chess enemeyChess = null;
        foreach(GameObject gameObject in chesses){
            chess = gameObject.GetComponent<Chess>();
            i = (int)-(gameObject.transform.position.x - 16) / 4;
            j = (int)(gameObject.transform.position.z + 16) / 4;
            cellNumber = i*8 + j;
            List<Vector3> vectors = chess.canMovePosition(cellNumber);
            Debug.Log(gameObject);
            foreach(Vector3 vector in vectors){
                i = (int) -(vector.x - 16) / 4;
                j = (int) (vector.z + 16) / 4;
                Debug.Log(chess.boardState.chessBoardArray[i,j]);
                enemeyObject = chess.boardState.chessBoardArray[i,j];
                if(enemeyChess == null){
                    continue;
                }
                enemeyChess = enemeyObject.GetComponent<Chess>();
                if(enemeyChess.getMaterial() > maxMaterial){
                    maxMaterial = enemeyChess.getMaterial();
                    maxScoreVector = vector;
                    maxChessObject = gameObject;
                    Debug.Log(enemeyObject);
                }
            }
        }
        Debug.Log(maxChessObject);
        this.selectedPosition = maxScoreVector;
        return maxChessObject;
    }

    public override Vector3 selectedMovePosition()
    {
        return this.selectedPosition;
    }

    void Start()
    {
        this.setPlayerState(this.GetComponent<PlayerState>(),2);
    }  
}
