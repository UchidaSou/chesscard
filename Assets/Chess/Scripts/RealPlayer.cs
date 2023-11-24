using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealPlayer : Player
{
    public Text text;
    public override GameObject selectedChess()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,100.0f)){
            return hit.collider.gameObject;
        }
        return null;
    }

    public override Vector3 selectedMovePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,100.0f)){
            if(hit.collider.gameObject.tag.Equals("Respawn")){
                return hit.collider.gameObject.transform.position;
            }
        }
        return new Vector3(0,0,0);
    }

    // Start is called before the first frame update
    public void Start()
    {
        base.setPlayerState(this.gameObject.GetComponent<PlayerState>(),0);
        base.card = this.gameObject.GetComponent<Card>();
        this.text = card.text;
        this.text.text = base.card.point.ToString();
    }

    public void Update(){
        this.text.text = base.card.point.ToString();
    }

    public override void UseCard()
    {
        return;
    }
}
