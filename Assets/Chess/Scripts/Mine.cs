using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mine : MonoBehaviour
{
    void OnTriggerEnter(Collider collider){
        string tag = collider.tag;
        if(tag.Equals("white") || tag.Equals("black")){
            Debug.Log(collider.name);
            GameObject retiredObject = GameObject.Find(tag + "Retired");
            collider.transform.parent = retiredObject.transform;
            collider.transform.position = retiredObject.transform.position;
            collider.tag = "Retired";
            Destroy(this.gameObject);
        }
    }

    void Update(){
        GameObject gameObject = GameObject.Find("Game");
        Game game = gameObject.GetComponent<Game>();
        this.gameObject.tag = game.nowPlayer.GetComponent<Player>().getColor();
    }
}
