using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	public string itemName;
	public int itemID;
	public ItemType itemType;
	public string itemDesc;
	public int itemMovementSpeed;
	public float itemAttackSpeed;
	public int itemArmor;
	public int itemDamage;
	public bool itemIsConsumable;
	public Texture2D itemIcon;
	public GameObject itemPrefab;

	
	public enum ItemType {
		Resource,
		Building,
		Weaponry,
		Gloves,
		Boots,
		Armor,
		Magic,
		Unknown
	}
	public Item(string name,int ID, ItemType type, string desc, int movementSpeed, float attackSpeed, int armor, int damage, bool isConsumable ) {
		itemName = name;
		itemID = ID;
		itemDesc = desc;
		itemType = type;
		itemIcon = Resources.Load<Texture2D> ("Item/" + name);
		itemPrefab = Resources.Load<GameObject> ("Item/" + name);
		itemIsConsumable = isConsumable;
		itemMovementSpeed = movementSpeed;
		itemAttackSpeed = attackSpeed;
		itemArmor = armor;
		itemDamage = damage;
	}
	public Item() {
		
	}
}
