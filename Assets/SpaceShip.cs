using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
	Rigidbody rigidBody;

	[SerializeField ]float speed = 20f;

	void Start()
    {
		rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			Debug.Log("Space pressed");
		}

		if (Input.GetKey(KeyCode.A))
		{
			Debug.Log("Rotating Left");
		}
		else if (Input.GetKey(KeyCode.D))
		{
			Debug.Log("Rotating Right");
		}
	}
}
