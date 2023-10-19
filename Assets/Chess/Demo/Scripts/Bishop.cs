using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bishop : Chess
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
            direction = new Vector3(Mathf.Sin(Mathf.PI * k/2 + Mathf.PI/4),0,Mathf.Cos(Mathf.PI*k/2 + Mathf.PI/4));
            ray = new Ray(origin,direction);
            if(Physics.Raycast(ray,out hit,34)){
                if(hit.collider.gameObject.tag.Equals("Finish")){
                    switch(hit.collider.gameObject.name){
                        case "I0":
                            hitI = 0;
                            hitJ = i;
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
            int startJ,endJ;
            if(hitJ < j){
                startJ = hitJ;
                endJ = j;
            }else{
                startJ = j;
                endJ = hitJ;
            }
            switch(k%2){
                case 0:
                    if(k<2){
                        //右下
                        for(int l=1;l<endJ-j+1;l++){
                            instantiatePosition = ChessUiEngine.ToWorldPoint((i-l)*8+(j+l));
                            canMoveList.Add(instantiatePosition);
                        }
                    }else{
                        //左上
                        for(int l=0;l<j-hitJ;l++){
                            instantiatePosition = ChessUiEngine.ToWorldPoint((i+(l+1))*8+(j-(l+1)));
                            canMoveList.Add(instantiatePosition);
                        } 
                    }
                    break;
                case 1:
                    //右上
                    if(k>2){
                        for(int l=1;l<endJ-j+1;l++){
                            instantiatePosition = ChessUiEngine.ToWorldPoint((i+l)*8+(j+l));
                            canMoveList.Add(instantiatePosition);
                        }
                    }else{
                        //左下
                       for(int l=0;l<j-hitJ;l++){
                            if(i-(l+1) < 0){
                                continue;
                            }
                            instantiatePosition = ChessUiEngine.ToWorldPoint((i-(l+1))*8+(j-(l+1)));
                            canMoveList.Add(instantiatePosition);
                        } 
                    }
                    break;
            }
        }
        return canMoveList;
    }

    public void OnTriggerEnter(Collider collider){
        this.destoryChess(collider);
    }
        

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
