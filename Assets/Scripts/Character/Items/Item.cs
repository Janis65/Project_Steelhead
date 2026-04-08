using UnityEngine;

namespace JO
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;        
        [TextArea] public string itemDescription;
        public int ItemID;
    }
}
