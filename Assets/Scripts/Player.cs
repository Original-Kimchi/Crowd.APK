using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;
    private float size = 1;
    
	public Movement movement;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    private void Start()
    {
        if (gameManager.MyPlayer.gameObject != gameObject) movement.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Food food = collision.transform.GetComponent<Food>();
            PointUp(food.GetScore());
            ObjectBox.Enqueue(food.transform);
            Debug.Log("사물 처리");
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Player enemy = collision.transform.GetComponent<Player>();
            if (enemy.CheckScore(score)) // 상대방 점수보다 높을때,상대방 게임 종료
            {
                StartCoroutine(SizeUp(5));
				movement.SpeedUp();
                PointUp(enemy.GetScore());
                enemy.gameObject.SetActive(false);
                Debug.Log(gameObject.name + "플레이어가 이김");
            }
        }
		else if (collision.gameObject.CompareTag("Respawn"))
		{
			transform.position += new Vector3(0, 15, 0);
			Debug.Log("지상으로 재 스폰");
		}
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
}
