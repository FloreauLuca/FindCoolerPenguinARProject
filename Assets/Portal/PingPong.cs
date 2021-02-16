using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portal
{

    public class PingPong : MonoBehaviour
    {

        public Transform startPoint;
        public Transform endPoint;

        public float frequency = 0.5f;
        public float timer = 0;
        public float delay = 3;

        // Start is called before the first frame update
        void Start()
        {
            timer = 0;
            transform.position = startPoint.position;
        }

        private void OnEnable()
        {
            timer = 0;
            transform.position = startPoint.position;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (timer > delay)
            {
                float t = (Mathf.Sin((timer-delay) * Mathf.PI * frequency) + 1) / 2;
                transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
            }
        }
    }
}
