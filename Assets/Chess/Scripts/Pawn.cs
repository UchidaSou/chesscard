using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;

public class Pawn : Chess
{
    public bool first = true;
    public GameObject Queen;

    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        int hitJ;
        int hitI;
        Vector3 origin;
        Vector3 direction;
        Vector3 instantiatePosition;
        Ray ray;
        int pm;
        if(this.gameObject.tag.Equals("white")){
            pm = 1;
        }else{
            pm = -1;
        }
        int move = this.getMove();
        origin = ChessUiEngine.ToWorldPoint(cellNumber);
        if(first){
           direction = (ChessUiEngine.ToWorldPoint((i+pm*2)*8+j) - origin) * move;
        }else{
            direction = (ChessUiEngine.ToWorldPoint((i+pm*1)*8+j) - origin) * move;
        }
        ray = new Ray(origin,direction);
        RaycastHit hit;
        float length = Mathf.Abs(direction.x - origin.x)/2;
        Vector3 hitPosition;
        int layerMask = 1 << 7;
        layerMask = ~layerMask;
        if(Physics.Raycast(ray,out hit,length,layerMask)){
            hitPosition = hit.collider.gameObject.transform.position;
        }else{
            hitPosition = direction;
        }
        hitJ = (int)hitPosition.z % 4;
        hitI = (int)-hitPosition.x / 4 + i;
        int toJ = Mathf.Abs(hitJ - j);
        int toI;
        if(this.gameObject.tag.Equals("white")){
            toI = Mathf.Abs(hitI - i);
        }else{
            toI = Mathf.Abs(i - hitI);
        }
        int c=0;
        for(int k=1;k<=toI;k++){
            c++;
            if(c==20){
                break;
            }
            instantiatePosition = ChessUiEngine.ToWorldPoint((i+pm*k)*8+j);
            canMoveList.Add(instantiatePosition);
        }
        return canMoveList;
    }

    public void OnTriggerEnter(Collider collider){
        this.destoryChess(collider);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.setMove(1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = this.gameObject.transform.position+ new Vector3(-16,0,16);
        int i = (int)-vector.x/4;
        if(i == 7){
            Vector3 instantiatePosition = this.transform.position;
            instantiatePosition.y = 3.1f;
            Instantiate(Queen,instantiatePosition,Quaternion.Euler(90,0,0));
            Destroy(this.gameObject);
        }
    }
}
