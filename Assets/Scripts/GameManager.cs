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
    [SerializeField] private List<string> ranking;
    [SerializeField] private Text timeText;
    [SerializeField] private Text playerID;
    [SerializeField] private Transform idBox;
    private Text[] playerIDList = null;
    //GameOver
    [SerializeField] private Image gameoverBackground;
    [SerializeField] private Text gameResult;

    private float time;
    private GameObject[] players = null;  // Player Objects

    private readonly int mapSize = 500;

    private void Awake()
    {
        MyPlayer = PhotonNetwork.Instantiate("Player", GetEmptyLocation(), Quaternion.identity, 0).GetComponent<Player>();
        players = GameObject.FindGameObjectsWithTag("Player");
        if (PhotonNetwork.isMasterClient)
        {
            foreach (var _ in Enumerable.Range(1, 50))
            {
                var go = PhotonNetwork.Instantiate("Food", GetEmptyLocation(), Quaternion.identity, 0);
                go.AddComponent<Food>();
                go.AddComponent<FoodRotation>();
            }
        }
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
        while (ObjectBox.ObjectExist)
            ObjectBox.Dequeue();

        for(int i = 0; i < players.Length; i++)
        {
            if (!players[i].activeSelf)
                playerIDList[i].gameObject.SetActive(false);
            playerIDList[i].transform.position = Camera.main.WorldToScreenPoint(players[i].transform.position) + (Vector3.up * 30f);
        } 
        time -= Time.deltaTime * 10;
        if (time <= 0)
            StartCoroutine(GameOver("time"));

        timeText.text = "Time: " + ((int)time).ToString();
        score.text = "Score: " + MyPlayer.GetScore().ToString();
    }

    public IEnumerator GameOver(string result)
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
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
}
