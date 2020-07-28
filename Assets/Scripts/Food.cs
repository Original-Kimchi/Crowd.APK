using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private GameManager gameManager;
    private int givingScore = 100;

    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
            if (gameManager is null) gameManager = Camera.main.GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        if (PhotonNetwork.isMasterClient) transform.position = gameManager.GetEmptyLocation();
    }

    public int GetScore()
    {
        return givingScore;
    }
}
