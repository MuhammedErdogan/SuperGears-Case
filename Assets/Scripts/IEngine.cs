using UnityEngine;

namespace Car.Engine
{
    public interface IEngine
    {
        float MaxSpeed { get; }
        float Acceleration { get; }
        float BrakingPower { get; }
        float Deceleration { get; } // Add Deceleration property
        int CurrentGear { get; }
        int NumberOfGears { get; }
        int RPM { get; }
        int MaxRPM { get; } // Add MaxRPM property

        void Accelerate(Rigidbody rb);
        void Brake(Rigidbody rb);
        void Decelerate(Rigidbody rb); // Add Decelerate method
    }
}