using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
	private Inventory heroInventory;
	private HeroClassDatabase heroClassDatabase;

	public int exp;
	public int nextExp;
	private int prevExp;
	public int level = 1;
	private int maxLevel = 30;

	public string heroName;
	private int heroBaseMovementSpeed;
	private float heroBaseAttackSpeed;
	private int heroBaseArmor;
	private int heroBaseLowDamage;
	private int heroBaseMaxDamage;
	private int heroBaseHealth;
	private int heroBaseMana;
	public int heroMovementSpeed;
	public float heroAttackSpeed;
	public int heroArmor;
	public int heroLowDamage;
	public int heroMaxDamage;
	public int heroMaxHeat = 100;
	public int heroMaxHealth;
	public int heroMaxMana;
	public byte heroInventorySize;
	public int heroVisionDay;
	public int heroVisionNight;
	public Texture2D heroIcon;
	public GameObject heroPrefab;

	void Start(){
		heroClassDatabase = GameObject.FindGameObjectWithTag ("HeroClassDatabase").GetComponent<HeroClassDatabase>();
		AddHero (0);
		// Add the heroPrefab GameObject to the parent
		heroPrefab.transform.parent = transform;
        heroPrefab.transform.position = transform.position;
	}
	void Update(){
		CheckLevel();
		UpdateStats();
	}
	void AddHero (int heroPick){
		for( int i = 0; i < heroClassDatabase.heroClasses.Count; i++ ) {
			if( heroClassDatabase.heroClasses[i].heroClassID == heroPick ) {
				heroName = heroClassDatabase.heroClasses[heroPick].heroClassName;
				heroBaseMovementSpeed = heroClassDatabase.heroClasses[heroPick].heroClassMovementSpeed;
				heroBaseAttackSpeed = heroClassDatabase.heroClasses[heroPick].heroClassAttackSpeed;
				heroBaseArmor = heroClassDatabase.heroClasses[heroPick].heroClassArmor;
				heroBaseLowDamage = heroClassDatabase.heroClasses[heroPick].heroClassLowDamage;
				heroBaseMaxDamage = heroClassDatabase.heroClasses[heroPick].heroClassMaxDamage;
				heroBaseHealth = heroClassDatabase.heroClasses[heroPick].heroClassHealth;
				heroBaseMana = heroClassDatabase.heroClasses[heroPick].heroClassMana;
				heroInventorySize = heroClassDatabase.heroClasses[heroPick].heroClassInventorySize;
				heroIcon = heroClassDatabase.heroClasses[heroPick].heroClassIcon;
				heroPrefab = Instantiate(heroClassDatabase.heroClasses[heroPick].heroClassPrefab) as GameObject;
				heroInventorySize = heroClassDatabase.heroClasses[heroPick].heroClassInventorySize;
				heroVisionDay = heroClassDatabase.heroClasses[heroPick].heroClassVisionDay;
				heroVisionNight =  heroClassDatabase.heroClasses[heroPick].heroClassVisionNight;
			}
		}
	}
	public void Exp (int exp) {
		this.exp += exp;
	}
	public void CheckLevel() {
		nextExp = (level + 1) * 100 + prevExp;
		if (level < maxLevel && exp >= nextExp) {
			prevExp = nextExp;
			level++;
		}
		if (level > maxLevel)
			level = maxLevel;
	}
	public void UpdateStats() {
		heroMaxHealth = heroBaseHealth + level * 8;
		heroMaxMana = heroBaseMana + level * 8;
		heroLowDamage = heroBaseLowDamage + (int)(level * 0.5f);
		heroMaxDamage = heroBaseMaxDamage + (int)(level * 0.5f);
		heroArmor = heroBaseArmor + (int)(level * 0.3f);
		heroAttackSpeed = heroBaseAttackSpeed - (level * 0.01f);
		heroMovementSpeed = heroBaseMovementSpeed + (level * 5);
	}
}
