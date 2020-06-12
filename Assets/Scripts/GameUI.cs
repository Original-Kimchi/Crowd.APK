using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    private Player myPlayer;
    [SerializeField] private Text score;
    [SerializeField] private List<string> ranking;
    [SerializeField] private Text timeText;

    private float time;

    private void Awake()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        time = 100f;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        timeText.text = "Time: " + ((int)time).ToString();
        score.text = "Score: " + myPlayer.GetScore().ToString();
    }
}
