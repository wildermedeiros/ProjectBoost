using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using TMPro;
using System;

public class SlowTime : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioClip powerUpSound;
    [SerializeField] float countDown = 45f;
    [SerializeField] float slowDownFactor = 0.02f;
    [SerializeField] float slowDownLength = 5f;

    Oscillator[] oscilator;
    AudioSource audioSource;
    Button button;
    TextMeshProUGUI countDownText;

    float timer = 0f;
    bool isPowerUpTriggered = false;
    bool doOnce = false;
    bool canCountDown = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        countDownText = GetComponentInChildren<TextMeshProUGUI>();
        oscilator = FindObjectsOfType<Oscillator>();

        timer = countDown;
        countDownText.enabled = false;
    }

    private void Update()
    {
        TriggerPowerUp();
        CountDownVerify();
        SlowMotionHandler();
    }

    public void TriggerPowerUp()
    {
        if (isPowerUpTriggered) { return; }

        if (oscilator.Length <= 0)
        {
            button.interactable = false;
            return;
        }
        RespondToInput();
    }

    private void RespondToInput()
    {
        if (CrossPlatformInputManager.GetButton("Fire4"))
        {
            PlaySoundEffect();
            PlayVisualEffect();
            ProcessPowerUp(oscilator);
            ResetCountDown();
        }
    }

    private void ProcessPowerUp(Oscillator[] oscilator)
    {
        isPowerUpTriggered = true;

        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f; //  50 vezes por segundo 1/50 
    }

    private void PlaySoundEffect()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(powerUpSound);
    }

    private void PlayVisualEffect()
    {
        Instantiate(particles, SpaceShip.instance.transform.position, Quaternion.identity);
    }

    private void ResetCountDown()
    {
        timer = countDown;
        canCountDown = true;
        doOnce = false;
    }

    private void SlowMotionHandler()
    {
        //Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        //Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    void CountDownVerify()
    {
        if (!isPowerUpTriggered) { return; }

        if (timer >= Mathf.Epsilon && canCountDown)
        {
            countDownText.enabled = true;
            timer -= Time.deltaTime;
            countDownText.text = timer.ToString("0");
        }
        else if (timer <= Mathf.Epsilon && !doOnce)
        {
            countDownText.enabled = false;
            canCountDown = false;
            doOnce = true;
            isPowerUpTriggered = false;
        }
    }
}
