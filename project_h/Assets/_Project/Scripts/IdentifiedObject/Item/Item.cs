using UnityEngine;

[CreateAssetMenu(fileName = "ITEM", menuName = "Item/AbilityItem")]
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private Sprite itemHolderSprite;
    [SerializeField, Tooltip("여러 개 획득할 수 있는지의 여부")] private bool isAllowMultiple = false;
    [SerializeField, Tooltip("장비 창에 보여줄지의 여부")] private bool isEquipment = true;
    [SerializeReference, SubclassSelector] public ItemAcquireAction itemAcquireAction;
    
    [SerializeField] private string description;
    
    public int ID => id;
    public Sprite ItemHolderSprite => itemHolderSprite;
    public bool IsAllowMultiple => isAllowMultiple;
    public bool IsEquipment => isEquipment;
    public bool IsSpawnable
    {
        get
        {
            if (itemAcquireAction == null)
                return false;
                
           return itemAcquireAction.IsSpawnable;
        }
    }  

    public void Acquire()
    {
        itemAcquireAction.AqcuireAction(this);
    }

    public void Load()
    {
        if(!itemAcquireAction.IsActionType<IncreaseStatItemAction>())
            return;

        IncreaseStatItemAction increaseStatItem = itemAcquireAction as IncreaseStatItemAction;
        increaseStatItem.Load(); 
    }
}


