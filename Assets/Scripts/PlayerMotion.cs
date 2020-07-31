using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotion : MonoBehaviour
{
	public Joystick fSqr;
	private Animator animator;

    void Start()
    {
        fSqr = GameObject.Find("UI/Panel/Joystick").GetComponent<Joystick>();
		animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log("조이스틱" + fSqr.m_fSqr);
		
		if (fSqr.m_fSqr < 0.1f) animator.SetInteger("animation", 0);
		else if (fSqr.m_fSqr < 0.33f) animator.SetInteger("animation", 1);
		else animator.SetInteger("animation", 2);
    }
}
