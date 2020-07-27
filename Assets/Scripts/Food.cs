using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float speed;
    #endregion

    private int givingScore = 100;

    private FoodRotation foodRotation;

    private void Awake()
    {
        foodRotation = gameObject.GetComponent<FoodRotation>();
    }

    private void Update()
    {
        float rotation = transform.rotation.eulerAngles.y;
        transform.position += new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), 0, Mathf.Sin(rotation * Mathf.Deg2Rad)) * speed;
    }

    public int GetScore()
    {
        return givingScore;
    }
}
