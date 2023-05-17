using UnityEngine;

public class StandardEngine : IEngine
{
    public float MaxSpeed { get; private set; }
    public float Acceleration { get; private set; }
    public float BrakingPower { get; private set; }

    public StandardEngine(float maxSpeed, float acceleration, float brakingPower)
    {
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        BrakingPower = brakingPower;
    }

    public void Accelerate(Rigidbody rb)
    {
        var currentSpeed = rb.velocity.magnitude;
        if (currentSpeed <= MaxSpeed)
        {
            currentSpeed += Acceleration * Time.deltaTime;
            rb.velocity = rb.transform.forward * currentSpeed;
            Debug.Log("Current speed: " + currentSpeed);
        }
    }

    public void Brake(Rigidbody rb)
    {
        var currentSpeed = rb.velocity.magnitude;
        if (currentSpeed > 0)
        {
            currentSpeed -= BrakingPower * Time.deltaTime;
            rb.velocity = rb.transform.forward * currentSpeed;
        }
    }
}