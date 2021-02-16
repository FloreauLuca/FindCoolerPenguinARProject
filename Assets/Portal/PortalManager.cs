using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Portal
{
    public class PortalManager : MonoBehaviour
    {

        public int virtualWorldLayer;
        public int realWorldLayer;
        public int portalLayer;

        public Camera realWorldCamera;
        public Camera virtualWorldCamera;

        private RenderTexture otherWorldPreview;

        public Material portalPreview;

        public bool inVirtualWorld = false;


        // Start is called before the first frame update
        void Start()
        {

            otherWorldPreview = new RenderTexture(Screen.width, Screen.height, 24);
            Debug.Log("AFWK: Render stexture " + Screen.width + "/" + Screen.height);
            portalPreview.mainTexture = otherWorldPreview;

            ChangeWorld(false);

            Debug.Log("AFWK: Start Portal Manager");
        }



        void LateUpdate()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
           
        }


        public void ChangeWorld(bool toVirtual)
        {

            Debug.Log("AFWK: Changing world to virtual = " + toVirtual);

            inVirtualWorld = toVirtual;//false mean we are in real world
            if (!inVirtualWorld)
            {
                if (realWorldCamera.targetTexture != null)
                {
                    realWorldCamera.targetTexture.Release();
                    realWorldCamera.targetTexture = null;
                }

                virtualWorldCamera.targetTexture = otherWorldPreview;
            }
            else
            {
                if (virtualWorldCamera.targetTexture != null)
                {
                    virtualWorldCamera.targetTexture.Release();
                    virtualWorldCamera.targetTexture = null;
                }

                realWorldCamera.targetTexture = otherWorldPreview;
            }


        }

    }
}
