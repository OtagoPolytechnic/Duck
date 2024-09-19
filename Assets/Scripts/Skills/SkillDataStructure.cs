using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Skill
{
    public SkillEnum id;
    public string name;
    public string desc;
    public int cooldown;
    public float duration;

}
[Serializable]
public class SkillList
{
    public List<Skill> skills;

}

