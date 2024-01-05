using UnityEngine;

public class EasyNPC : Player
{
    
    public override GameObject selectedChess()
    {
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        int cardUse = Random.Range(0,7);
        GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
        if(checker.isCheck(this.getColor()) && checker.inFlg){
            if(this.getColor().Equals("white")){
                return GameObject.Find("White King(Clone)");
            }else{
                return GameObject.Find("Black King(Clone)");
            }
        }
        Chess chess;
        int r,i,j;
        do{
            r = Random.Range(0,chesses.Length);
            chess = chesses[r].GetComponent<Chess>();
            i = (int)-(chesses[r].transform.position.x - 16) / 4;
            j = (int)(chesses[r].transform.position.z + 16) / 4;
        }while(chess.canMovePosition(i*8+j).Count == 0);
        return chesses[r];
    }

    public override Vector3 selectedMovePosition()
    {
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        bool check = checker.isCheck(this.getColor());
        if(check && checker.inFlg){
            Debug.Log(checker.checkObject.name);
            return checker.checkObject.transform.position;
        }
        GameObject[] square = GameObject.FindGameObjectsWithTag("Respawn");
        int r = Random.Range(0,square.Length);
        Vector3 vector = square[r].transform.position;
        return vector;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.setState(1);
        this.card = this.gameObject.GetComponent<Card>();
    }    

    public override void UseCard()
    {
        if(PlayerPrefs.GetInt("Mode",0) == 1){
            return;
        }
        int x = UnityEngine.Random.Range(0,20);
        //int x = 10;
        switch(x){
            case 0:
             GameObject.Find("Game").GetComponent<Game>().Resurrection();
             break;
            case 1:
             GameObject.Find("Game").GetComponent<Game>().TurnReverse();
             break;
            case 2:
             GameObject.Find("Game").GetComponent<Game>().setMine();
             break;
             case 3:
             GameObject.Find("Game").GetComponent<Game>().twiceMove();
             break;
            case 4:
             GameObject.Find("Game").GetComponent<Game>().canntMove();
             break;
            case 5:
             GameObject.Find("Game").GetComponent<Game>().notUseCard();
             break;
            default:
             break;
        }
    }
}
