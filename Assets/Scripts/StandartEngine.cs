using UnityEngine;

public class StandardEngine : BaseEngine
{
    public StandardEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears)
    {
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        BrakingPower = brakingPower;
        Deceleration = deceleration;
        NumberOfGears = numberOfGears;
        gearShiftSpeed = maxSpeed / numberOfGears;
    }
}

public class ElectricalEngine : BaseEngine
{
    private float _electiricity;

    public ElectricalEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears, float electiricity)
    {
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        BrakingPower = brakingPower;
        Deceleration = deceleration;
        NumberOfGears = numberOfGears;
        gearShiftSpeed = maxSpeed / numberOfGears;
        _electiricity = electiricity;
    }
}

public class TurboEngine : BaseEngine
{
    private float _turboBoost;

    public TurboEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears, float turboBoost)
    {
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        BrakingPower = brakingPower;
        Deceleration = deceleration;
        NumberOfGears = numberOfGears;
        gearShiftSpeed = maxSpeed / numberOfGears;
        _turboBoost = turboBoost;
    }
}
