using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StertScript : MonoBehaviour
{
    public void change(bool state)
    {
        if (state)
        {
           
        }

    }
    


    public void onClickBotton()
    {
        Toggle easy = GameObject.Find("easy").GetComponent<Toggle>();
        bool easyflag = easy.isOn;
        Debug.Log(easyflag);
        if (easyflag)
        {
            PlayerPrefs.SetInt("Level",1);
        }else
        {
            PlayerPrefs.SetInt("Level",2);
        }
        Toggle first = GameObject.Find("white").GetComponent<Toggle>();
        bool firstflag = first.isOn;
        Debug.Log(firstflag);
        if (firstflag)
        {
            PlayerPrefs.SetInt("First", 1);
        }
        else
        {
            PlayerPrefs.SetInt("First", 2);
        }
        PlayerPrefs.Save();

        SceneManager.LoadScene("chessMain");
    }
    
}
