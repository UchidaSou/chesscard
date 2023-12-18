using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : Chess
{
    public bool first = true;
    public GameObject Queen;
    bool promotion = false;

    public override List<Vector3> canMovePosition(int cellNumber)
    {
        int maxI = this.getMaxI();
        int maxJ = this.getMaxJ();
        List<Vector3> canMoveList = new List<Vector3>();
        if(!this.canMove){
            return canMoveList;
        }
        if(promotion){
            canMoveList = PromotionMovePosition(cellNumber);
            return canMoveList;
        }
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        int pm;
        if(this.gameObject.tag.Equals("white")){
            pm = 1;
        }else{
            pm = -1;
        }
        int move = this.getMove();
        if(first){
            move = move * 2;
        }
        GameObject gameObject;
        for(int k=1;k<=move&&(i+k<maxI || i-k>=0);k++){
            if(this.tag.Equals("white") && i+k>=maxI){
                break;
            }else if(this.tag.Equals("black") && i-k<0){
                break;
            }
            gameObject = boardState.chessBoardArray[i+pm*k,j];
            if(gameObject != null){
                break;
            }else{
                canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm*k)*8 + j));
            }
        }
        if(j+1<maxJ && (i < this.getMaxI()-1 && i > 0) && boardState.chessBoardArray[i+pm,j+1] != null && !boardState.chessBoardArray[i+pm,j+1].tag.Equals(this.tag)){
            canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm)*8 + j+1));
        }
        if(j-1>=0 && (i<this.getMaxI()-1 && i>0) &&boardState.chessBoardArray[i+pm,j-1] != null && !boardState.chessBoardArray[i+pm,j-1].tag.Equals(this.tag)){
            canMoveList.Add(ChessUiEngine.ToWorldPoint((i+pm)*8 + j-1));
        }
        return canMoveList;
    }

    public List<Vector3> PromotionMovePosition(int cellNumber){
        int maxI = this.getMaxI();
        int maxJ = this.getMaxJ();
        List<Vector3> canMoveList = new List<Vector3>();
        int j = (int) cellNumber % 8;
        int i = (int) cellNumber / 8;
        int move = this.getMove();
        GameObject gameObject;
        for(int k=j-move;k<=j+move;k++){
            for(int l=i-move;l<=i+move;l++){
                if(k>=maxJ||k<0){
                    continue;
                }
                if(l>=maxI||l<0){
                    continue;
                }
                if(k==j&&l==i){
                    continue;
                }
                gameObject = boardState.chessBoardArray[l,k];
                if(gameObject != null && gameObject.tag.Equals(this.tag)){
                    continue;
                }
                if(boardState.checkBoardArray[l,k]){
                    continue;
                }
                canMoveList.Add(ChessUiEngine.ToWorldPoint(l*8+k));
            }
        }
        return canMoveList;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.setMove(1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.tag == "Retired" || promotion){
            return;
        }
        Vector3 vector = this.gameObject.transform.position+ new Vector3(-16,0,16);
        int i = (int)-vector.x/4;
        int j = (int)vector.z / 4;
        if(i == this.getMaxI()-1 || i == 0){
            if(PlayerPrefs.GetInt("Mode",0) == 0){
                Vector3 instantiatePosition = this.transform.position;
                instantiatePosition.y = 3.1f;
                GameObject queen = Instantiate(Queen,instantiatePosition,Quaternion.Euler(90,0,0));
                this.boardState.chessBoardArray[i,j] = queen;
                queen.tag = this.gameObject.tag;
                Chess chess = queen.GetComponent<Chess>();
                chess.board = this.board;
                chess.boardState = this.board.gameObject.GetComponent<BoardState>();
                chess.boardState.chessBoardArray = this.boardState.chessBoardArray;
                chess.boardState.checkBoardArray = this.boardState.checkBoardArray;
                chess.boardState.imbalance = this.boardState.imbalance;
                chess.setMaxI(this.getMaxI());
                chess.setMaxJ(this.getMaxJ());
                Vector3 first = this.getFirstVector();
                first.y = 3.2f;
                chess.setFirstVector(first);
                Vector3 before = this.getBeforeVector();
                before.y = 3.2f;
                chess.setBeforeVector(before);
                chess.setMaterial(9);
                chess.setBrightSquare(this.getBrightSquare());
                chess.setSetUp(1);
                Debug.Log("Promotion");
                Destroy(this.gameObject);
            }else{
                promotion = true;
            }
        }
    }
}
