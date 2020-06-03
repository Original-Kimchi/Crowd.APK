using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int score;

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Food"))
        {
            
        }
    }

    public void PointUp(int _score)
    {
        score += _score;
    }
}
