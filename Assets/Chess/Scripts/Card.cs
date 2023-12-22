using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public bool resurrection = true;
    public bool turnreverse = true;
    public bool setmine = true;
    public bool twicemove = true;
    public bool canntmove = true;
    public bool notusecard = true;
    public bool usecard = true;
    public int point = 10;
    public Text text;

    public GameObject mine;
    public GameObject ressuEfect,reverceEffect,twiceEffect,cantMoveEffect,insCantMove;
    public GameObject board;
    [SerializeField]
    GameObject mineSquare;
    [SerializeField]
    GameObject minePositionSquare;
    AudioSource audioSource;
    [SerializeField]
    AudioClip[] audioClips = new AudioClip[5];
    [SerializeField]
    TMP_Text plainText;
    [SerializeField]
    Image image;
    bool cameraFlg = false;

    public IEnumerator Resurrection(string color,int state){
        bool flg = false;
        List<GameObject> retiredList;
        GameObject retiredObject;
        GameObject select = new GameObject();
        BoardState boardState = board.GetComponent<BoardState>();
        if(color.Equals("white")){
            retiredList = boardState.whiteRetired;
        }else{
            retiredList = boardState.blackRetired;
        }
        if(GameObject.Find(color+"Retired").transform.childCount == 0){
            GameObject.Find("Game").GetComponent<Game>().useCard = false;
            yield break;
        }
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        if(state == 0){
            GameObject position = GameObject.Find(color + "RessCameraPosition");
            Camera.main.transform.parent = position.transform;
            Debug.Log(Camera.main.transform.localPosition);
            cameraFlg = true;
            StartCoroutine(cameraMove(color));
            yield return new WaitUntil(() => !cameraFlg);
            plainText.text = "復活させる駒を選んでください";
            Debug.Log("クリック待ち");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            Debug.Log("クリック後");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
                Debug.Log(hit.collider.gameObject.name);
                GameObject gameObject = hit.collider.gameObject;
                if(gameObject.layer == LayerMask.NameToLayer("chess")){
                    Vector3 firstVec = hit.collider.gameObject.GetComponent<Chess>().getFirstVector();
                    int x = (int)-(firstVec.x - 16) / 4;
                    int z = (int)(firstVec.z + 16) / 4;
                    if(gameObject.tag.Equals("Retired")){
                        if(boardState.chessBoardArray[x,z] == null){
                            if(gameObject.transform.parent.name.Equals(color + "Retired")){
                                Debug.Log("Ok");
                                select = hit.collider.gameObject;
                                flg = true;
                            }else{
                                Debug.Log("No");
                            }
                        }else if(!boardState.chessBoardArray[x,z].tag.Equals(color)){
                            select = hit.collider.gameObject;
                            flg = true;
                        }else{
                            flg = false;
                        }
                    }else{
                        Debug.Log("No Retired");
                        audioSource.PlayOneShot(this.audioClips[4]);
                        audioSource.PlayDelayed(0.001f);
                    }
                }else{
                    audioSource.PlayOneShot(this.audioClips[4]);
                    audioSource.PlayDelayed(0.001f);
                    flg = false;
                }
            }
            Camera.main.transform.parent = GameObject.Find(color+"DefualtCameraPosition").transform;
            StartCoroutine(cameraMove(color));
            cameraFlg = true;
            yield return new WaitUntil(() => !cameraFlg);
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            plainText.text = "";
        }else{
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            int size = retiredList.Count;
            if(size == 0){
                Debug.Log("No size");
                GameObject.Find("Game").GetComponent<Game>().useCard = false;
                yield break;
            }
            int r = Random.Range(0,size);
            select = retiredList[r];
            Vector3 firstVec = select.GetComponent<Chess>().getFirstVector();
            int x = (int)-(firstVec.x - 16) / 4;
            int z = (int)(firstVec.z + 16) / 4;
            Debug.Log(select.name + " " + x + " " + z);
            if(boardState.chessBoardArray[x,z] != null){
                if(!boardState.chessBoardArray[x,z].tag.Equals(color)){
                    Debug.Log("not color");
                    flg = true;
                }
                if(boardState.chessBoardArray[x,z].name.Contains("King")){
                    Debug.Log("King");
                    GameObject.Find("Game").GetComponent<Game>().useCard = false;
                    yield break;
                }
            }else{
                flg = true;
            }
        }
        if(state == 0 && !flg){
            Debug.Log("No flg");
            GameObject.Find("Game").GetComponent<Game>().useCard = false;
            yield break;
        }
        Debug.Log(select.name);
        Chess chess = select.GetComponent<Chess>();
        Vector3 vector = chess.getFirstVector() + new Vector3(-16,0,16);
        int i = (int)-vector.x/4;
        int j  = (int)vector.z/4;
        select.tag = color;
        select.transform.position = chess.getFirstVector();
        Instantiate(ressuEfect,select.transform.position,select.transform.rotation);
        audioSource.PlayOneShot(this.audioClips[1]);
        audioSource.PlayDelayed(0.001f);
        retiredList.Remove(select);
        if(boardState.chessBoardArray[i,j] != null){
            GameObject gameObject = boardState.chessBoardArray[i,j];
            if(gameObject.tag.Equals("white")){
                boardState.whiteRetired.Add(gameObject);
            }else{
                boardState.blackRetired.Add(gameObject);
            }
            string retiredObjectname = gameObject.tag + "Retired";
            retiredObject = GameObject.Find(retiredObjectname);
            gameObject.transform.position = retiredObject.transform.position;
            gameObject.transform.parent = retiredObject.transform;
            gameObject.tag = "Retired";
            chess = gameObject.GetComponent<Chess>();
            Game game = GameObject.Find("Game").GetComponent<Game>();
            Player player = game.nowPlayer.GetComponent<Player>();
            player.setScore(player.getScore() - chess.getMaterial());
        }
        boardState.chessBoardArray[i,j] = select;
        select.transform.parent = null;
        this.resurrection = false;
        this.point -= 15;
        this.text.text = this.point.ToString();
        if(state == 0){
            Destroy(GameObject.Find("ResurrectionBtn").gameObject);
        }
        StartCoroutine(GameObject.Find("Game").GetComponent<Game>().ChangeTurn());
        GameObject.Find("Game").GetComponent<Game>().useCard = false;
    }

    IEnumerator cameraMove(string color){
        image.raycastTarget = true;
        Quaternion quaternion = Camera.main.transform.localRotation;
        for(int s = 90;s>0;s--){
            Camera.main.transform.localPosition = Camera.main.transform.localPosition * Mathf.Sin(s*(Mathf.PI/180));
            quaternion.x = quaternion.x * Mathf.Sin(s*(Mathf.PI/180));
            quaternion.y = quaternion.y * Mathf.Sin(s*(Mathf.PI/180));
            quaternion.z = quaternion.z * Mathf.Sin(s*(Mathf.PI/180));
            Camera.main.transform.localRotation = quaternion;
            yield return new WaitForSeconds(0.01f);     
        }
        Camera.main.transform.localPosition = new Vector3(0,0,0);
        Camera.main.transform.localRotation = Quaternion.Euler(0,0,0);
        image.raycastTarget = false;
        cameraFlg = false;
    }

    public IEnumerator turnReverse(GameObject beforeMoveObject, int state){
        Debug.Log("Reverse");
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        int i = (int) -(beforeMoveObject.transform.position.x - 16) / 4;
        int j = (int) (beforeMoveObject.transform.position.z + 16) /4;
        BoardState boardState = board.GetComponent<BoardState>();
        boardState.chessBoardArray[i,j] = null;
        Chess chess = beforeMoveObject.GetComponent<Chess>();
        Vector3 vector = chess.getBeforeVector();
        beforeMoveObject.transform.position = vector;
        i = (int) -(vector.x - 16) / 4;
        j = (int) (vector.z + 16) / 4;
        boardState.chessBoardArray[i,j] = beforeMoveObject;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
        if(state == 0){
            Destroy(GameObject.Find("TurnReverseBtn").gameObject);
        }
        this.turnreverse = false;
        this.point -= 10;
        this.text.text = this.point.ToString();
        GameObject.Find("Game").GetComponent<Game>().useCard = false;
        yield break;
    }

    public IEnumerator setMine(string color,int setup){
        int mode = PlayerPrefs.GetInt("Mode",0);
        int cellNumber=-1,maxI = 0,maxJ = 0;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        if(setup == 0){
            BoardState boardState = board.GetComponent<BoardState>();
            if(mode == 0){
                maxI = 8;
                maxJ = 8;
            }else{
                maxI = 6;
                maxJ = 5;
            }
            for(int i=0;i<maxI;i++){
                for(int j=0;j<maxJ;j++){
                    if(boardState.chessBoardArray[i,j] != null){
                        continue;
                    }
                    GameObject.Instantiate(minePositionSquare,ChessUiEngine.ToWorldPoint(i*8+j),Quaternion.Euler(0,0,0));
                }
            }
            int col=0,row=0;
            plainText.text = "設置する場所を選んでください";
            Debug.Log("クリック待ち");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            Debug.Log("クリック後");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
                Debug.Log(hit.collider.gameObject.name);
                Vector3 vector3 = hit.collider.gameObject.transform.position;
                col = (int)-(vector3.x - 16) / 4;
                row = (int)(vector3.z + 16) / 4;
                Debug.Log("i :"+col+" j:"+row);
                if(boardState.chessBoardArray[col,row] == null){
                    cellNumber = col*8+row;
                }else{
                    GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawn");
                    for(int k = 0;k<respawns.Length;k++){
                        Destroy(respawns[k]);
                    }
                    GameObject.Find("Game").GetComponent<Game>().useCard = false;
                    audioSource.PlayOneShot(this.audioClips[4]);
                    audioSource.PlayDelayed(0.001f);
                    GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
                    plainText.text = "";
                    yield break;
                }
            }
            GameObject[] positions = GameObject.FindGameObjectsWithTag("Respawn");
            for(int k = 0;k<positions.Length;k++){
                Destroy(positions[k]);
            }
        }else{
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            bool ok = true;
            int i=0,j= 0;
            BoardState boardState = this.board.GetComponent<BoardState>();
            while(ok){
                if(mode == 0){
                    cellNumber = Random.Range(2*8,5*8+7);
                }else{
                    cellNumber = Random.Range(2*8,3*8+4);
                    if(cellNumber < 3*8 && cellNumber > 2*8+4){
                        if(color.Equals("white")){
                            cellNumber = Random.Range(3*8,3*8+4);
                        }else{
                            cellNumber = Random.Range(2*8,2*8+4);
                        }
                    }
                }
                i = cellNumber / 8;
                j = cellNumber % 8;
                if(boardState.chessBoardArray[i,j] == null){
                    ok = false;
                }
            }
        }
        if(cellNumber < 0){
            GameObject.Find("Game").GetComponent<Game>().useCard = false;
            audioSource.PlayOneShot(audioClips[4]);
            audioSource.PlayDelayed(0.001f);
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            plainText.text = "";
            yield break; 
        }
        Vector3 vector = ChessUiEngine.ToWorldPoint(cellNumber);
        Debug.Log("setMine " + cellNumber);
        GameObject setmine = GameObject.Instantiate(mine,vector,Quaternion.Euler(0,0,0));
        audioSource.PlayOneShot(this.audioClips[0]);
        audioSource.PlayDelayed(0.001f);
        setmine.GetComponent<Mine>().color = color;
        GameObject ms = GameObject.Instantiate(mineSquare,setmine.transform.position,Quaternion.Euler(0,0,0),setmine.transform);
        setmine.GetComponent<Mine>().board = GameObject.Find("Game").GetComponent<Game>().checker.board;
        audioSource.PlayOneShot(audioClips[0]);
        audioSource.PlayDelayed(0.001f);
        ms.tag = "mine";
        switch(color){
            case "white":
                ms.layer = 8;
                break;
            case "black":
                ms.layer = 9;
                break;
        }
        this.setmine = false;
        this.point -= 5;
        this.text.text = this.point.ToString();
        GameObject.Find("Game").GetComponent<Game>().useCard = false;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
        if(setup == 0){
            Destroy(GameObject.Find("SetMineBtn").gameObject);
        }
        plainText.text = "";
    }
    public IEnumerator twiceMove(string color,int state){
        bool flg = false;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        if(state == 0){
            plainText.text = "駒を選んでください";
            Debug.Log("クリック待ち");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            Debug.Log("クリック後");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
                Debug.Log(hit.collider.gameObject.name);
                GameObject select = hit.collider.gameObject;
                Chess chess = select.GetComponent<Chess>();
                Debug.Log(select.name);
                if(select.tag.Equals(color) && (chess.getSetUp()==0 || chess.getSetUp()==5)){
                    select.GetComponent<Chess>().setMove(2);
                    Instantiate(twiceEffect,select.transform.position,Quaternion.Euler(0,0,90),select.transform);
                    audioSource.PlayOneShot(this.audioClips[1]);
                    audioSource.PlayDelayed(0.001f);
                    flg = true;
                    Debug.Log("Twice "+select.name);
                }else{
                    GameObject.Find("Game").GetComponent<Game>().useCard = false;
                    audioSource.PlayOneShot(this.audioClips[4]);
                    audioSource.PlayDelayed(0.001f);
                    GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
                    plainText.text = "";
                    yield break;
                }
            }
        }else{
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            List<GameObject> objects = new List<GameObject>();
            GameObject[] cheesses = GameObject.FindGameObjectsWithTag(color);
            foreach(GameObject chess in cheesses){
                if(chess.name.Contains("Pawn") || chess.name.Contains("King")){
                    objects.Add(chess);
                }
            }
            int r = Random.Range(0,objects.Count);
            objects[r].GetComponent<Chess>().setMove(2);
            Instantiate(twiceEffect,objects[r].transform.position,Quaternion.Euler(0,0,90),objects[r].transform);
            audioSource.PlayOneShot(this.audioClips[1]);
            audioSource.PlayDelayed(0.001f);
            flg = true;
            Debug.Log("Twice " + objects[r].name);
        }
        if(state==0 && !flg){
            GameObject.Find("Game").GetComponent<Game>().useCard = false;
            audioSource.PlayOneShot(this.audioClips[4]);
            audioSource.PlayDelayed(0.001f);
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            plainText.text = "";
            yield break;
        }
        this.twicemove = false;
        this.point -= 6;
        this.text.text = this.point.ToString();
        GameObject.Find("Game").GetComponent<Game>().useCard = false;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
        if(state == 0){
            Destroy(GameObject.Find("TwiceMoveBtn").gameObject);
        }
        plainText.text = "";
    }


    public IEnumerator canntMove(string color,int state){
        bool flg = false;
        GameObject select = new GameObject();
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        if(state == 0){
            plainText.text = "駒を選んでください";
            Debug.Log("クリック待ち");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            Debug.Log("クリック後");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.tag.Equals(color) || hit.collider.gameObject.layer != LayerMask.NameToLayer("chess")){
                    GameObject.Find("Game").GetComponent<Game>().useCard = false;
                    audioSource.PlayOneShot(this.audioClips[4]);
                    audioSource.PlayDelayed(0.001f);
                    GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
                    plainText.text = "";
                    yield break;
                }else{
                    select = hit.collider.gameObject;
                    flg = true;
                }
            }
        }else{
            GameObject[] objects;
            if(color.Equals("white")){
            objects = GameObject.FindGameObjectsWithTag("black");
            }else{
                objects = GameObject.FindGameObjectsWithTag("white");
            }
            int r = Random.Range(0,objects.Length);
            select = objects[r];
            flg = true;
        }
        if(state == 0 && !flg){
            GameObject.Find("Game").GetComponent<Game>().useCard = false;
            audioSource.PlayOneShot(this.audioClips[4]);
            audioSource.PlayDelayed(0.001f);
            GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
            plainText.text = "";
            yield break;
        }
        Game game = GameObject.Find("Game").GetComponent<Game>();
        switch(color){
            case "white":
                game.canntMoveObject[0] = select;
                game.count = 1;
                break;
            case "black":
                game.canntMoveObject[1] = select;
                game.count = 2;
                break;
        }
        select.GetComponent<Chess>().canMove = false;
        Debug.Log("cantMove " + select.name);
        insCantMove = Instantiate(cantMoveEffect,select.transform.position,Quaternion.Euler(0,0,0));
        audioSource.PlayOneShot(this.audioClips[3]);
        audioSource.PlayDelayed(0.001f);
        this.canntmove = false;
        this.point -= 10;
        this.text.text = this.point.ToString();
        GameObject.Find("Game").GetComponent<Game>().useCard = false;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
        if(state == 0){
            Destroy(GameObject.Find("CantMoveBtn").gameObject);
        }
        plainText.text = "";
    }

    public IEnumerator notUseCard(Card card,int state){
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        card.usecard = false;
        this.point -= 12;
        GameObject.Find("Game").GetComponent<Game>().mainCanvas.SetActive(true);
        if(state == 0){
            Destroy(GameObject.Find("NotCardBtn").gameObject);
        }
        this.text.text = this.point.ToString();
        this.notusecard = false;
        GameObject.Find("Game").GetComponent<Game>().useCard = false;
        yield break;
    }

    void Start(){
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
    }
}
