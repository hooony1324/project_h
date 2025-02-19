using System;
using UnityEngine;

// Impact
// - 충격 이펙트 소환
// - 다른 대상으로 튕김


[System.Serializable]
public abstract class ImpactAction : ICloneable
{    
    [SerializeField]
    private GameObject _impactEffect;
    public GameObject ImpactEffect => _impactEffect;

    private Projectile _owner;
    private Skill _skill;
    private Rigidbody2D _rigidbody;

    protected Projectile Owner => _owner;
    protected Skill Skill => _skill;
    protected Rigidbody2D Rigidbody => _rigidbody;
    
    public ImpactAction() {}
    public ImpactAction(ImpactAction other)
    {
        _impactEffect = other._impactEffect;
        _owner = other._owner;
        _skill = other._skill;
        _rigidbody = other._rigidbody;
    }

    public virtual void Setup(Projectile owner, Skill skill, GameObject impactEffect)
    {
        _owner = owner;
        _skill = skill;
        _impactEffect = impactEffect;
        _rigidbody = owner.Rigidbody;
    }

    public abstract void Apply(Collision2D other);

    public abstract object Clone();
}