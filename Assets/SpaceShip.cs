using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
	Rigidbody rigidBody;
	AudioSource audioSource;

	[SerializeField] float mainThrust = 100;
	[SerializeField] float rcsThrust = 100;

	void Start()
    {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
    }

    void Update()
	{
		Thrust();
		Rotate();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Friendly"))
		{
			Debug.Log("Friendly");
		} else if (collision.gameObject.CompareTag("Finish"))
		{
			Debug.Log("Finish");
		} else
		{
			Debug.Log("Dead");
			// kill de player
		}
	}

	private void Thrust()
	{

		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.Space))
		{
			rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void Rotate()
	{
		rigidBody.freezeRotation = true;

		float rotationThisFrame = rcsThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * rotationThisFrame);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}

		rigidBody.freezeRotation = false; 
	}
}
