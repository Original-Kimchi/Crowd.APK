using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : PhotonViewObject
{
    private GameManager gameManager;
    private int givingScore = 100;

    private void OnEnable()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (gameManager is null) gameManager = Camera.main.GetComponent<GameManager>();
            transform.position = gameManager.GetEmptyLocation();
        }
    }

    public int GetScore()
    {
        return givingScore;
    }
}
