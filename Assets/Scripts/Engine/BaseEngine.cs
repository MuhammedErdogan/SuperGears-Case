﻿using UnityEngine;

namespace Engine
{
    public abstract class BaseEngine : IEngine
    {
        #region Properties
        public float MaxSpeed { get; protected set; }
        public float Acceleration { get; protected set; }
        public float BrakingPower { get; protected set; }
        public float Deceleration { get; protected set; }
        public int CurrentGear { get; protected set; }
        public int NumberOfGears { get; protected set; }
        public int MaxRPM { get; protected set; }
        public int RPM { get; protected set; }
        #endregion

        #region Variables
        protected float gearShiftSpeed;
        #endregion

        protected BaseEngine(float maxSpeed, float acceleration, float brakingPower, float deceleration, int numberOfGears, int maxRpm)
        {
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            BrakingPower = brakingPower;
            Deceleration = deceleration;
            NumberOfGears = numberOfGears;
            gearShiftSpeed = maxSpeed / numberOfGears;
            MaxRPM = maxRpm;

            CurrentGear = 1;
        }

        public virtual void Accelerate(Rigidbody rb)
        {
            float speedDelta = Acceleration * Time.deltaTime;
            HandleSpeedChange(rb, speedDelta);
        }

        public virtual void Brake(Rigidbody rb)
        {
            float speedDelta = -BrakingPower * Time.deltaTime;
            HandleSpeedChange(rb, speedDelta);
        }

        public virtual void Decelerate(Rigidbody rb)
        {
            float speedDelta = -Deceleration * Time.deltaTime;
            HandleSpeedChange(rb, speedDelta);
        }

        protected virtual void HandleSpeedChange(Rigidbody rb, float speedDelta)
        {
            float currentSpeed = rb.velocity.magnitude;
            currentSpeed += speedDelta;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, MaxSpeed);

            rb.velocity = rb.transform.forward * currentSpeed;
            ShiftGear(currentSpeed);
            CalculateRPM(currentSpeed);
        }

        protected virtual void ShiftGear(float currentSpeed)
        {
            if (CurrentGear < NumberOfGears && RPM > (MaxRPM - (MaxRPM / NumberOfGears)))
            {
                CurrentGear++;
            }
            else if (CurrentGear > 1 && RPM < (MaxRPM / (NumberOfGears - 1)))
            {
                CurrentGear--;
            }

            EventManager.TriggerEvent(EventManager.OnGearChange, CurrentGear);
        }

        protected virtual void CalculateRPM(float currentSpeed)
        {
            RPM = (int)(currentSpeed * 360) / CurrentGear;
        }
    }
}
