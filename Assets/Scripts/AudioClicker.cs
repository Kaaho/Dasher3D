using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioClicker : MonoBehaviour {

    public delegate void AudioClick();
    public static event AudioClick OnClicked;

    private float silencelimit = 0.05f;
    private float timeFromLastClick = 0f;
    private float silenceBetweenClicks = 0.05f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            if (OnClicked != null)
            {
                OnClicked();
            }
        }
        if (UnityEngine.XR.XRSettings.enabled)
        {
            if (AudioTracker.MicLoudness < silencelimit)
            {
                timeFromLastClick += Time.deltaTime;
            }
            else
            {
                if (timeFromLastClick > silenceBetweenClicks && AudioTracker.MicLoudness > silencelimit)
                {
                    Debug.Log("AudioClick:" + timeFromLastClick);
                    timeFromLastClick = 0f;
                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    PointerEventData ped = new PointerEventData(EventSystem.current);
                    EventSystem.current.RaycastAll(ped, raycastResults);
                    if (raycastResults.Count > 0)
                    {
                        Debug.Log("raycastResults.Count:" + raycastResults.Count);
                        Debug.Log("raycastResults[0].gameObject:" + raycastResults[0].gameObject.name);
                        ExecuteEvents.Execute<IPointerClickHandler>(raycastResults[0].gameObject, ped, ExecuteEvents.pointerClickHandler);
                    }
                    if (OnClicked != null)
                    {
                        OnClicked();
                    }
                }
            }
        }
    }
}
