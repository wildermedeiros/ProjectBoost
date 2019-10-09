using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
	[SerializeField] float mainThrust = 100;
	[SerializeField] float rcsThrust = 100;
	[SerializeField] float timeToTranscend = 1f;
	[SerializeField] float timeToDie = 1f;

	Rigidbody rigidBody;
	AudioSource audioSource;

	enum State { Alive, Dying, Transcending}
	State state = State.Alive;

	void Start()
    {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
    }

    void Update()
	{
		// todo somewhere to stop sound
		if (state == State.Alive)
		{
			Thrust();
			Rotate();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (state != State.Alive) { return; }

		if (collision.gameObject.CompareTag("Friendly"))
		{

		} else if (collision.gameObject.CompareTag("Finish"))
		{
			state = State.Transcending;
			Invoke("LoadNextScene", timeToTranscend);
		}
		else
		{
			state = State.Dying;
			Invoke("ResetScene", timeToDie);
		}
	}

	private void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
