using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroClassDatabase : MonoBehaviour {
	public List<HeroClass> heroClasses = new List<HeroClass>();
	
	void Awake() {
		heroClasses.Add (new HeroClass( "Gatherer", 0, HeroClass.HeroClassType.unsub, 270, 2.00f, -1, 11, 11, 192, 192, 6, 1800, 900));
	}
}
