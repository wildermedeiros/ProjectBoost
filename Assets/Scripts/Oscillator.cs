using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
	[SerializeField] Vector3 movementVector;
	[SerializeField] float period = 2f;

	float movementFactor;
	Vector3 startingPos;

    void Start()
    {
		startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		//if(Mathf.Approximately(period, 0)) { return; }

		if(period <= Mathf.Epsilon) { return; }

		float cycles = Time.time / period; // grows continually from 0 

		const float tau = Mathf.PI * 2f; //  ~ 6,28
		float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1 

		movementFactor = rawSinWave / 2f + 0.5f;

		Vector3 offSet = movementVector * movementFactor;
		transform.position = startingPos + offSet;
    }

    public void SetPeriodForPowerUp(float modifier)
    {
        period *= modifier;
    }

    public void RevertPeriodForPowerUp(float modifier)
    {
        period /= modifier;
    }
}
