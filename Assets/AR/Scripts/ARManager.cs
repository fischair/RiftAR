using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ARManager : MonoBehaviour {
	
	// List of all connectes webcams
	WebCamDevice[] webcamDevices;
	WebCamTexture webcamLeft;
	WebCamTexture webcamRight;

	// The static webcam parameters for 2 identical webcams
	// !!! Be carefull with the USB Ports you use, when using a full-hd resolution
	public static int webcamPixelHeight = 720;
	public static int webcamPixelWidtht = 1280;

	// The static FOV of the webcams
	public static int webcamFovHorizontal = 0;
	public static int webcamFovVertical = 0;

	// TODO find better solution
	// Reference to the HUD
	public Canvas hud;
	public Text hudPosition;
	public Text hudTracking;
	public Camera oneCamera;

	// Use this for initialization
	void Start () {
		
		// Test if ARManager is attached to the OculusRift GameObject
		if (this.name.Equals("OculusRift"))
			Debug.Log ("Correct GameObject");
		
		setupWebcams ();

		// Disable Hud at start
		hud.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

		/*********************** Buttons ***************************/
		// increase height of OculusRift
		if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
			Vector3 position = this.transform.position;
			position.y += 0.1f;
			this.transform.position = position;
		}
		// decrease height of OculusRift
		if (Input.GetKeyDown (KeyCode.KeypadMinus)) {
			Vector3 position = this.transform.position;
			position.y -= 0.1f;
			this.transform.position = position;
		}
		// switch both webcams
		if (Input.GetKeyDown (KeyCode.S)) {
			switchWebcams ();
		}
		// print info to Log
		if (Input.GetKeyDown (KeyCode.I)) {
			Debug.Log ("Resolution of the left webcam = " + webcamLeft.width + " x " + webcamLeft.height);
			Debug.Log ("Resolution of the right webcam = " + webcamRight.width + " x " + webcamRight.height);
		}
		// show HUD
		if (Input.GetKeyDown (KeyCode.H)) {
			if (!hud.enabled) {
				hud.enabled = true;
				Debug.Log ("HUD enabled");
			} else {
				hud.enabled = false;
				Debug.Log ("HUD disabled");
			}
		}

		// recenter
		if (Input.GetKeyDown (KeyCode.Space)) {
			OVRManager.display.RecenterPose();
		}

		/************************ Buttons end **************************/

		// update HUD
		if (hud.enabled == true) {
			//update Tracker Status
			if (OVRManager.tracker.isPositionTracked) {
				hudTracking.text = "Pos. Tracking: ON";
			} else {
				hudTracking.text = "Pos. Tracking: OFF";
			}
			//update Position
			hudPosition.text = "Position: " + Math.Round (oneCamera.transform.position.x, 2) + 
				":" + Math.Round (oneCamera.transform.position.y, 2) + 
				":" + Math.Round (oneCamera.transform.position.z + 10, 2);
		}
	

	}
	void setupWebcams() {
		
		// Get connected webcams
		webcamDevices = WebCamTexture.devices;
		foreach (WebCamDevice device in webcamDevices)
			Debug.Log ("Webcam " + " = " + device.name);

		// Setup the BackgroundVideo
		webcamLeft = new WebCamTexture (webcamDevices [0].name);
		webcamLeft.requestedHeight = webcamPixelHeight;
		webcamLeft.requestedWidth = webcamPixelWidtht;
		webcamRight = new WebCamTexture (webcamDevices [1].name);
		webcamRight.requestedHeight = webcamPixelHeight;
		webcamRight.requestedWidth = webcamPixelWidtht;

		GameObject.Find("QuadRightEye").GetComponent<Renderer>().material.mainTexture = webcamRight;
		GameObject.Find("QuadLeftEye").GetComponent<Renderer>().material.mainTexture = webcamLeft;
		webcamRight.Play();
		webcamLeft.Play();
	}
	
	void switchWebcams() {
		
		webcamLeft.Stop();
		webcamRight.Stop();
		
		WebCamTexture tmpWebcam = webcamLeft;
		webcamLeft = webcamRight;
		webcamRight = tmpWebcam;
		
		GameObject.Find("QuadRightEye").GetComponent<Renderer>().material.mainTexture = webcamRight;
		GameObject.Find("QuadLeftEye").GetComponent<Renderer>().material.mainTexture = webcamLeft;
		webcamLeft.Play();
		webcamRight.Play();
	}
}
