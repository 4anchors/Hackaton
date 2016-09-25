using UnityEngine;
using System.Collections;

public class EPSSystem : MonoBehaviour
{

    CarControl m_Car;

    void Start()
    {
        m_Car = GetComponent<CarControl>();
    }
    void FixedUpdate()
    {

        giveBackTorque();
        if (m_Car.speedInKmH < 30)
            return;
        if (m_Car.statuse == CarStause.Braking)
            return;
        if (m_Car.car.steering.m_SteerAngle > 0)
        {
            brakeInnerWheels(m_Car.car.rightWheels);
            addMoreTorque(m_Car.car.rightWheels, m_Car.car.leftWheels);
        }
        else if (m_Car.car.steering.m_SteerAngle < 0)
        {
            brakeInnerWheels(m_Car.car.leftWheels);
            addMoreTorque(m_Car.car.leftWheels, m_Car.car.rightWheels);
        }
    }

    void brakeInnerWheels(Wheel[] wheels)
    {
        for (int i = 0; i < 2; i++)
        {
            wheels[i].wheelCollider.brakeTorque = 1 * m_Car.getSpeedFactor();
        }
    }

    void addMoreTorque(Wheel[] fromWheels, Wheel[] toWheels)
    {
        for (int i = 0; i < 2; i++)
        {
            fromWheels[i].currentengineTorquePercen = fromWheels[i].engineTorquePercent - 0.1f;
            toWheels[i].currentengineTorquePercen = toWheels[i].engineTorquePercent + 0.1f;
        }

    }
    public void giveBackTorque()
    {
        for (int i = 0; i < 4; i++)
        {
            m_Car.car.wheels[i].currentengineTorquePercen = m_Car.car.wheels[i].engineTorquePercent;
        }
    }
}
