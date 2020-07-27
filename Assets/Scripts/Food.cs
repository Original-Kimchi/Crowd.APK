using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Game game;
    private int givingScore = 100;

    private void OnEnable()
    {
        if (game is null) game = Camera.main.GetComponent<Game>();
        transform.position = game.GetEmptyLocation();
    }
    public int GetScore()
    {
        return givingScore;
    }
}
