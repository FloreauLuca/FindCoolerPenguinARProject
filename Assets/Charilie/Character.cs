using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected MeshRenderer meshRenderer;
    protected Color myColor;
    bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        //myColor = Random.ColorHSV(0, 1, 0, 1, 1, 1);
        myColor = Color.white;
        meshRenderer.material.color = myColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            meshRenderer.material.color = Color.red;
            isSelected = false;
        } else
        {
            meshRenderer.material.color = myColor;
        }
    }

    public void SetSelected()
    {
        isSelected = true;
    }

    public virtual void Eliminate()
    {
        FindObjectOfType<GameManager>().IncreaseTimer();
        Destroy(gameObject);
    }
}
