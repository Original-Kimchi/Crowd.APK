using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Player MyPlayer { get; private set; }

    // UI
    [SerializeField] private Text score;
    [SerializeField] private List<Text> ranking;
    [SerializeField] private Text timeText;
    [SerializeField] private Text playerID;
    [SerializeField] private Transform idBox;
    private Text[] playerIDList = null;
    //GameOver
    [SerializeField] private Image gameoverBackground;
    private Text gameResult;
    private Text finalScore;

    private float time;
    private GameObject[] playerObjects = null;  // Player Objects
    private List<Player> players = new List<Player>();

    private readonly int mapSize = 500;

    private void Awake()
    {
        MyPlayer = PhotonNetwork.Instantiate("Player", GetEmptyLocation(), Quaternion.identity, 0).GetComponent<Player>();
        gameResult = gameoverBackground.transform.Find("GameResultTxt").GetComponent<Text>();
        finalScore = gameoverBackground.transform.Find("Score").GetComponent<Text>();
        if (PhotonNetwork.isMasterClient)
        {
            foreach (var _ in Enumerable.Range(1, 50))
            {
                var go = PhotonNetwork.Instantiate("Food", GetEmptyLocation(), Quaternion.identity, 0);
                go.AddComponent<FoodRotation>();
            }
        }
    }

    private void Start()
    {
        StartCoroutine(CoStart());
    }

    private IEnumerator CoStart()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == PhotonNetwork.room.MaxPlayers);
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
        playerIDList = new Text[playerObjects.Length];
        time = 100f;
        for (int i = 0; i < playerObjects.Length; i++)
        {
            playerIDList[i] = Instantiate(playerID, idBox);
            playerIDList[i].text = playerObjects[i].name;
            players.Add(playerObjects[i].GetComponent<Player>());
        }
    }

    private void Update()
    {
        while (ObjectBox.ObjectExist)
            ObjectBox.Dequeue();

        for(int i = 0; i < playerObjects.Length; i++)
        {
            if (!playerObjects[i].activeSelf)
                playerIDList[i].gameObject.SetActive(false);
            playerIDList[i].transform.position = Camera.main.WorldToScreenPoint(playerObjects[i].transform.position) + (Vector3.up * 30f);
        }

        Ranking();

        time -= Time.deltaTime;
        if (time <= 0)
            StartCoroutine(GameOver("time"));

        timeText.text = "Time: " + ((int)time).ToString();
        score.text = "Score: " + MyPlayer.GetScore().ToString();
    }

    public IEnumerator GameOver(string result)
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        finalScore.text = "Score" + MyPlayer.GetScore().ToString();
        gameoverBackground.gameObject.SetActive(true);
        yield return wait;
        switch (result)
        {
            case "gameover":
                gameResult.text = "Game Over!";
                break;
            case "win":
                gameResult.text = "You Win!";
                break;
            case "time":
                gameResult.text = "Time Over!";
                break;
            default:
                Debug.Log("Gameover Error");
                break;
        }
        gameResult.gameObject.SetActive(true);
        yield return wait;
        finalScore.gameObject.SetActive(true);
    }

    public Vector3 GetEmptyLocation()
    {
        while (true)
        {
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int z = Random.Range(-mapSize / 2, mapSize / 2);

            Physics.Raycast(new Vector3(x, 1 << 5, z), Vector3.down, out RaycastHit hit);
            if (hit.collider.CompareTag("Floor"))
                return new Vector3(x, 1, z);
        }
    }

    private void Ranking()
    {
        players.OrderByDescending(x => x.GetScore());
        for(int i =0; i < players.Count; i++)
        {
            ranking[i].text = players[i].name + ": " + players[i].GetScore().ToString();
        }
    }
}
