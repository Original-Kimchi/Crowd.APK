using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField] Camera camera;
	[SerializeField] float speed;
	private Vector3 normalPos;
	private Vector3 mousePos;

	public (float x, float y) map(float x, float y, float inMinX, float inMinY,
	float inMaxX, float inMaxY, float outMinX, float outMinY, float outMaxX, float outMaxY)
	{
		var returnX = (x - inMinX) * (outMaxX - outMinX) / (inMaxX - inMinX) + outMinX;
		var returnY = (y - inMinY) * (outMaxY - outMinY) / (inMaxY - inMinY) + outMinY;

		return (returnX, returnY);
	}

	void Start()
    {
        
    }

    void Update()
    {
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		// Debug.Log(mousePos);
	}

	private void FixedUpdate()
	{
		(var x, var y) = map(mousePos.x, mousePos.y, 0, 0, 1, 1, -1, -1, 1, 1);
		// Debug.Log("x : " + x);
		// Debug.Log("y : " + y);

		normalPos = new Vector3(x, 0f, y).normalized;
		transform.position += normalPos * speed;
	}
}
