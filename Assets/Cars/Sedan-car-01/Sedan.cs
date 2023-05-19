using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    public class Sedan : BaseCar
    {
        private float turboBoost;

        public override void Initialize()
        {
            base.Initialize();

        }

        public void ActivateTurboBoost()
        {
            // Add the turboBoost to the engine power.
            //engine.AddPower(turboBoost);
        }
    }
}
