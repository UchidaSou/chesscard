using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EasyNPC : Player
{
    
    public override GameObject selectedChess()
    {
        int cardUse = Random.Range(0,7);
        GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
        int r = Random.Range(0,chesses.Length);
        return chesses[r];
    }

    public override Vector3 selectedMovePosition()
    {
        GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
        GameObject[] square = GameObject.FindGameObjectsWithTag("Respawn");
        int r = Random.Range(0,square.Length);
        Vector3 vector = square[r].transform.position;
        return vector;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.setPlayerState(this.GetComponent<PlayerState>(),1);
    }    
}
