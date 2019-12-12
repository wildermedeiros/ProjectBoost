using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using TMPro;

public class Snow : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioClip powerUpSound;
    [SerializeField] float countDown = 45f;

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
        if (CrossPlatformInputManager.GetButtonDown("Fire6"))
        {
            PlaySoundEffect();
            PlayVisualEffect();
            ProcessPowerUp(oscilator);
            ResetCountDown();
        }
    }

    private void ProcessPowerUp(Oscillator[] oscilator)
    {
        foreach (var oscilatorItem in oscilator)
        {
            oscilatorItem.SetPeriodForPowerUp(0); // serialize this field 
            oscilatorItem.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.blue);
        }
        // todo a habilidade irá durar por quanto tempo?
        isPowerUpTriggered = true;
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
