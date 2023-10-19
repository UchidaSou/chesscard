using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SquareState : MonoBehaviour
{
    private string color;

    public void setColor(string color){
        this.color = color;
    }

    public void OnTriggerEnter(Collider collider){
        if(collider.tag.Equals(this.color)){
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
