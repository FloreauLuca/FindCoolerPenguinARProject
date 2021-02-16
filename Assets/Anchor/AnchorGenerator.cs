using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ArFoundWorkshop
{

    public class AnchorGenerator : MonoBehaviour
    {

        ARRaycastManager m_RaycastManager;
        ARAnchorManager m_AnchorManager;

        List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        ARAnchor m_Anchor = null;

        // Start is called before the first frame update
        void Start()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
            Debug.Log("AFWK: Start Anchor generator");
        }


        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);

                    planeManager.SetTrackablesActive(false);

                    return anchor;
                } else
                {
                    Debug.LogWarning("AFWK CreateAnchor: no plane manager ");
                }
            }
            else
            {
                Debug.LogWarning("AFWK CreateAnchor: hit is not on a plane: " + hit.trackable);
                
            }

            return null;

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount == 0)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            Debug.Log("AFWK: New touch !!!");

            // Raycast against planes and feature points
            const TrackableType trackableTypes =
                TrackableType.PlaneWithinPolygon;

            // Perform the raycast
            if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    if (m_Anchor != null)
                    {
                        Destroy(m_Anchor.gameObject);
                    }

                    // Remember the anchor so we can remove it later.
                    m_Anchor = anchor;
                }
                else
                {
                    Debug.LogWarning("AFW: Error creating anchor");
                }
            }
        }
    }

}
