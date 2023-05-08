using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverText : MonoBehaviour
{
    public TextMesh hpRecover;

    private void Start()
    {
        Destroy(gameObject, 1f); 
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 2f);
    }

    public void DisplayRecover(float recover)
    {
        hpRecover.text = "+" + (int)recover;
    }
}
