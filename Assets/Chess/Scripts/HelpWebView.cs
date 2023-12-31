using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HelpWebView : MonoBehaviour
{
    [SerializeField]
    GameObject helpCanvasObject;
    [SerializeField]
    GameObject mainCanvas;
    RectTransform rectTransform;
    bool open = false;
    [SerializeField]
    Image image;
    [SerializeField]
    GameObject[] button;
    [SerializeField]
    GameObject[] canvas;

    // Start is called before the first frame update
    void Start()
    {
        this.rectTransform = helpCanvasObject.GetComponent<RectTransform>();
        this.rectTransform.sizeDelta = new Vector2(0,0);
        for(int i=0;i<button.Length;i++){
            button[i].SetActive(false);
            canvas[i].SetActive(false);
        }
        helpCanvasObject.SetActive(false);
        }

    public void onClickHelpView(){
        Debug.Log("Help");
        if(open) return;
        helpCanvasObject.SetActive(true);
        StartCoroutine(showHelp());
    }

    IEnumerator showHelp(){
        this.open = true;
        for(int i=0;i<=90;i=i+10){
            rectTransform.sizeDelta = mainCanvas.GetComponent<RectTransform>().sizeDelta * Mathf.Sin(i*Mathf.PI/180);
            yield return new WaitForSeconds(0.01f);
        }
        for(int i=0;i<button.Length;i++){
            button[i].SetActive(true);
        }
        this.open = false;
        onClickGame();
    }

    public void onClickCloseHelp(){
        Debug.Log("close");
        StartCoroutine(closeHelp());
    }

    IEnumerator closeHelp(){
        this.open = true;
        for(int i=0;i<button.Length;i++){
            button[i].SetActive(false);
            canvas[i].SetActive(false);
        }
        for(int i=0;i<=90;i=i+10){
            rectTransform.sizeDelta = mainCanvas.GetComponent<RectTransform>().sizeDelta * Mathf.Cos(i*Mathf.PI/180);
            yield return new WaitForSeconds(0.01f);
        }
        this.open = false;
        this.helpCanvasObject.SetActive(false);
    }

    public void onClickGame(){
        //image.color = Color.red;
        canvas[0].SetActive(true);
        canvas[1].SetActive(false);
        canvas[2].SetActive(false);
    }

    public void onClickChess(){
        //image.color = Color.blue;
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
        canvas[2].SetActive(false);
    }

    public void onClickCard(){
        //image.color = Color.green;
        canvas[0].SetActive(false);
        canvas[1].SetActive(false);
        canvas[2].SetActive(true);
    }
}
