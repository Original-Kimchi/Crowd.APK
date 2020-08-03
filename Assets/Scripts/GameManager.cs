using Photon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : PunBehaviour
{
    private string[] CharactorType = { "Alpaca", "Cat", "Chick", "Chicken", "Dog", "Goat", "Horse", "Pig", "Rabbit", "Sheep" };
    public Player MyPlayer { get; private set; }

    // UI
    [SerializeField] private Joystick joystick;
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

    private readonly int mapSize = 300;

    private bool gameEnded;

    private void Awake()
    {
        joystick = GameObject.Find("UI/Panel/Joystick").GetComponent<Joystick>();
        var n = Random.Range(0, 9);
        MyPlayer = PhotonNetwork.Instantiate(CharactorType[n], GetEmptyLocation(), Quaternion.identity, 0).GetComponent<Player>();
        gameResult = gameoverBackground.transform.Find("GameResultTxt").GetComponent<Text>();
        finalScore = gameoverBackground.transform.Find("Score").GetComponent<Text>();
        joystick.player = MyPlayer;
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

            var player = playerObjects[i].GetComponent<Player>();
            playerIDList[i].text = PhotonNetwork.playerList.ToList().Find(p => p.ID == player.PlayerId).NickName;
            players.Add(player);
        }
    }

    private void Update()
    {
        if (!gameEnded)
        {
            for (int i = 0; i < playerObjects.Length; i++)
            {
                if (playerObjects[i] == null)
                        playerIDList[i].gameObject.SetActive(false);
                else
                    playerIDList[i].transform.position = Camera.main.WorldToScreenPoint(playerObjects[i].transform.position) + (Vector3.up * 30f);
            }

            Ranking();

            time -= Time.deltaTime;
            if (time <= 0)
                StartCoroutine(GameOver("time"));

            timeText.text = "Time: " + ((int)time).ToString();
            score.text = "Score: " + PhotonNetwork.player.GetScore().ToString();
        }
    }

    public IEnumerator GameOver(string result)
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        finalScore.text = "Score: " + PhotonNetwork.player.GetScore().ToString();
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
        var playerList = PhotonNetwork.playerList.ToList();
        var playerListTemp = from player in playerList orderby player.GetScore() descending select player;
        var enumerator = playerListTemp.GetEnumerator();
        enumerator.MoveNext();

        for (int i = 0; i < playerList.Count; i++)
        {
            ranking[i].text = enumerator.Current.NickName + ": " + enumerator.Current.GetScore();
            enumerator.MoveNext();
        }
    }

    public void GoToMainScene()
    {
        gameEnded = true;
        PhotonNetwork.player.SetScore(0);
        PhotonNetwork.LeaveRoom();
    }

#region Photon Callbacks

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("WaitingScene");
    }

#endregion
}
