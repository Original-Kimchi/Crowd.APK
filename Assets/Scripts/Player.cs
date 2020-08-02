using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : PhotonViewObject, IPunObservable
{
    [SerializeField] private int score;
    private float size = 1;
    
    public int PlayerId { get; private set; }

	// public Movement movement;
    private GameManager gameManager;
    private Vector3 originCamPos;
    private bool isMyPlayer;
    private void Awake()
    {
        base.Awake();
        gameManager = Camera.main.GetComponent<GameManager>();
        PlayerId = PhotonNetwork.player.ID;
    }

    private void Start()
    {
        if (gameManager.MyPlayer.gameObject == gameObject)
        {
            originCamPos = Camera.main.transform.position;
            isMyPlayer = true;
        }
        //if (gameManager.MyPlayer.gameObject != gameObject) movement.enabled = false;
    }
    private void Update()
    {
        if(isMyPlayer)
            Camera.main.transform.position = transform.position + originCamPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.isMasterClient)
        {
            var playerList = PhotonNetwork.playerList.ToList();
            if (collision.gameObject.CompareTag("Food"))
            {
                Food food = collision.transform.GetComponent<Food>();
                PointUp(food.GetScore());
                var player = playerList.Find(p => p.ID == PlayerId);
                player.AddScore(food.GetScore());
                food.transform.position = gameManager.GetEmptyLocation();
                Debug.Log("사물 처리");
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                Player enemy = collision.transform.GetComponent<Player>();
                if (enemy.CheckScore(score)) // 상대방 점수보다 높을때,상대방 게임 종료
                {
                    StartCoroutine(SizeUp(5));
                    // movement.SpeedUp();
                    PointUp(enemy.GetScore());
                    var enemyPlayer = playerList.Find(p => p.ID == enemy.PlayerId);
                    playerList.Find(p => p.ID == PlayerId).AddScore(enemyPlayer.GetScore());
                    enemy.PhotonView.RPC(nameof(GameOver), enemyPlayer);
                    Debug.Log(gameObject.name + "플레이어가 이김");
                }
            }
            else if (collision.gameObject.CompareTag("Respawn"))
            {
                transform.position += new Vector3(0, 15, 0);
                Debug.Log("지상으로 재 스폰");
            }
        }
    }

    private void OnDisable()
    {
        if(isMyPlayer)
            StartCoroutine(gameManager.GameOver("gameover"));
    }

    private void PointUp(int _score)
    {
        score += _score;
    }

    public bool CheckScore(int enemyScore)
    {
        if (score < enemyScore) return true;
        else return false;
    }

    public int GetScore()
    {
        return score;
    }

    private IEnumerator SizeUp(int n)
    {
        WaitForEndOfFrame frame = new WaitForEndOfFrame();
        float time = 0;
        while (time <= 1)
        {

            transform.localScale *= Mathf.Lerp(1, 1.3f, Time.deltaTime);
            time += Time.deltaTime;
            yield return frame;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
            stream.SendNext(PlayerId);
        else
            PlayerId = (int)stream.ReceiveNext();
    }

    [PunRPC]
    public void GameOver()
    {
        gameManager.StartCoroutine(gameManager.GameOver("gameover"));
        PhotonNetwork.Destroy(gameObject);
    }
}
