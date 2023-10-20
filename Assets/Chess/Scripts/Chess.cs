using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
public abstract class Chess:MonoBehaviour
{
    public GameObject brightSquare;
    private Vector3 beforeVector;
    private Vector3 firstVector;

    private string color;
    private int move;
    public bool canMove = true;
    public int count = 0;

    public void ShowCanMovePosition(){
        if(!this.canMove){
            return;
        }
        Vector3 position = this.gameObject.transform.position + new Vector3(-16,0,16);
        int i = (int)-position.x/4;
        int j = (int)position.z/4;
        int cellNumber = i*8+j;
        List<Vector3> canMoveList = canMovePosition(cellNumber);
        GameObject square;
        SquareState squareState;
        State state = this.gameObject.GetComponent<State>();
        foreach(Vector3 vec in canMoveList){
            square = GameObject.Instantiate(this.brightSquare,vec,Quaternion.Euler(0,0,0));
            squareState = square.GetComponent<SquareState>();
            squareState.setColor(state.getColor());
        }
    }

    public void movePosition(Vector3 position){
        Vector3 vector = this.gameObject.transform.position;
        setBeforeVector(vector);
        vector.x = position.x;
        vector.z = position.z;
        this.gameObject.transform.position = vector;
        State state = this.gameObject.GetComponent<State>();
        if(state.getSetUp() == 5){
            this.gameObject.GetComponent<Pawn>().first = false;
        }
        this.setMove(1);
    }

    public abstract List<Vector3> canMovePosition(int cellNumber);


    public void setBrightSquare(GameObject brightSquare){
        this.brightSquare = brightSquare;
    }

    public void setFirstVector(Vector3 firstVector){
        this.firstVector = firstVector;
    }

    public Vector3 getFirstVector(){
        return this.firstVector;
    }

    public void setBeforeVector(Vector3 beforeVector){
        this.beforeVector = beforeVector;
    }

    public Vector3 getBeforeVector(){
        return this.beforeVector;
    }


    public GameObject getBrightSquare(){
        return this.brightSquare;
    }

    public void setMove(int move){
        this.move = move;
    }

    public int getMove(){
        return this.move;
    }

    public void destoryChess(Collider collider){
        State state = this.gameObject.GetComponent<State>();
        GameObject gameObject = GameObject.Find("Game");
        GameObject now = gameObject.GetComponent<Game>().nowPlayer;
        Player player = now.GetComponent<Player>();
        string color = player.getColor();
        GameObject retiredObject;
        if(color.Equals("black")){
            color = "white";
        }else{
            color = "black";
        }
        string retiredColor = collider.tag;
        retiredObject = GameObject.Find(retiredColor+"Retired");
        if(!collider.tag.Equals("Respawn") && !collider.name.Equals("Bounds")){
            if(collider.tag.Equals(this.GetComponent<State>().getColor())){
                return;
            }
            if(!collider.tag.Equals(color)){
                collider.transform.parent = retiredObject.transform;
                collider.transform.position = retiredObject.transform.position;
                collider.tag = "Retired";
                //Destroy(collider.gameObject);
            }
            
        }
    }
}
