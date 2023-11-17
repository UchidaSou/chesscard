using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Player:MonoBehaviour
{
    private String mycolor;
    private PlayerState playerState;
    private int score;
    public Card card;

    public void setPlayerState(PlayerState playerState,int state){
        this.playerState = playerState;
        this.playerState.setState(state);
        
    }

    public abstract GameObject selectedChess();
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

    public void setScore(int score){
        this.score = score;
    }

    public int getScore(){
        return this.score;
    }

    void Start(){
        this.setScore(0);
    }
}

