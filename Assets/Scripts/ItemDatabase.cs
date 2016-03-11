using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	public List<Item> items = new List<Item>();

	void Awake() {
		items.Add (new Item ("Stick", 0, Item.ItemType.Resource, "A rough stick", 0, 0f, 0, 1, false));
	}
}
