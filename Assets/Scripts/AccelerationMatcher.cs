using UnityEngine;

namespace Bose.Wearable
{
	
	[AddComponentMenu("Bose/Wearable/AccelerationMatcher")]
    public class AccelerationMatcher : MonoBehaviour
	{
        public bool interpolation = false;

        public float relativeSpeed = 1f;


        private Vector3 lastFrameAcceleration = new Vector3(0,0,0);

		private WearableControl _wearableControl;


		private void Awake()
		{
			// Begin in absolute mode and cache the wearable controller.
			_wearableControl = WearableControl.Instance;


			// Establish a requirement for the rotation sensor
			WearableRequirement requirement = GetComponent<WearableRequirement>();
			if (requirement == null)
			{
				requirement = gameObject.AddComponent<WearableRequirement>();
			}

			requirement.EnableSensor(SensorId.Accelerometer);
            requirement.SetSensorUpdateInterval(SensorUpdateInterval.EightyMs);
		}

		private void Update()
		{
			if (_wearableControl.ConnectedDevice == null)
			{
				return;
			}

            // Get a frame of sensor data. Since no integration is being performed, we can safely ignore all
            // intermediate frames and just grab the most recent.

            SensorFrame frame = _wearableControl.LastSensorFrame;
            Vector3 newAcceleration = frame.acceleration;

            if (lastFrameAcceleration != new Vector3(0,0,0))
            {
                Vector3 diff = lastFrameAcceleration - newAcceleration;
                lastFrameAcceleration = newAcceleration;

                
                if (interpolation)
                {
                    transform.position = Vector3.Lerp(transform.position, diff, Time.deltaTime * relativeSpeed);
                }
                else
                {
                    transform.position = diff*relativeSpeed;
                }
                


            }
            else{
                lastFrameAcceleration = newAcceleration;

            }
		}

	}
}
