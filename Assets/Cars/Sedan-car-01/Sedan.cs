using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sedan : BaseCar
{
    private float turboBoost;

    public override void Initialize(DependencyContainer container)
    {
        base.Initialize(container);

    }

    public void ActivateTurboBoost()
    {
        // Add the turboBoost to the engine power.
        //engine.AddPower(turboBoost);
    }
}
