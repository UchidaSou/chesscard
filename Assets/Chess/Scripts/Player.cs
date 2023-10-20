using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Player:MonoBehaviour
{
    private String mycolor;
    private PlayerState playerState;

    public void setPlayerState(PlayerState playerState,int state){
        this.playerState = playerState;
        this.playerState.setState(state);
        
    }
/*
    public void selectedChess(GameObject gameObject){
        Vector3 pos = gameObject.transform.position;
        pos.y += 0.5f;
        gameObject.transform.position = pos;
    }
  */ 

    public abstract GameObject selectedChess();

/*
    public void selectedMovePosition(int cellNumber,GameObject chess){
        Vector3 pos = ChessUiEngine.ToWorldPoint(cellNumber);
        chess.GetComponent<Chess>().setBeforeVector(chess.transform.position);
        pos.y = chess.transform.position.y;
        chess.transform.position = pos;
        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Respawn");
        for(int i=0;i<gameObject.Length;i++){
            Destroy(gameObject[i]);
        }
    }
    
*/
    public abstract Vector3 selectedMovePosition();

    public void setColor(String color){
        this.mycolor = color;
    }

    public String getColor(){
        return this.mycolor;
    }

    public PlayerState getPlayerState(){
        return this.playerState;
    }


}

