using System.IO;
using UnityEngine;
using System.Collections.Generic;

public enum ItemType {
	unknown,
	audio,
	image
}

public enum DelayType {
	absolute,
	relativeToPrevious
}

[System.Serializable]
public struct Delay {
	public float seconds;
	public DelayType type;
}

[System.Serializable]
public struct ScreenPlayItem {
	public string url;
	public Delay delay;
	public ItemType type;
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
