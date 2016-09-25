using UnityEngine;
using System.Collections;
public delegate void OnABS_Working();

public class Brake
{
    public event OnABS_Working onABS_Working;
    public float brakeForce;
    public bool hasABS;

    public CarControl m_Car;
    Wheel wheel;
    public Brake(Wheel wheel, float brakeForce, bool hasABS)
    {
        this.wheel = wheel;
        this.brakeForce = brakeForce;
        this.hasABS = hasABS;
    }
    public void doBrake()
    {
        wheel.wheelCollider.motorTorque = 0;
        if (hasABS)
        {
            ABS();
        }
        else
        {
            wheel.wheelCollider.brakeTorque = brakeForce;
        }

    }

    public void ABS()
    {
        if (wheel.wheelCollider.brakeTorque <= 0 || wheel.speedInKmH >= m_Car.speedInKmH
            )
            wheel.wheelCollider.brakeTorque = brakeForce;
        else if (

            wheel.slipRatio >
            wheel.wheelCollider.forwardFriction.extremumSlip
                + (wheel.diffrenceForwardFrictionEextremumSlip / 4f)//0.5f
            && wheel.speedInKmH < m_Car.speedInKmH

            )
        {

            wheel.wheelCollider.brakeTorque *= 0.3f;


            if (onABS_Working != null)
                onABS_Working();

        }

        else if (wheel.slipRatio < 0.38f)
        {
            wheel.wheelCollider.brakeTorque = brakeForce;
        }

    }


    public void releaseBrake()
    {
        wheel.wheelCollider.brakeTorque = 0;
       
    }
}
