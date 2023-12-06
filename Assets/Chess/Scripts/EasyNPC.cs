using UnityEngine;

public class EasyNPC : Player
{
    
    public override GameObject selectedChess()
    {
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        int cardUse = Random.Range(0,7);
        GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
        if(checker.isCheck(this.getColor())){
            if(this.getColor().Equals("white")){
                return GameObject.Find("White King(Clone)");
            }else{
                return GameObject.Find("Black King(Clone)");
            }
        }
        int r = Random.Range(0,chesses.Length);
        return chesses[r];
    }

    public override Vector3 selectedMovePosition()
    {
        GameObject gameObject = GameObject.Find("Game");
        Checker checker = gameObject.GetComponent<Game>().checker;
        bool check = checker.isCheck(this.gameObject.GetComponent<Player>().getColor());
        if(check && checker.inFlg){
            Debug.Log(checker.checkObject.name);
            return checker.checkObject.transform.position;
        }
        GameObject[] chesses = GameObject.FindGameObjectsWithTag(this.getColor());
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
        int x = UnityEngine.Random.Range(0,20);
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
