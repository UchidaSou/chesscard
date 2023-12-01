using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public TMP_Text result;
    int count = 0;
    GameObject beforeMoveObject = null;
    public GameObject resultCanvas;
    public GameObject mainCanvas;
    public bool playStop = false;

    void Start(){
        resultCanvas.SetActive(false);
        int x = PlayerPrefs.GetInt("Level",1);
        int y = PlayerPrefs.GetInt("First", 1);
        int mode = PlayerPrefs.GetInt("Mode",0);
        if(mode == 0){
            uiEngine.SetupPieces();
        }else{
            uiEngine.SetUpDemo();
            Vector3 pos = firstCamera.transform.position;
            pos.z = -5.0f;
            firstCamera.transform.position = pos;
            pos = secondCamera.transform.position;
            pos.z = -5.0f;
            secondCamera.transform.position = pos;
        }
        if(y == 1){
            firstPlayer.AddComponent<RealPlayer>();
            secondCamera.SetActive(false);
            player = firstPlayer.GetComponent<Player>();
            player.setColor("white");
            if(x == 1){
                secondPlayer.AddComponent<EasyNPC>();
            }else{
                secondPlayer.AddComponent<normalNPC>();
            }
            player = secondPlayer.GetComponent<Player>();
            player.setColor("black");
        }else{
            secondPlayer.AddComponent<RealPlayer>();
            firstCamera.SetActive(false);
            player = secondPlayer.GetComponent<Player>();
            player.setColor("black");
            if(x == 1){
                firstPlayer.AddComponent<EasyNPC>();
            }else{
                firstPlayer.AddComponent<normalNPC>();
            }
            player = firstPlayer.GetComponent<Player>();
            player.setColor("white");
        }
        GameObject[] gameObjects;
        Chess chess;
        //先行プレイヤーの設定
        player = firstPlayer.GetComponent<Player>();
        gameObjects = GameObject.FindGameObjectsWithTag(player.getColor());
        foreach(GameObject gameObject in gameObjects){
            chess = gameObject.GetComponent<Chess>();
            player.setScore(player.getScore() + chess.getMaterial());
        }
        //後行プレイヤーの設定
        player = secondPlayer.GetComponent<Player>();
        gameObjects = GameObject.FindGameObjectsWithTag(player.getColor());
        foreach(GameObject gameObject in gameObjects){
            chess = gameObject.GetComponent<Chess>();
            player.setScore(player.getScore() + chess.getMaterial());
        }
        //現在のプレイヤーを設定
        nowPlayer = firstPlayer;
        player = nowPlayer.GetComponent<Player>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(playStop){
            return;
        }
        if(checker.isCheckMate(player.getColor())){
            mainCanvas.SetActive(false);
            resultCanvas.SetActive(true);
            result.text = player.getColor() + " LOSE";
            playStop = true;
            return;
        }
        if(checker.isCheck(player.getColor())){
            Debug.Log("check " + player.getColor());
        }
        bool npcFlg = false;
        if(Input.GetMouseButtonDown(0) || nowPlayer.GetComponent<PlayerState>().getState() != 0){
            if(nowPlayer.GetComponent<PlayerState>().getState() != 0){
                player.UseCard();
            }
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
                        chess = chessObject.GetComponent<Chess>();
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
                //プレイヤーを交代する
                ChangeTurn();
            }
        }   
    }

    public void ChangeTurn(){
        if(!player.card.usecard){
            player.card.usecard = true;
        }
        switch(player.getColor()){
            case "white":
                nowPlayer = secondPlayer;
                break;
            case "black":
                nowPlayer = firstPlayer;
                break;
        }
        player = nowPlayer.GetComponent<Player>();
        player.card.point += 1;
        playerState = nowPlayer.GetComponent<PlayerState>();
        for(int i=0;i<count;i++){
            if(canntMoveObject[i] != null){
                Chess chess = canntMoveObject[i].GetComponent<Chess>();
                chess.count++;
                if(chess.GetComponent<Chess>().count%2 == 0){
                    chess.GetComponent<Chess>().canMove = true;
                    canntMoveObject[i] = null;
                    Destroy(player.GetComponent<Card>().insCantMove);
                }
            }
        }
    }


    public void Resurrection(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.resurrection || card.point < 15){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        card.Resurrection(color);
        ChangeTurn();
    }

    public void TurnReverse(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.turnreverse || card.point < 10 || beforeMoveObject == null){
            return;
        }
        string color = beforeMoveObject.tag;
        card.turnReverse(beforeMoveObject);
        beforeMoveObject.GetComponent<Chess>().canMove = false;
        if(color.Equals("white")){
            canntMoveObject[0] = beforeMoveObject;
            count = 1;
        }else{
            canntMoveObject[1] = beforeMoveObject;
            count = 2;
        }
        ChangeTurn();
    }

    public void setMine(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.setmine || card.point < 5){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        card.setMine(color);
    }

    public void twiceMove(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.twicemove || card.point < 6){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        card.twiceMove(color);
    }

    public void canntMove(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.canntmove || card.point < 10){
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

    public void notUseCard(){
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.notusecard || card.point < 12){
            return;
        }
        if(nowPlayer.Equals(firstPlayer)){
            card.notUseCard(secondPlayer.GetComponent<Player>().card);
        }else{
            card.notUseCard(firstPlayer.GetComponent<Player>().card);
        }
    }

    public void GoStartBtn(){
        SceneManager.LoadScene("select");
    }
}
