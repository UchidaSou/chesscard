using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    private int setup;
    private string color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSetUp(int setup){
        this.setup = setup;
    }

    public void setColor(string color){
        this.color = color;
    }

    public int getSetUp(){
        return this.setup;
    }

    public string getColor(){
        return this.color;
    }
}
