using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRotation : MonoBehaviour
{
	[Header("회전 방향")]
	public bool x;
	public bool y = true;
	public bool z;

	[Header("회전 시간")]
	public float timeX = 3f;
	public float timeY = 3f;
	public float timeZ = 3f;

	private float rotationX = 360f;
	private float rotationY = 360f;
	private float rotationZ = 360f;

	private float angleX;
	private float angleY;
	private float angleZ;

	private void Start()
	{
		StartCoroutine(ChangeRotation());
	}

	private void Update()
	{
		if (x) angleX += (rotationX / timeX) * Time.deltaTime;
		if (y) angleY += (rotationY / timeY) * Time.deltaTime;
		if (z) angleZ += (rotationZ / timeZ) * Time.deltaTime;

		transform.localRotation = Quaternion.Euler(angleX, angleY, angleZ);
	}

	private IEnumerator ChangeRotation()
	{
		while (gameObject.activeSelf)
		{
			yield return new WaitForSeconds(timeY);
			rotationY = Random.Range(-180f, 180f);
		}
	}
}