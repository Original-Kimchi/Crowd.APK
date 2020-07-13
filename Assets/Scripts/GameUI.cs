using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    private Player myPlayer;

    // UI
    [SerializeField] private Text score;
    [SerializeField] private List<string> ranking;
    [SerializeField] private Text timeText;
    [SerializeField] private Text playerID;
    [SerializeField] private Transform idBox;
    private Text[] playerIDList = null;

    private float time;
    private GameObject[] players = null;  // Player Objects
    

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        playerIDList = new Text[players.Length];
        time = 100f;
        for (int i = 0; i < players.Length; i++)
        {
            playerIDList[i] = Instantiate(playerID,idBox);
            playerIDList[i].text = players[i].name;
        }
    }

    private void Update()
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (!players[i].activeSelf)
                playerIDList[i].gameObject.SetActive(false);
            playerIDList[i].transform.position = Camera.main.WorldToScreenPoint(players[i].transform.position) + (Vector3.up * 30f);
        }
        time -= Time.deltaTime;
        timeText.text = "Time: " + ((int)time).ToString();
        score.text = "Score: " + myPlayer.GetScore().ToString();
    }
}
