using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSplit : MonoBehaviour
{
    public GameObject ChildMonster;

    public void Split()
    {
        GameObject monster =  Instantiate(ChildMonster, transform.position+ 2*Vector3.left, Quaternion.identity);
        monster.transform.SetParent(transform.parent.parent);

        GameObject monster2 = Instantiate(ChildMonster, transform.position + 2*Vector3.right, Quaternion.identity);
        monster2.transform.SetParent(transform.parent.parent);
    }
}
