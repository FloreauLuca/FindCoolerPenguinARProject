using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portal
{
    public class PortalObject : MonoBehaviour
    {
        public int lastLayer { get; private set; }

        private void Start()
        {
            lastLayer = gameObject.layer;
        }

        public void SetLayer(int targetLayer, bool saveAsLastLayer)
        {

            Debug.Log("AFWK: " + name + " set layer to " + targetLayer + " - " + saveAsLastLayer);

            gameObject.layer = targetLayer;
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = targetLayer;
            }
            if (saveAsLastLayer)
            {
                lastLayer = targetLayer;
            }
        }
    }
}