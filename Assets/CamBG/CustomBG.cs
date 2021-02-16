using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBG : MonoBehaviour
{

    public Material bgMaterial;

    public ParticleSystem explosion;

    public Color mulColor = Color.black;
    public float colorDuration = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        bgMaterial.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                StartCoroutine(ExplodeMockUp());
            }
        }
    }

    IEnumerator ExplodeMockUp()
    {

        explosion.Play();
        yield return new WaitForEndOfFrame();

        bgMaterial.color = mulColor;

        float timer = colorDuration;

        while(timer > 0 )
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;

            bgMaterial.color = Color.Lerp(Color.white, mulColor, timer / colorDuration);
        }
        bgMaterial.color = Color.white;


    }
}
