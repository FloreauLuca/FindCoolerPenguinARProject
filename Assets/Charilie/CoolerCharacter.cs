using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerCharacter : Character
{
    public override void Eliminate()
    {
        StartCoroutine(EndAnimation());
    }
    IEnumerator EndAnimation()
    {
        float timer = 0.0f;
        while (timer < 2.5f) {
            transform.position += Vector3.up * 0.01f;
            transform.Rotate(Vector3.up, 10);
            yield return null;
            timer += 0.1f;
        }
        FindObjectOfType<GameManager>().EndGame();
    }
}
