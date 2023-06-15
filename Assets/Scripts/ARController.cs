using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    [SerializeField]
    private GameObject currentPlayerObj = null;

    public GameObject playerObj;
    public ARRaycastManager raycastManager;

    private bool isAct = false;

    private void Update()
    {
        if (isAct)
            return;

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            raycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if(hits.Count > 0 ) 
            {
                currentPlayerObj = Instantiate(playerObj, hits[0].pose.position, hits[0].pose.rotation);
                isAct = true;
            }
        }
    }
}
