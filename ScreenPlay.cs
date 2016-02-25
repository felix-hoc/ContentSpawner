using System.IO;
using UnityEngine;
using System.Collections.Generic;

public enum ItemType {
	unknown,
	audio,
	image
}

[System.Serializable]
public class ScreenPlayItem : System.Object {
	public string url = "";
	public float delay = 0f;
	public ItemType type = ItemType.unknown;

	public override string ToString () {
		return JsonUtility.ToJson(this, true);
	}
}

[System.Serializable]
public class ScreenPlay : System.Object {
	[SerializeField]
	private List<ScreenPlayItem> _screenPlayItems = new List<ScreenPlayItem>();

	public void addItem(ScreenPlayItem screenPlayItem) {
		_screenPlayItems.Add(screenPlayItem);
	}

	public List<ScreenPlayItem> getItems() {
		return _screenPlayItems;
	}

	public void saveToFile(string fileName) {
		File.WriteAllText(fileName, JsonUtility.ToJson(this));
	}

	public static ScreenPlay loadFromFile(string fileName) {
		return JsonUtility.FromJson<ScreenPlay>(File.ReadAllText(fileName));
	}
}
