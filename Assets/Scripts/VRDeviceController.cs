using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class VRDeviceController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!Application.isEditor)
        {
            if (GameVariables.Device == "Magic Window") StartCoroutine(SetVRDevice("Cardboard", false));
            else StartCoroutine(SetVRDevice(GameVariables.Device, true));
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (!Application.isEditor && !UnityEngine.XR.XRSettings.enabled) Camera.main.GetComponent<Transform>().localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(XRNode.CenterEye);
	}

    private IEnumerator SetVRDevice(string device, bool vrEnabled)
    {
        Debug.Log(device);
        XRSettings.LoadDeviceByName(device);
        yield return null;
        if (!XRSettings.loadedDeviceName.Equals(device, System.StringComparison.OrdinalIgnoreCase))
        {
            GameVariables.DaydreamSupported = false;
            SceneManager.LoadScene("StartScene");
        }
        else if (vrEnabled)
        {
            XRSettings.enabled = vrEnabled;
        }
        else
        {
            yield return null;
            XRSettings.enabled = vrEnabled;
        }
    }

    public void ToggleCardboard()
    {
        if (!Application.isEditor)
        {
            UnityEngine.XR.XRSettings.enabled = !UnityEngine.XR.XRSettings.enabled;
        }
    }

}
