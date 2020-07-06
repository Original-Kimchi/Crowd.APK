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
    [SerializeField] private Text userID;
    [SerializeField] private Transform idBox;

    private float time;
    private GameObject[] users = null;
    private Text[] userIDList = null;
    

    private void Awake()
    {
        users = GameObject.FindGameObjectsWithTag("Player");
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        userIDList = new Text[users.Length];
        time = 100f;
        for (int i = 0; i < users.Length; i++)
        {
            userIDList[i] = Instantiate(userID,idBox);
            userIDList[i].text = users[i].name;
        }
    }

    private void Update()
    {
        for(int i = 0; i < users.Length; i++)
        {
            if (!users[i].activeSelf)
                userIDList[i].gameObject.SetActive(false);
            userIDList[i].transform.position = Camera.main.WorldToScreenPoint(users[i].transform.position) + (Vector3.up * 30f);
        }
        time -= Time.deltaTime;
        timeText.text = "Time: " + ((int)time).ToString();
        score.text = "Score: " + myPlayer.GetScore().ToString();
    }
}
