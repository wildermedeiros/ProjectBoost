using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
	[SerializeField] float mainThrust = 100;
	[SerializeField] float rcsThrust = 100;
	[SerializeField] float timeToLoadLevel = 1f;
	[SerializeField] float timeToRestartLevel = 1f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip success;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem successParticles;


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
			RespondToThrustInput();
			RespondToRotateInput();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (state != State.Alive) { return; }

		if (collision.gameObject.CompareTag("Friendly"))
		{

		} else if (collision.gameObject.CompareTag("Finish"))
		{
			StartSuccessSequence();
		}
		else
		{
			StartDeathSequence();
		}
	}

	private void StartSuccessSequence()
	{
		state = State.Transcending;
		audioSource.Stop();
		audioSource.PlayOneShot(success);
		mainEngineParticles.Stop();
		successParticles.Play();
		Invoke("LoadNextScene", timeToLoadLevel);
	}

	private void StartDeathSequence()
	{
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(death);
		mainEngineParticles.Stop();
		deathParticles.Play();
		Invoke("ResetScene", timeToRestartLevel);
	}

	private void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	private void RespondToThrustInput()
	{
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.Space))
		{
			ApplyThrust(thrustThisFrame);
		}
		else
		{
			audioSource.Stop();
			mainEngineParticles.Stop();
		}
	}

	private void ApplyThrust(float thrustThisFrame)
	{
		rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
		if (!audioSource.isPlaying)
		{
			audioSource.PlayOneShot(mainEngine);
		}
		mainEngineParticles.Play();
	}

	private void RespondToRotateInput()
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
