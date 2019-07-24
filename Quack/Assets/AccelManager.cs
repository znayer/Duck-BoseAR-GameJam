using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bose.Wearable;

public class AccelManager : MonoBehaviour {

	private Vector3 lastFrameAcceleration = new Vector3(0,0,0);

	private WearableControl _wearableControl;

	public Transform head;

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
	
	// Update is called once per frame
	void Update () {
		if (_wearableControl.ConnectedDevice == null)
		{
			return;
		}

        // Get a frame of sensor data. Since no integration is being performed, we can safely ignore all
        // intermediate frames and just grab the most recent.

        SensorFrame frame = _wearableControl.LastSensorFrame;
        Vector3 frameDelta = frame.acceleration;

        transform.rotation = head.rotation;

        //frameDelta += transform.rotation * (-Vector3.up * 9.8f);
        if (frameDelta.magnitude > 8f){
        	print(frameDelta);
        }
	}
}
