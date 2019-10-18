using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SpaceShip : MonoBehaviour
{
	[SerializeField] float mainThrust = 100;
	[SerializeField] float rcsThrust = 100;
	[SerializeField] float timeToLoadLevel = 1f;
	[SerializeField] float timeToRestartLevel = 1f;
	[SerializeField] Joystick joystick;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip success;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem successParticles;

	Rigidbody rigidBody;
	AudioSource audioSource;
	bool colliderIsOn = true;

	enum State { Alive, Dying, Transcending, Debug}
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

		if (Input.GetKey(KeyCode.L))
		{
			LoadNextScene();
		}

		if (Input.GetKey(KeyCode.C))
		{
			RespondToDebugMode();
		}
	}

	private void RespondToDebugMode()
	{
		if (colliderIsOn)
			colliderIsOn = false;
		else
			colliderIsOn = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!colliderIsOn) { return; }
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

	public void ResetScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}

	public void ApplicationQuit()
	{
		Application.Quit();
	}

	public void RespondToThrustInput()
	{
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (CrossPlatformInputManager.GetButton("Fire1"))
		{
			ApplyThrust(thrustThisFrame);
		}
		else
		{
			audioSource.Stop();
			mainEngineParticles.Stop();
		}
	}

	public void ApplyThrust(float thrustThisFrame)
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
		if (CrossPlatformInputManager.GetButton("Fire2"))
		{
			transform.Rotate(Vector3.forward * rotationThisFrame);
		}
		else if (CrossPlatformInputManager.GetButton("Fire3"))
		{
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}

		rigidBody.freezeRotation = false; 
	}

	public void RotateLeftInputMobile()
	{
		rigidBody.freezeRotation = true;

		float rotationThisFrame = rcsThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * rotationThisFrame);
		}

		rigidBody.freezeRotation = false;
	}

	public void RotateRightInputMobile()
	{
		rigidBody.freezeRotation = true;

		float rotationThisFrame = rcsThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}

		rigidBody.freezeRotation = false;
	}
}
