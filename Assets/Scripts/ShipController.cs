using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float thrustPower = 10f;
    public float maxSpeed = 100f; 
    public float rotationSpeed = 90f;
    public float rollSpeed = 90f;
    public float rollOnYawFactor = 0.5f;
    public float rollOnYawSpeed = 3f;
    public float boostMultiplier = 2f;
    public float maxMouse = 1f;
    public float dampingValue = 10f; // Значення демпінгу
    private float currentThrust = 0f;

    public ParticleSystem[] thrusterEffects;
    private Rigidbody rb;
    private float currentRoll = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        HandleThrust();
        HandleRotation();
        HandleDamping();
        ClampSpeed();
    }

    private void HandleRotation()
    {
        float pitch = Mathf.Clamp(Input.GetAxis("Mouse Y"), -maxMouse, maxMouse) * rotationSpeed * Time.deltaTime;
        float yaw = Mathf.Clamp(Input.GetAxis("Mouse X"), -maxMouse, maxMouse) * rotationSpeed * Time.deltaTime;

        float rollInput = (Input.GetKey(KeyCode.A) ? 1 : 0) + (Input.GetKey(KeyCode.D) ? -1 : 0);
        float targetRoll = -yaw * rollOnYawFactor;
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, Time.deltaTime * rollOnYawSpeed);

        transform.Rotate(-pitch, yaw, (rollInput * rollSpeed + currentRoll) * Time.deltaTime, Space.Self);
    }

    private void HandleThrust()
    {
       // bool isThrusting = Input.GetKey(KeyCode.W);
      //  bool isReversing = Input.GetKey(KeyCode.S);

        if (Input.GetKey(KeyCode.W))
        {

            //rb.AddForce(transform.forward * thrustPower);
            currentThrust = thrustPower * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f);
            ApplyThrust();
            SetThrusterEffects(true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentThrust = -thrustPower / 2;
            ApplyThrust();
            SetThrusterEffects(false);
        }
        else
        {
         //   currentThrust = 0f;
            SetThrusterEffects(false);
        }
    }

    private void ApplyThrust()
    {
        rb.AddForce(transform.forward * currentThrust);
    }

    private void SetThrusterEffects(bool state)
    {
        foreach (ParticleSystem thruster in thrusterEffects)
        {
            if (state && !thruster.isPlaying)
                thruster.Play();
            else if (!state && thruster.isPlaying)
                thruster.Stop();
        }
    }

    private void HandleDamping()
    {
        rb.linearDamping = Input.GetKey(KeyCode.LeftControl) ? 0 : dampingValue;
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
