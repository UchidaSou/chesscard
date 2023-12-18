using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject cheeNameCanvas;
    public bool playStop = false;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectAudioClip;
    public bool useCard = false;
    bool check = false;
    private Coroutine coroutine;
    [SerializeField]
    GameObject normalBoard;
    [SerializeField]
    GameObject miniBoard;
    [SerializeField]
    GameObject whiteRetiredObject;
    [SerializeField]
    GameObject blackRetiredObject;
    [SerializeField]
    Image effectPanel;
    [SerializeField]
    TMP_Text effectName;

    IEnumerator Fadein(){
        Image image = GameObject.Find("Fede").GetComponent<Image>();
        for(float i=1;i>=0.0f;i=i-0.01f){
            image.color = new Color(0,0,0,i);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Fadeout(){
        Image image = GameObject.Find("Fede").GetComponent<Image>();
        for(float i=0.0f;i<=1.0f;i=i+0.01f){
            image.color = new Color(0,0,0,i);
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("select");
    }
    IEnumerator cutin(string effectName){
        this.audioSource.PlayOneShot(effectAudioClip);
        this.audioSource.PlayDelayed(0.001f);
        RectTransform rectTransform = effectPanel.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(1200,0);
        rectTransform.offsetMax = new Vector2(0,200);
        this.effectName.text = effectName;
        for(int i=0;i<=10;i++){
            rectTransform.offsetMin = new Vector2(1200 - i*120,0);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1.0f);
        for(int i=0;i<=10;i++){
            rectTransform.offsetMax = new Vector2(-i*120,200);
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
    void Start(){
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.LandscapeRight;
        StartCoroutine(Fadein());
        resultCanvas.SetActive(false);
        audioSource = this.gameObject.GetComponent<AudioSource>();
        int x = PlayerPrefs.GetInt("Level",1);
        int y = PlayerPrefs.GetInt("First", 1);
        int mode = PlayerPrefs.GetInt("Mode",0);
        if(mode == 0){
            uiEngine.SetupPieces();
            Destroy(miniBoard);
            checker.board  = normalBoard;
        }else{
            uiEngine.SetUpDemo();
            Destroy(normalBoard);
            checker.board = miniBoard;
            firstCamera.transform.parent.transform.position = new Vector3(22,29,-9);
            secondCamera.transform.parent.transform.position = new Vector3(-15,30,-4);
            whiteRetiredObject.transform.position = new Vector3(12,3.5f,10);
            GameObject.Find("whiteRessCameraPosition").transform.position = new Vector3(5,21,15);
            blackRetiredObject.transform.position = new Vector3(-7,3.5f,-23);
            GameObject.Find("blackRessCameraPosition").transform.position = new Vector3(2.5f,20,-25);
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
        firstPlayer.GetComponent<Card>().board = checker.board;
        //後行プレイヤーの設定
        player = secondPlayer.GetComponent<Player>();
        gameObjects = GameObject.FindGameObjectsWithTag(player.getColor());
        foreach(GameObject gameObject in gameObjects){
            chess = gameObject.GetComponent<Chess>();
            player.setScore(player.getScore() + chess.getMaterial());
        }
        secondPlayer.GetComponent<Card>().board = checker.board;
        //現在のプレイヤーを設定
        nowPlayer = firstPlayer;
        player = nowPlayer.GetComponent<Player>();
        Invoke("firstsetAura",0.5f);
        StartCoroutine(showChessName());
    }
    // Update is called once per frame
    void Update()
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
        if(check){
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
                check = true;
                //プレイヤーを交代する
                StartCoroutine(ChangeTurn());
            }
        }   
    }

    public IEnumerator ChangeTurn(){
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
        yield return new WaitForSeconds(0.5f);
        checker.setCheck(player.getColor());
        if(checker.isCheck(player.getColor())){
            Debug.Log("check " + player.getColor());
        }
        if(checker.isCheckMate(player.getColor())){
            mainCanvas.SetActive(false);
            cheeNameCanvas.SetActive(false);
            resultCanvas.SetActive(true);
            loser.text = "Loser " + player.getColor();
            winner.text = "Winner ";
            if(player.getColor().Equals("white")){
                winner.text = winner.text + "black";
            }else{
                winner.text = winner.text + "white";
            }
            playStop = true;
            yield break;
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
        check = false;
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

    public IEnumerator showChessName(){
        GameObject namePanel = GameObject.Find("NamePanel");
        GameObject nameText = GameObject.Find("chessNameText");
        TMP_Text textMeshPro = nameText.GetComponent<TMP_Text>();
        Ray ray;
        RaycastHit hit;
        string name;
        while(!this.playStop){
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("chess")){
                    name = hit.collider.gameObject.name.Split(" ")[1].Split("(")[0];
                    textMeshPro.text = name;
                }else{
                    textMeshPro.text = "";
                }
            }
            namePanel.transform.position = Input.mousePosition + new Vector3(60,0,0);
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("stop name show");
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
        StartCoroutine(cutin("復活"));
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
        StartCoroutine(cutin("待った"));
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
        Card card = nowPlayer.GetComponent<Card>();
        if(!card.setmine || card.point < 5){
            return;
        }
        string color = nowPlayer.GetComponent<Player>().getColor();
        this.useCard = true;
        StartCoroutine(cutin("地雷"));
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
        StartCoroutine(cutin("倍行動"));
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
        StartCoroutine(cutin("封印"));
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
        StartCoroutine(cutin("禁止"));
        if(nowPlayer.Equals(firstPlayer)){
            card.notUseCard(secondPlayer.GetComponent<Player>().card);
        }else{
            card.notUseCard(firstPlayer.GetComponent<Player>().card);
        }
    }

    public void GoStartBtn(){
        StartCoroutine(Fadeout());
    }
}
