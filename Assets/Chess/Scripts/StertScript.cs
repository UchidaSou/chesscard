using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StertScript : MonoBehaviour
{
    public void onClickBotton()
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
        PlayerPrefs.SetInt("Mode",0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("chessMain");
    }
    public void onClickDemoBotton()
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

        SceneManager.LoadScene("chessMain");
    }
}
