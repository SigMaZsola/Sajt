using System;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class StartTimer : MonoBehaviour
{

    public TMP_Text timerText;
    public bool canStart;
    float time = 3f;

    void Start()
    {
        canStart = false;
        timerText.text = "";
    }
    void Update()
    {
        if(!canStart)return;
         if(Convert.ToInt16(time)< 0){timerText.text = " "; return;}
        time -= Time.deltaTime;
        timerText.text = Convert.ToInt16(time).ToString();
        if( Convert.ToInt16(time) == 3)timerText.text = "3";
        if(Convert.ToInt16(time) == 2)timerText.text = "2";
        if(Convert.ToInt16(time) == 1)timerText.text = "1";
        if(Convert.ToInt16(time)== 0)timerText.text = "GO!";


    }
}