using UnityEngine;
using System.Collections;

public class Car1: MonoBehaviour{
		public WheelCollider[] frontCols;
		public Transform[] dataFront;
		public WheelCollider[] backCol;
		public Transform[] dataBack;
		public Transform centerOfMass;
		
		public float maxSpeed = 30f;
		public float sideSpeed = 30f;
		public float breakSpeed = 350f;/*tormojenie*/
		
		public Sound sound;
		
		void Start () {
			GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
			sound = gameObject.GetComponent<Sound>();
		}
		
		void Update () {
			float vAxis = Input.GetAxis ("Vertical");
			float hAxis = Input.GetAxis ("Horizontal");
			bool brakeButton = Input.GetButton("Jump");
			
			/*   Motor   */
			frontCols[0].motorTorque = vAxis * maxSpeed;
			frontCols [1].motorTorque = vAxis * maxSpeed;
			
			if (brakeButton) {
			frontCols[0].brakeTorque = Mathf.Abs(frontCols [0].motorTorque) * breakSpeed;
			frontCols[1].brakeTorque = Mathf.Abs(frontCols [1].motorTorque) * breakSpeed;
			}
			else {
				frontCols[0].brakeTorque = 0;
				frontCols[1].brakeTorque = 0;
			}
			
			/*rotate cols*/
			frontCols [0].steerAngle = hAxis * sideSpeed;
			frontCols [1].steerAngle = hAxis * sideSpeed;
			
			/*graphic cols updation*/
			dataFront [0].Rotate(0, 0, -frontCols [0].rpm * Time.deltaTime);
			dataFront [1].Rotate(0, 0, -frontCols [1].rpm * Time.deltaTime);
			dataBack [0].Rotate(0, 0, -backCol [0].rpm * Time.deltaTime);
			dataBack [1].Rotate(0, 0, -backCol [1].rpm * Time.deltaTime);
			dataFront [0].localEulerAngles = new Vector3(dataFront [0].localEulerAngles.x, hAxis * sideSpeed, dataFront [0].localEulerAngles.z);
			dataFront [0].localEulerAngles = new Vector3(dataFront [1].localEulerAngles.x, hAxis * sideSpeed, dataFront [1].localEulerAngles.z);
			
			/*Skid*/
			WheelHit hit;
			if (backCol [0].GetGroundHit (out hit)){
				float vol = (Mathf.Abs(hit.sidewaysSlip) > .25f)?hit.sidewaysSlip/5:0;
				sound.playSkid (vol * 2f);
			}
		}
			
			
}
 