using UnityEngine;

public interface IEngine
{
    float MaxSpeed { get; }
    float Acceleration { get; }
    float BrakingPower { get; }

    void Accelerate(Rigidbody rb);
    void Brake(Rigidbody rb);
}