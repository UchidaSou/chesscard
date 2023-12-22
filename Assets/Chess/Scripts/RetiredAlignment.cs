using UnityEngine;

public class RetiredAlignment : MonoBehaviour
{
    int count = 0;
    [SerializeField]
    GameObject board;
    private BoardState boardState;
    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.childCount != this.count && this.gameObject.transform.childCount >= 1 && this.count < this.gameObject.transform.childCount){
            GameObject gameObject = this.gameObject.transform.GetChild(this.count).gameObject;
            Vector3 vector = this.gameObject.transform.position;
            if(this.gameObject.transform.position.x < 0){
                gameObject.transform.position = new Vector3(vector.x + 3*(int)(this.count/2),gameObject.transform.position.y,vector.z + 3*(int)(this.count%2));
            }else{
                gameObject.transform.position = new Vector3(vector.x - 3*(int)(this.count/2),gameObject.transform.position.y,vector.z + 3*(int)(this.count%2));
            }
            this.count = this.gameObject.transform.childCount;
        }else{
            this.count = this.gameObject.transform.childCount;
        }
        for(int c=0;c<this.count;c++){
            GameObject child = this.gameObject.transform.GetChild(c).gameObject;
            Chess chess = child.GetComponent<Chess>();
            Outline outline = child.GetComponent<Outline>();
            Vector3 first = chess.getFirstVector();
            int i = (int)-(first.x - 16) / 4;
            int j = (int)(first.z + 16) / 4;
            if(boardState.chessBoardArray[i,j] == null){
                outline.OutlineColor = Color.red;
                outline.OutlineWidth = 2;
            }else{
                outline.OutlineColor = Color.clear;
            }

        }
    }

    void Start(){
        this.boardState = this.board.GetComponent<BoardState>();
    }
}
