using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Score : MonoBehaviour
{
    //private Bait bait;
    [SerializeField] private UnityEngine.UI.Text scoreTxt;
    

    void Start()
    {    
        scoreTxt.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        int score = Bait.score;
        scoreTxt.text = "Score:" + " "+score.ToString();
        

    }
}
