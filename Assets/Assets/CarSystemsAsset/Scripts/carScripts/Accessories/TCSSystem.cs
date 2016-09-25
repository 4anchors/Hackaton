using UnityEngine;
public class TCSSystem : MonoBehaviour
{

    [Range(0f, 1)]
    [SerializeField]
    public float m_TractionControl = 1; // 0 is no traction control, 1 is full interference

    private Car car;
    private CarControl mCar;

    void Start()
    {
        car = GetComponent<Car>();
        mCar = car.GetComponent<CarControl>();

    }


    float forwardSlip;

    public void applyTCS(float motorTorque)
    {

        wheel.applyMotorTorque(wheel.wheelCollider.motorTorque * 1.9f);
    }

    Wheel wheel;
    bool is_forward_slip;
    public void wheelsTractionSystem(float motorTorque)
    {
        for (int i = 0; i < car.motorWheelsCount; i++)
        {
            wheel = car.motorWheels[i];

            if (wheel.speedInKmH <= mCar.speedInKmH || wheel.wheelCollider.motorTorque == 0)
            {
                wheel.applyMotorTorque(motorTorque);
                continue;
            }

            is_forward_slip = wheel.slipRatio >
                wheel.wheelCollider.forwardFriction.extremumSlip
                +
                (wheel.diffrenceForwardFrictionEextremumSlip / (2.5f * m_TractionControl));// 0.55f;

            if (is_forward_slip)
            {
                applyTCS(motorTorque);
                wheel.setTCS_ON();
            }
            else
            {

                if (wheel.slipRatio < wheel.wheelCollider.forwardFriction.extremumSlip - 0.1f
                    && wheel.wheelCollider.motorTorque > 0
                    && wheel.wheelCollider.motorTorque * 2f < motorTorque * wheel.currentengineTorquePercen)
                {
                    wheel.wheelCollider.motorTorque *= 2f;

                }
                else wheel.applyMotorTorque(motorTorque);
                wheel.setTCS_OFF();
            }
        }

    }
    float tcsValue = 0;
    public void deactivateTCS()
    {
        tcsValue = m_TractionControl;
        m_TractionControl = 0;
        car.accessories.epsSystem.giveBackTorque();
        car.accessories.disableEPS();
    }

    public void activateTCS()
    {
        m_TractionControl = tcsValue;
        car.accessories.enableEPS();
    }

}
