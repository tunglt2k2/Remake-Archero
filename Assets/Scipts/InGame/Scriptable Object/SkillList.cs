using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill
{
    public Sprite imageSkill;
    public string name;
}

[CreateAssetMenu]
public class SkillList : ScriptableObject
{
    public List<Skill> skills;
}
