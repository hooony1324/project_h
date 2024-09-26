using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class IdentifiedObject : ScriptableObject, ICloneable
{
    [SerializeField]
    private Category[] categories;

    [SerializeField]
    private int id = -1;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string displayName;
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string description;

    public int ID => id;
    public Sprite Icon => icon;
    public string DisplayName => displayName;
    public string CodeName => codeName;
    public virtual string Description => description;

    public virtual object Clone() => Instantiate(this);
    
    public bool HasCategory(Category category)
        => categories.Any(x => x.ID == category.ID);

    public bool HasCategory(string category)
        => categories.Any(x => x == category);

}