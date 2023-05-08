using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMesh damageText;

    private void Start()
    {
        Destroy(gameObject, 1f); 
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 2f);
    }

    public void DisplayDamage(float potatoDmg, bool isCritical)
    {
        if (isCritical)
        {
            damageText.text = "<color=#ff0000>" +"-" + (int)potatoDmg +"</color>";
        }
        else
        {
            damageText.text = "<color=#ffffff>" + "-" + (int)potatoDmg + "</color>";
        }
    }

    public void DisplayOneHit()
    {

        damageText.text = "<color=#ff0000>" + "-" + "HeadShot" + "</color>";
    }   
}
