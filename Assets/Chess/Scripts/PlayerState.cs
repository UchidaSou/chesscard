using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    //0:人 1:easyNPC 2:normalNPC
    public int state;
    
    public void setState(int state){
        this.state = state;
    }

    public int getState(){
        return this.state;
    }

}
