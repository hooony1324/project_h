using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SkillPrecedingAction : ICloneable
{
    public virtual void Start(Skill skill) { }
    public abstract bool Run(Skill skill);
    public virtual void Release(Skill skill) { }

    protected virtual IReadOnlyDictionary<string, string> GetStringsByKeyword() => null;

    public abstract object Clone();
}
