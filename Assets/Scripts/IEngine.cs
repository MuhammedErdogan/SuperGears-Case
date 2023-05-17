using UnityEngine;

public interface IEngine
{
    float MaxSpeed { get; }
    float Acceleration { get; }
    float BrakingPower { get; }

    void Accelerate(float currentSpeed, Rigidbody rb);
    void Brake(float currentSpeed, Rigidbody rb);
}