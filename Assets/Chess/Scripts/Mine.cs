using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject effect;
    public GameObject board;
    public string color;
    void OnTriggerEnter(Collider collider){
        string tag = collider.tag;
        if(!this.color.Equals(tag)){
            BoardState boardState = board.GetComponent<BoardState>();
            Debug.Log(collider.name);
            if(tag.Equals("white")){
                boardState.whiteRetired.Add(collider.gameObject);
            }else{
                boardState.blackRetired.Add(collider.gameObject);
            }
            int i = (int) - (collider.gameObject.transform.position.x - 16) / 4;
            int j = (int) (collider.gameObject.transform.position.z + 16) / 4;
            boardState.chessBoardArray[i,j] = null;
            GameObject retiredObject = GameObject.Find(tag + "Retired");
            Instantiate(effect,this.transform.position,this.transform.rotation);
            collider.transform.position = retiredObject.transform.position;
            collider.tag = "Retired";
            Destroy(this.gameObject);
        }
    }

    void Update(){
        GameObject gameObject = GameObject.Find("Game");
        Game game = gameObject.GetComponent<Game>();
        this.gameObject.tag = game.nowPlayer.GetComponent<Player>().getColor();
        board = GameObject.Find("Board");
        this.tag = "mine";
    }
}
