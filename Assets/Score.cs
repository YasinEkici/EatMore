using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    //private Bait bait;
    [SerializeField] private TMP_Text scoreTxt;
    

    void Start()
    {    
        scoreTxt.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        int score = Bait.score;
        scoreTxt.text = "Score: " + score;
    }
}
