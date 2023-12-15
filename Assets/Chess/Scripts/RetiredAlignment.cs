using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetiredAlignment : MonoBehaviour
{
    int count = 0;
    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.childCount != this.count){
            GameObject gameObject = this.gameObject.transform.GetChild(this.count).gameObject;
            Vector3 vector = this.gameObject.transform.position;
            if(this.gameObject.transform.position.x < 0){
                gameObject.transform.position = new Vector3(vector.x + 3*(int)(this.count/2),gameObject.transform.position.y,vector.z + 3*(int)(this.count%2));
            }else{
                gameObject.transform.position = new Vector3(vector.x - 3*(int)(this.count/2),gameObject.transform.position.y,vector.z + 3*(int)(this.count%2));
            }
            Debug.Log(this.gameObject.transform.GetChild(this.count));
            this.count = this.gameObject.transform.childCount;
        }
    }
}
