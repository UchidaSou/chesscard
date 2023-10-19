using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Chess
{
    public override List<Vector3> canMovePosition(int cellNumber)
    {
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        int hitJ=0;
        int hitI=0;
        Vector3 origin = this.gameObject.transform.position;
        Vector3 direction,hitPosition;
        Vector3 instantiatePosition;
        Ray ray;
        RaycastHit hit;
        for(int k=0;k<4;k++){
            direction = new Vector3(Mathf.Sin(Mathf.PI*k/2),0,Mathf.Cos(Mathf.PI*k/2));
            ray = new Ray(origin,direction);
            if(Physics.Raycast(ray,out hit,32)){
                if(hit.collider.gameObject.tag.Equals("Finish")){
                    switch(hit.collider.gameObject.name){
                        case "I0":
                            hitI = 0;
                            hitJ = j;
                            break;
                        case "I7":
                            hitI = 7;
                            hitJ = j;
                            break;
                        case "J0":
                            hitJ = 0;
                            hitI = i;
                            break;
                        case "J7":
                            hitJ = 7;
                            hitI = i;
                            break;
                    }
                }else{
                    hitPosition = hit.collider.gameObject.transform.position + new Vector3(-16,0,16);
                    hitI = (int)-hitPosition.x / 4;
                    hitJ = (int)hitPosition.z / 4;
                }
            }else{
                hitPosition = direction;
                hitI = (int)-hitPosition.x / 4;
                hitJ = (int)hitPosition.z / 4;
            }
            int startI,endI,startJ,endJ;
            if(hitJ < j){
                startJ = hitJ;
                endJ = j;
            }else{
                startJ = j;
                endJ = hitJ;
            }
            
            if(hitI < i){
                startI = hitI;
                endI = i;
            }else{
                startI = i;
                endI = hitI;
            }
            for(int l=startI;l<=endI;l++){
                for(int m=startJ;m<=endJ;m++){
                    if(l==i&&m==j){
                        continue;
                    }
                    instantiatePosition = ChessUiEngine.ToWorldPoint(l*8+m);
                    canMoveList.Add(instantiatePosition);
                }
            }
        }
        return canMoveList;
    }


    public void OnTriggerEnter(Collider collider){
        this.destoryChess(collider);
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
