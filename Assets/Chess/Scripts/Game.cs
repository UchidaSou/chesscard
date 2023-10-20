using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Game : MonoBehaviour
{
    public GameObject firstPlayer;
    public GameObject secondPlayer;
    public GameObject nowPlayer;

    public Checker checker;
    public ChessUiEngine uiEngine;
    Player player;
    PlayerState playerState;
    GameObject chessObject;
    public GameObject firstCamera,secondCamera;
    Chess chess;
    Vector3 selectedPosition;
    GameObject[] canntMoveObject = new GameObject[2];
    int count = 0;
    GameObject beforeMoveObject = null;

    void Start(){
        secondCamera.SetActive(false);
        uiEngine.SetupPieces();
        //先行プレイヤーの設定
        //firstPlayer.GetComponent<Player>().mycolor = "white";
        player = firstPlayer.GetComponent<Player>();
        player.setColor("white");
        /*
        player = getPlayerCompornent(firstPlayer);
        player.mycolor = "white";
        */
        //後行プレイヤーの設定
        player = secondPlayer.GetComponent<Player>();
        player.setColor("black");
        //secondPlayer.GetComponent<Player>().mycolor = "black";
        /*
        player = getPlayerCompornent(secondPlayer);
        player.mycolor = "black";
        */
        //現在のプレイヤーを設定
        nowPlayer = firstPlayer;
        player = nowPlayer.GetComponent<Player>();
        //player = nowPlayer.GetComponent<Player>();
        

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        bool npcFlg = false;
        if(Input.GetMouseButtonDown(0) || nowPlayer.GetComponent<PlayerState>().getState() != 0){
            GameObject[] reapwn = GameObject.FindGameObjectsWithTag("Respawn");
            if(reapwn.Length > 0){
                foreach(GameObject gameObject in reapwn){
                    Destroy(gameObject);
                }
            }
            chessObject = player.selectedChess();
            if(chessObject == null){
                return;
            }
            if(chessObject.gameObject.tag.Equals(player.getColor())){
                chess = chessObject.GetComponent<Chess>();
                chess.ShowCanMovePosition();
                int count=0;
                if(nowPlayer.GetComponent<PlayerState>().getState() != 0){
                    while(GameObject.FindGameObjectsWithTag("Respawn").Length == 0){
                        chessObject = player.selectedChess();
                        chess = ChessUiEngine.chessGetComponent(chessObject);
                        chess.ShowCanMovePosition();
                        count++;
                        if(count > 50){
                            return;
                        }
                    }
                    selectedPosition = player.selectedMovePosition();
                    npcFlg = true;
                }
                beforeMoveObject = chessObject;
            }
            if(chessObject.gameObject.tag == "Respawn" || npcFlg == true){
                if(nowPlayer.GetComponent<PlayerState>().getState() == 0){
                    selectedPosition = chessObject.gameObject.transform.position;
                }
                reapwn = GameObject.FindGameObjectsWithTag("Respawn");
                if(reapwn.Length > 0){
                    foreach(GameObject gameObject in reapwn){
                        Destroy(gameObject);
                    }
                }
                chess.movePosition(selectedPosition);
                chessObject = null;
                /*if(checker.isCheck(player.getColor())){
                    Debug.Log("LOSE");
                }*/
                //プレイヤーを交代する
                changeTurn();
            }
        }   
    }

    public void changeTurn(){
        switch(player.getColor()){
            case "white":
                //firstCamera.SetActive(false);
                //secondCamera.SetActive(true);
                nowPlayer = secondPlayer;
                break;
            case "black":
                //firstCamera.SetActive(true);
                //secondCamera.SetActive(false);
                nowPlayer = firstPlayer;
                break;
        }
        player = nowPlayer.GetComponent<Player>();
        playerState = nowPlayer.GetComponent<PlayerState>();
        for(int i=0;i<count;i++){
            if(canntMoveObject[i] != null){
                Chess chess = canntMoveObject[i].GetComponent<Chess>();
                chess.count++;
                if(chess.GetComponent<Chess>().count%2 == 0){
                    chess.GetComponent<Chess>().canMove = true;
                    canntMoveObject[i] = null;
                }
            }
        }
    }

    public Player getPlayerCompornent(GameObject playerObject){
        int playerState = playerObject.GetComponent<PlayerState>().state;
        switch(playerState){
            case 0:
                return playerObject.gameObject.GetComponent<RealPlayer>();
            case 1:
                return playerObject.gameObject.GetComponent<EasyNPC>();
        }
        return null;
    }

    public void Resurrection(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.resurrection){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        card.Resurrection(color);
        changeTurn();
    }

    public void turnReverse(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.turnreverse){
            return;
        }
        string color = beforeMoveObject.tag;
        if(color.Equals("white")){
            canntMoveObject[0] = beforeMoveObject;
            count = 1;
        }else{
            canntMoveObject[1] = beforeMoveObject;
            count = 2;
        }
        beforeMoveObject.GetComponent<Chess>().canMove = false;
        card.turnReverse(beforeMoveObject);
        changeTurn();
    }

    public void setMine(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.setmine){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        card.setMine(color);
    }

    public void twiceMove(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.twicemove){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        card.twiceMove(color);
    }

    public void canntMove(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.canntmove){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        GameObject effectObject = card.canntMove(color);
        if(color.Equals("white")){
            canntMoveObject[0] = effectObject;
            count = 1;
        }else{
            canntMoveObject[1] = effectObject;
            count = 2;
        }
        
    }

    public void nullBtn(){

    }
}
