using UnityEngine;

namespace Engine.Car
{
    public class StandardEngine : BaseEngine
    {
        public StandardEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears, int maxRpm) :
            base(maxSpeed, acceleration, brakingPower, deceleration, numberOfGears, maxRpm)
        {

        }
    }

    public class ElectricalEngine : BaseEngine
    {
        private float _electiricity;

        public ElectricalEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears, int maxRpm, float electiricity) :
            base(maxSpeed, acceleration, brakingPower, deceleration, numberOfGears, maxRpm)
        {
            _electiricity = electiricity;
        }
    }

    public class TurboEngine : BaseEngine
    {
        private float _turboBoost;

        public TurboEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears, int maxRpm, float turboBoost) :
            base(maxSpeed, acceleration, brakingPower, deceleration, numberOfGears, maxRpm)
        {
            _turboBoost = turboBoost;
        }
    }
}
