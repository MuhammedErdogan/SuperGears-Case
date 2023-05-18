using UnityEngine;

public abstract class BaseEngine : IEngine
{
    public float MaxSpeed { get; protected set; }
    public float Acceleration { get; protected set; }
    public float BrakingPower { get; protected set; }
    public float Deceleration { get; protected set; }
    public int CurrentGear { get; protected set; }
    public int NumberOfGears { get; protected set; }
    public float RPM { get; protected set; }

    protected float gearShiftSpeed;

    public virtual void Accelerate(Rigidbody rb)
    {
        var currentSpeed = rb.velocity.magnitude;
        if (currentSpeed > MaxSpeed)
        {
            return;
        }

        currentSpeed += Acceleration * Time.deltaTime;
        rb.velocity = rb.transform.forward * currentSpeed;
    }

    public virtual void Brake(Rigidbody rb)
    {
        var currentSpeed = rb.velocity.magnitude;
        if (currentSpeed <= 0)
        {
            return;
        }

        currentSpeed -= BrakingPower * Time.deltaTime;
        rb.velocity = rb.transform.forward * currentSpeed;
    }

    public virtual void Decelerate(Rigidbody rb)
    {
        var currentSpeed = rb.velocity.magnitude;
        if (currentSpeed <= 0)
        {
            return;
        }

        currentSpeed -= Deceleration * Time.deltaTime;
        rb.velocity = rb.transform.forward * currentSpeed;
    }

    protected virtual void ShiftGear(float currentSpeed)
    {
        if (CurrentGear < NumberOfGears && currentSpeed >= gearShiftSpeed * CurrentGear)
        {
            CurrentGear++;
            Debug.Log("Gear Shifted Up: " + CurrentGear);
        }
        else if (CurrentGear > 1 && currentSpeed < gearShiftSpeed * (CurrentGear - 1))
        {
            CurrentGear--;
            Debug.Log("Gear Shifted Down: " + CurrentGear);
        }
    }

    protected virtual void CalculateRPM(float currentSpeed)
    {
        RPM = currentSpeed * CurrentGear * 60;
    }
}