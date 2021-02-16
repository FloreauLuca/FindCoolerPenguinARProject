using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Portal
{
    public class ARPortalManager : MonoBehaviour
    {

        ARRaycastManager m_RaycastManager;
        ARAnchorManager m_AnchorManager;
        ARSessionOrigin m_SessionOrigin;

        ARAnchor previousAnchor = null;

        List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        public PortalManager portal;

        // Start is called before the first frame update
        void Start()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();

            portal.gameObject.SetActive(false);

            ARCameraManager arCamManager = portal.realWorldCamera.GetComponent<ARCameraManager>();
            if (arCamManager == null )
            {
                Debug.LogError("AFWK: No AR camera on portal real world camera !!");
            }
            arCamManager.frameReceived += CameraFrameReceived;

            Debug.Log("AFWK: Start AR Portal Manager");
        }

        private void CameraFrameReceived(ARCameraFrameEventArgs camFrame)
        {
            if (camFrame.projectionMatrix.HasValue)
            {
                portal.virtualWorldCamera.projectionMatrix = camFrame.projectionMatrix.Value;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

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
                    //turn on portal
                    portal.gameObject.SetActive(false);
                    portal.gameObject.SetActive(true);
                    portal.transform.parent = anchor.transform;
                    portal.transform.localPosition = Vector3.zero;
                    portal.transform.localRotation = Quaternion.Euler(0, 180.0f, 0);
                    if (previousAnchor)
                    {
                        Destroy(previousAnchor.gameObject);
                    }
                    previousAnchor = anchor;
                }
                else
                {
                    Debug.LogWarning("AFW: Error creating portal");
                }
            }
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

                    //remove plane visualisation
                    planeManager.SetTrackablesActive(false);

                    Debug.Log("AFWK CreateAnchor: successfully created anchor ");

                    return anchor;
                }
                else
                {
                    Debug.LogWarning("AFWK CreatePortal: no plane manager ");
                }
            }
            else
            {
                Debug.LogWarning("AFWK CreatePortal: hit is not on a plane: " + hit.trackable);

            }

            return null;

        }
    }
}