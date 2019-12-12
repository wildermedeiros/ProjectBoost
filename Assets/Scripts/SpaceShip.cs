using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SpaceShip : MonoBehaviour
{
    public static SpaceShip instance;

	[SerializeField] float mainThrust = 100;
	[SerializeField] float rcsThrust = 100;
	[SerializeField] float timeToLoadLevel = 1f;
	[SerializeField] float timeToRestartLevel = 1f;
	[SerializeField] bool pcTest = true;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip success;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem successParticles;

	Rigidbody rigidBody;
	AudioSource audioSource;
	bool colliderIsOn = true;
	bool isTransitioning = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
    }

    void Update()
	{
		if (!isTransitioning)
		{
			if (pcTest)
			{
                RespondToThrustInputPC();
                RespondToRotateInputPC();
			} else 
			RespondToThrustInputMobile();
			RespondToRotateInputMobile();
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
		if (isTransitioning) { return; }

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
		isTransitioning = true;
		audioSource.Stop();
		audioSource.PlayOneShot(success);
		mainEngineParticles.Stop();
		successParticles.Play();
		Invoke("LoadNextScene", timeToLoadLevel);
	}

	private void StartDeathSequence()
	{
		isTransitioning = true;
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
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;
		if (SceneManager.sceneCountInBuildSettings == nextSceneIndex)
		{
			nextSceneIndex = 0; // loop back to start 
		}
		SceneManager.LoadScene(nextSceneIndex);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}

	public void ApplicationQuit()
	{
		Application.Quit();
	}

	public void RespondToThrustInputMobile() // mobile 
	{
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (CrossPlatformInputManager.GetButton("Fire1"))
		{
			ApplyThrust(thrustThisFrame);
		}
		else
		{
			StopApllyingThrust();
		}
	}

    public void RespondToThrustInputPC()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            StopApllyingThrust();
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

    private void StopApllyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

	private void RespondToRotateInputMobile()
	{
		float rotationThisFrame = rcsThrust * Time.deltaTime;
		if (CrossPlatformInputManager.GetButton("Fire2"))
        {
            RotateManually(rcsThrust * Time.deltaTime);
        }
        else if (CrossPlatformInputManager.GetButton("Fire3"))
		{
          RotateManually(-rcsThrust * Time.deltaTime);
		}
	}

    private void RespondToRotateInputPC()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rcsThrust * Time.deltaTime);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false;
    }
}
