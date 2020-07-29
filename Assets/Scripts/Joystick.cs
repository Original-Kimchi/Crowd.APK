using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	public RectTransform rectJoyback;
	public RectTransform rectJoystick;
	public float m_fSpeed = 5.0f;
	public float m_fSqr = 0f;
	public GameObject player;

	private float m_fRadius;
	

	private Vector3 m_vecMove;
	private Vector2 m_vecNormal;

	private bool m_bTouch = false;

	void Start()
	{
		m_fRadius = rectJoyback.rect.width * 0.5f;
	}

	void FixedUpdate()
	{
		if (m_bTouch)
		{
			player.transform.position += m_vecMove;
		}
	}

	void OnTouch(Vector2 vecTouch)
	{
		Vector2 vec = new Vector2(vecTouch.x - rectJoyback.position.x, vecTouch.y - rectJoyback.position.y);

		vec = Vector2.ClampMagnitude(vec, m_fRadius);
		rectJoystick.localPosition = vec;

		m_fSqr = (rectJoyback.position - rectJoystick.position).sqrMagnitude / (m_fRadius * m_fRadius);

		Vector2 vecNormal = vec.normalized;

		m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime * m_fSqr, 0f, vecNormal.y * m_fSpeed * Time.deltaTime * m_fSqr);
		player.transform.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
	}

	public void OnDrag(PointerEventData eventData)
	{
		OnTouch(eventData.position);
		m_bTouch = true;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnTouch(eventData.position);
		m_bTouch = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		rectJoystick.localPosition = Vector2.zero;
		m_bTouch = false;
	}
}
