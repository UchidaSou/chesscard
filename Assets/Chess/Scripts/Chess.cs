using System.Collections.Generic;
using UnityEngine;
public abstract class Chess:MonoBehaviour
{
    public GameObject brightSquare;
    private Vector3 beforeVector;
    private Vector3 firstVector;

    private string color;
    private int move;
    public bool canMove = true;
    public int count = 0;
    public GameObject board;
    public BoardState boardState;
    private int material;
    private int maxI,maxJ;

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
        State state = this.gameObject.GetComponent<State>();
        foreach(Vector3 vec in canMoveList){
            square = GameObject.Instantiate(this.brightSquare,vec,Quaternion.Euler(0,0,0));
        }
    }

    public void movePosition(Vector3 position){
        Vector3 vector = this.gameObject.transform.position;
        int i = (int)-(vector.x-16)/4;
        int j = (int)(vector.z+16)/4;
        boardState.chessBoardArray[i,j] = null;
        setBeforeVector(vector);
        vector.x = position.x;
        i = (int)-(vector.x - 16) / 4;
        vector.z = position.z;
        j = (int)(vector.z + 16) / 4;
        GameObject gameObject = boardState.chessBoardArray[i,j];
        Game game = GameObject.Find("Game").GetComponent<Game>();
        Player player = game.nowPlayer.GetComponent<Player>();
        if(gameObject != null){
            if(gameObject.transform.childCount != 0){
                Destroy(gameObject.transform.GetChild(0).gameObject);
            }
            if(gameObject.tag.Equals("white")){
                boardState.whiteRetired.Add(gameObject);
            }else{
                boardState.blackRetired.Add(gameObject);
            }
            string retiredObjectname = gameObject.tag + "Retired";
            GameObject retiredObject = GameObject.Find(retiredObjectname);
            gameObject.transform.position = retiredObject.transform.position;
            gameObject.tag = "Retired";
            Chess chess = gameObject.GetComponent<Chess>();
            player.setScore(player.getScore() - chess.getMaterial());
        }
        boardState.chessBoardArray[i,j] = this.gameObject;
        if(this.gameObject.transform.childCount != 0){
            Destroy(this.gameObject.transform.GetChild(0).gameObject);
        }
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

    public void setMaterial(int material){
        this.material = material;
    }

    public int getMaterial(){
        return this.material;
    }

    public int getScore(int i,int j){
        return (int)this.boardState.imbalance[i,j] * this.getMaterial();
    }

    public void setMaxI(int maxI){
        this.maxI = maxI;
    }

    public void setMaxJ(int maxJ){
        this.maxJ = maxJ;
    }

    public int getMaxI(){
        return this.maxI;
    }
    public int getMaxJ(){
        return this.maxJ;
    }
}
