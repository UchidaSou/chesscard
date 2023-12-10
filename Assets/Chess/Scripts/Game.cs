using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject firstPlayer;
    public GameObject secondPlayer;
    public GameObject nowPlayer;

    public Checker checker;
    public ChessUiEngine uiEngine;
    Player player;
    GameObject chessObject;
    public GameObject firstCamera,secondCamera;
    Chess chess;
    Vector3 selectedPosition;
    public GameObject[] canntMoveObject = new GameObject[2];
    public TMP_Text loser,winner;
    public int count = 0;
    GameObject beforeMoveObject = null;
    public GameObject resultCanvas;
    public GameObject mainCanvas;
    public bool playStop = false;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] effectAudioClip = new AudioClip[3];
    public bool useCard = false;
    private Coroutine coroutine;

    void Start(){
        resultCanvas.SetActive(false);
        audioSource = this.gameObject.GetComponent<AudioSource>();
        int x = PlayerPrefs.GetInt("Level",1);
        int y = PlayerPrefs.GetInt("First", 1);
        int mode = PlayerPrefs.GetInt("Mode",0);
        if(mode == 0){
            uiEngine.SetupPieces();
        }else{
            uiEngine.SetUpDemo();
            Vector3 pos = firstCamera.transform.position;
            pos.z = -5.0f;
            firstCamera.transform.position = pos;
            pos = secondCamera.transform.position;
            pos.z = -5.0f;
            secondCamera.transform.position = pos;
        }
        if(y == 1){
            firstPlayer.AddComponent<RealPlayer>();
            secondCamera.SetActive(false);
            player = firstPlayer.GetComponent<Player>();
            player.setColor("white");
            this.removeAura(player.getColor());
            if(x == 1){
                secondPlayer.AddComponent<EasyNPC>();
            }else{
                secondPlayer.AddComponent<normalNPC>();
            }
            player = secondPlayer.GetComponent<Player>();
            player.setColor("black");
            this.removeAura(player.getColor());
        }else{
            secondPlayer.AddComponent<RealPlayer>();
            firstCamera.SetActive(false);
            player = secondPlayer.GetComponent<Player>();
            player.setColor("black");
            if(x == 1){
                firstPlayer.AddComponent<EasyNPC>();
            }else{
                firstPlayer.AddComponent<normalNPC>();
            }
            player = firstPlayer.GetComponent<Player>();
            player.setColor("white");
        }
        GameObject[] gameObjects;
        Chess chess;
        //先行プレイヤーの設定
        player = firstPlayer.GetComponent<Player>();
        gameObjects = GameObject.FindGameObjectsWithTag(player.getColor());
        foreach(GameObject gameObject in gameObjects){
            chess = gameObject.GetComponent<Chess>();
            player.setScore(player.getScore() + chess.getMaterial());
        }
        //後行プレイヤーの設定
        player = secondPlayer.GetComponent<Player>();
        gameObjects = GameObject.FindGameObjectsWithTag(player.getColor());
        foreach(GameObject gameObject in gameObjects){
            chess = gameObject.GetComponent<Chess>();
            player.setScore(player.getScore() + chess.getMaterial());
        }
        //現在のプレイヤーを設定
        nowPlayer = firstPlayer;
        player = nowPlayer.GetComponent<Player>();
        Invoke("firstsetAura",0.5f); 
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(playStop){
            return;
        }
        if(audioSource.isPlaying){
            return;
        }
        if(useCard){
            return;
        }
        bool npcFlg = false;
        if(Input.GetMouseButtonDown(0) ||player.getState() != 0){
            if(player.getState() != 0){
                player.UseCard();
            }
            GameObject[] reapwn = GameObject.FindGameObjectsWithTag("Respawn");
            if(reapwn.Length > 0){
                foreach(GameObject gameObject in reapwn){
                    Destroy(gameObject);
                }
            }
            chessObject = player.selectedChess();
            if(chessObject == null){
                return;
            }
            if(chessObject.gameObject.tag.Equals(player.getColor())){
                chess = chessObject.GetComponent<Chess>();
                chess.ShowCanMovePosition();
                int count=0;
                if(player.getState() != 0){
                    while(GameObject.FindGameObjectsWithTag("Respawn").Length == 0 && chessObject == null){
                        chessObject = player.selectedChess();
                        chess = chessObject.GetComponent<Chess>();
                        chess.ShowCanMovePosition();
                        if(GameObject.FindGameObjectsWithTag("Respawn").Length == 0){
                            continue;
                        }
                        count++;
                        if(count > 50){
                            return;
                        }
                    }
                    selectedPosition = player.selectedMovePosition();
                    npcFlg = true;
                }
                beforeMoveObject = chessObject;
            }
            if(chessObject.gameObject.tag == "Respawn" || npcFlg == true){
                if(player.getState() == 0){
                    selectedPosition = chessObject.gameObject.transform.position;
                }
                reapwn = GameObject.FindGameObjectsWithTag("Respawn");
                if(reapwn.Length > 0){
                    foreach(GameObject gameObject in reapwn){
                        Destroy(gameObject);
                    }
                }
                audioSource.PlayOneShot(audioSource.clip);
                audioSource.PlayDelayed(0.001f);
                chess.movePosition(selectedPosition);
                chessObject = null;
                //プレイヤーを交代する
                ChangeTurn();
            }
        }   
    }

    public void ChangeTurn(){
        if(!player.card.usecard){
            player.card.usecard = true;
        }
        this.removeAura(player.getColor());
        switch(player.getColor()){
            case "white":
                nowPlayer = secondPlayer;
                break;
            case "black":
                nowPlayer = firstPlayer;
                break;
        }
        player = nowPlayer.GetComponent<Player>();
        checker.setCheck(player.getColor());
        if(checker.isCheck(player.getColor())){
            Debug.Log("check " + player.getColor());
        }
        if(checker.isCheckMate(player.getColor())){
            mainCanvas.SetActive(false);
            resultCanvas.SetActive(true);
            loser.text = "Loser " + player.getColor();
            winner.text = "Winner ";
            if(player.getColor().Equals("white")){
                winner.text = winner.text + "black";
            }else{
                winner.text = winner.text + "white";
            }
            playStop = true;
            return;
        }
        setAura(player.getColor());
        player.card.point += 1;
        for(int i=0;i<count;i++){
            if(canntMoveObject[i] != null){
                Chess chess = canntMoveObject[i].GetComponent<Chess>();
                chess.count++;
                if(chess.GetComponent<Chess>().count%2 == 0){
                    chess.GetComponent<Chess>().canMove = true;
                    canntMoveObject[i] = null;
                    Destroy(player.GetComponent<Card>().insCantMove);
                }
            }
        }
    }

    public void firstsetAura(){
        string color = player.getColor();
        setAura(color);
    }
    public void setAura(string color){
        GameObject[] objects = GameObject.FindGameObjectsWithTag(color);
        Chess chess;
        int i=0,j=0;
        foreach(GameObject gameObject in objects){
            chess = gameObject.GetComponent<Chess>();
            i = (int)-(gameObject.transform.position.x - 16) / 4;
            j = (int)(gameObject.transform.position.z + 16) / 4;
            if(chess.canMovePosition(i*8+j).Count != 0){
                gameObject.GetComponent<Outline>().OutlineWidth = 2;
                gameObject.GetComponent<Outline>().OutlineColor = Color.green;
            }else{
                gameObject.GetComponent<Outline>().OutlineWidth = 0;
            }
        }
        GameObject king = GameObject.Find("White King(Clone)");
        king.GetComponent<Outline>().OutlineColor = Color.black;
        king.GetComponent<Outline>().OutlineWidth = 5;
        king = GameObject.Find("Black King(Clone)");
        king.GetComponent<Outline>().OutlineColor = Color.cyan;
        king.GetComponent<Outline>().OutlineWidth = 5;
    }

    private void removeAura(string color){
        GameObject[] objects = GameObject.FindGameObjectsWithTag(color);
        foreach(GameObject gameObject in objects){
            gameObject.GetComponent<Outline>().OutlineWidth = 0;
        }
    }



    public void Resurrection(){
        if(coroutine != null){
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.resurrection || card.point < 15){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        this.useCard = true;
        Debug.Log(color);
        coroutine = StartCoroutine(card.Resurrection(color,player.getState()));
    }

    public void TurnReverse(){
        if(coroutine != null){
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.turnreverse || card.point < 10 || beforeMoveObject == null){
            return;
        }
        string color = beforeMoveObject.tag;
        card.turnReverse(beforeMoveObject);
        beforeMoveObject.GetComponent<Chess>().canMove = false;
        if(color.Equals("white")){
            canntMoveObject[0] = beforeMoveObject;
            count = 1;
        }else{
            canntMoveObject[1] = beforeMoveObject;
            count = 2;
        }
        ChangeTurn();
    }

    public void setMine(){
        if(coroutine != null){
            Debug.Log("Stop coroutine");
            StopCoroutine(coroutine);
            coroutine = null;
        }
        this.useCard = true;
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.setmine || card.point < 5){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        this.useCard = true;
        coroutine = StartCoroutine(card.setMine(color,player.getState()));
    }

    public void twiceMove(){
        if(coroutine != null){
            Debug.Log("Stop coroutine");
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.twicemove || card.point < 6){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        this.useCard = true;
        coroutine = StartCoroutine(card.twiceMove(color,player.getState()));
    }

    public void canntMove(){
        if(coroutine != null){
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.canntmove || card.point < 10){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        this.useCard = true;
        coroutine = StartCoroutine(card.canntMove(color,player.getState()));
    }

    public void notUseCard(){
        if(coroutine != null){
            StopCoroutine(coroutine);
            coroutine = null;
        }
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.notusecard || card.point < 12){
            return;
        }
        if(nowPlayer.Equals(firstPlayer)){
            card.notUseCard(secondPlayer.GetComponent<Player>().card);
        }else{
            card.notUseCard(firstPlayer.GetComponent<Player>().card);
        }
    }

    public void GoStartBtn(){
        SceneManager.LoadScene("select");
    }
}
