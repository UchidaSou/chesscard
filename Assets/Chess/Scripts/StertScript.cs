using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StertScript : MonoBehaviour
{
    bool flg = false;
    public IEnumerator onClickBotton()
    {
        Debug.Log("onClick");
        Toggle easy = GameObject.Find("easy").GetComponent<Toggle>();
        bool easyflag = easy.isOn;
        if (easyflag)
        {
            PlayerPrefs.SetInt("Level",1);
        }else
        {
            PlayerPrefs.SetInt("Level",2);
        }
        Toggle first = GameObject.Find("white").GetComponent<Toggle>();
        bool firstflag = first.isOn;
        if (firstflag)
        {
            PlayerPrefs.SetInt("First", 1);
        }
        else
        {
            PlayerPrefs.SetInt("First", 2);
        }
        PlayerPrefs.SetInt("Mode",0);
        PlayerPrefs.Save();
        Image image = GameObject.Find("Fede").GetComponent<Image>();
        for(float i=0;i<=1.0f;i=i+0.01f){
            Debug.Log("color");
            image.color = new Color(0,0,0,i);
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("chessMain");
    }

    public void onClickFade(){
        Debug.Log("Start");
        StartCoroutine(onClickBotton());
    }
    public IEnumerator onClickDemoBotton()
    {
        Toggle easy = GameObject.Find("easy").GetComponent<Toggle>();
        bool easyflag = easy.isOn;
        if (easyflag)
        {
            PlayerPrefs.SetInt("Level",1);
        }else
        {
            PlayerPrefs.SetInt("Level",2);
        }
        Toggle first = GameObject.Find("white").GetComponent<Toggle>();
        bool firstflag = first.isOn;
        if (firstflag)
        {
            PlayerPrefs.SetInt("First", 1);
        }
        else
        {
            PlayerPrefs.SetInt("First", 2);
        }

        PlayerPrefs.SetInt("Mode",1);
        PlayerPrefs.Save();
        Image image = GameObject.Find("Fede").GetComponent<Image>();
        for(float i=0;i<=1.0f;i=i+0.01f){
            Debug.Log("color");
            image.color = new Color(0,0,0,i);
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("chessMain");
    }
    public void onClickDemoFade(){
        Debug.Log("Start");
        StartCoroutine(onClickDemoBotton());
    }
    void Start(){
        StartCoroutine(Fadein());
    }
    IEnumerator Fadein(){
        Image image = GameObject.Find("Fede").GetComponent<Image>();
        for(float i=1;i>=0.0f;i=i-0.01f){
            Debug.Log("color");
            image.color = new Color(0,0,0,i);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
