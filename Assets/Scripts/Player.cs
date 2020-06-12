using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;
    private float size = 1;
    private void Awake()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            PointUp(100);
            ObjectBox.Enqueue(collision.transform);
            Debug.Log("사물 처리");
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Player enemy = collision.transform.GetComponent<Player>();
            if (enemy.CheckScore(score)) // 상대방 점수보다 높을때,상대방 게임 종료
            {
                PointUp(enemy.GetScore());
                enemy.gameObject.SetActive(false);
                transform.localScale *= 5f;
                Debug.Log(gameObject.name + "플레이어가 이김");
            }
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
}
