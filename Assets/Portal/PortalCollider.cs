using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portal
{
    public class PortalCollider : MonoBehaviour
    {
        public PortalManager portal;

        private void OnTriggerEnter(Collider other)
        {

            PortalUser user = other.gameObject.GetComponent<PortalUser>();

            if (user != null)
            {
                Debug.Log("AFWK:user enter the portal " + user.gameObject.name);

                portal.ChangeWorld(!portal.inVirtualWorld);

            }

            PortalObject obj = other.gameObject.GetComponent<PortalObject>();

            if (obj != null)
            {
                Debug.Log("AFWK:object enter the portal " + obj.gameObject.name);

                obj.SetLayer(portal.portalLayer, false);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            PortalObject obj = other.gameObject.GetComponent<PortalObject>();

            if (obj != null)
            {
                Debug.Log("AFWK: object exit the portal " + obj.gameObject.name);

                if (obj.lastLayer == portal.virtualWorldLayer)
                {
                    obj.SetLayer(portal.realWorldLayer, true);
                }
                else
                {
                    obj.SetLayer(portal.virtualWorldLayer, true);
                }

            }
        }
    }
}
