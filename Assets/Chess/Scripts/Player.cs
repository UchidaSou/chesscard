using System;
using UnityEngine;

public abstract class Player:MonoBehaviour
{
    private String mycolor;
    private int score;
    public Card card;
    public int state;

  public void setState(int state){
        this.state = state;
    }

    public int getState(){
        return this.state;
    }
    public abstract GameObject selectedChess();
    public abstract Vector3 selectedMovePosition();
    public abstract void UseCard();

    public void setColor(String color){
        this.mycolor = color;
    }

    public String getColor(){
        return this.mycolor;
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

