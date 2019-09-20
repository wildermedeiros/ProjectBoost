using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
	Rigidbody rigidBody;
	AudioSource audioSource;

	[SerializeField] float mainThruster = 5f;
	[SerializeField] float rotationThruster = 5f;

	void Start()
    {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
    }

    void Update()
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			rigidBody.AddRelativeForce(Vector3.up * mainThruster * Time.deltaTime);
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		} else
		{
			audioSource.Stop();
		}

		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * rotationThruster * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward * rotationThruster * Time.deltaTime);
		}
	}
}
