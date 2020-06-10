using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;

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
            if (CheckScore(score))
                Debug.Log(gameObject.name + "플레이어가 이김");
        }
    }

    private void PointUp(int _score)
    {
        score += _score;
    }

    public bool CheckScore(int enemyScore)
    {
        if (score > enemyScore) return true;
        else return false;
    }
}
