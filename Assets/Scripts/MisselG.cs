using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using TMPro;

public class MisselG : MonoBehaviour
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

        //if (oscilator.Length <= 0)
        //{
        //    //button.interactable = false;
        //    return;
        //}

        RespondToInput();
    }

    private void RespondToInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire5"))
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
        //particles.gameObject.SetActive(true);
    }

    private void PlaySoundEffect()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(powerUpSound);
    }

    private void PlayVisualEffect()
    {
        SpaceShip spaceShip = SpaceShip.instance;
        //float newXRotation = 20f;
        //Quaternion newRotation = new Quaternion(spaceShip.transform.rotation.x + newXRotation, 
        //    spaceShip.transform.rotation.y, spaceShip.transform.rotation.z, spaceShip.transform.rotation.w);
        
        // todo apenas colocar o colisor da particula pra colider com a layer do obstacle

        Instantiate(particles, spaceShip.transform.position, spaceShip.transform.rotation);
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
            //particles.gameObject.SetActive(false);
        }
    }
}
