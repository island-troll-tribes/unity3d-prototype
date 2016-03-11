using UnityEngine;
using System.Collections;

[System.Serializable]
public class HeroClass {
	public string heroClassName;
	public int heroClassID;
	public HeroClassType heroClassType;
	public int heroClassMovementSpeed;
	public float heroClassAttackSpeed;
	public int heroClassArmor;
	public int heroClassLowDamage;
	public int heroClassMaxDamage;
	public int heroClassHealth;
	public int heroClassMana;
	public byte heroClassInventorySize;
	public int heroClassVisionDay;
	public int heroClassVisionNight;
	public Texture2D heroClassIcon;
	public GameObject heroClassPrefab;


	public enum HeroClassType {
		unsub,
		sub,
		supersub
	}
	public HeroClass(string name, int ID, HeroClassType type, int movementSpeed, float attackSpeed, int armor, int lowDamage, int maxDamage, int health, int mana, byte inventorySize, int visionDay, int visionNight ) {
		heroClassName = name;
		heroClassID = ID;
		heroClassType = type;
		heroClassMovementSpeed = movementSpeed;
		heroClassAttackSpeed = attackSpeed;
		heroClassArmor = armor;
		heroClassLowDamage = lowDamage;
		heroClassMaxDamage = maxDamage;
		heroClassHealth = health;
		heroClassMana = mana;
		heroClassIcon = Resources.Load<Texture2D> ("Hero Class/" + name);
		heroClassPrefab = Resources.Load<GameObject> ("Hero Class/" + name);
		heroClassInventorySize = inventorySize;
		heroClassVisionDay = visionDay;
		heroClassVisionNight = visionNight;
	}
	public HeroClass() {
		
	}
}
