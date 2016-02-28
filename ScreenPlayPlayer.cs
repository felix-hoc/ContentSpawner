using UnityEngine;

public class RingList<T> {
	private T[] _itemList;
	private uint _i = 0;

	public RingList(T[] itemList) {
		_itemList = itemList;
	}

	public T getNext() {
		_i ++;
		if (_i >= _itemList.Length)
			_i = 0;

		return _itemList[_i];
	}
}

public class ScreenPlayPlayer : MonoBehaviour {
	private ScreenPlay _screenPlay;

	public AudioSource[] _audioSources;
	public GameObject _imageContainer;
	public GameObject[] _imageSources;

	public string screenPlayName;
	public bool _autostart = false;

	private RingList<AudioSource> _audioSourcesRing;
	private RingList<GameObject> _imageSourcesRing;

	public void Start() {
		Spawner.init(this);
		_screenPlay = ScreenPlay.loadFromFile(screenPlayName);
		_audioSourcesRing = new RingList<AudioSource>(_audioSources);
		_imageSourcesRing = new RingList<GameObject> (_imageSources);

		if (_autostart)
			play();
	}

	private AudioSource getNextAudioSource() {
		return _audioSourcesRing.getNext();
	}

	private GameObject getNextImageSource() {
		return _imageSourcesRing.getNext();
	}

	private float getNextTimestamp(float currentTime, Delay delay) {
		switch (delay.type) {
		case DelayType.absolute:
			currentTime = delay.seconds;
			break;
		case DelayType.relativeToPrevious:
			currentTime += delay.seconds;
			break;
		default:
			Debug.Log("Unknown delay type: " + delay);
			break;
		}

		return currentTime;
	}

	private Spawner getSpawnerForItem(ScreenPlayItem screenPlayItem) {
		Spawner spawner = null;

		switch (screenPlayItem.type) {
		case ItemType.audio:
			Debug.Log("New audio spawner: " + screenPlayItem);
			spawner = new AudioSpawner()
				.setAudioSource(getNextAudioSource())
				.setMediaObject(Resources.Load(screenPlayItem.url) as AudioClip);
			break;
		case ItemType.image:
			Debug.Log("New image spawner: " + screenPlayItem);
			spawner = new ImageSpawner()
				.setContainer(_imageContainer)
				.setImageSource(getNextImageSource())
				.setMediaObject(Resources.Load(screenPlayItem.url) as Texture);
			break;
		default:
			Debug.Log("Unknown item: " + screenPlayItem);
			break;
		}

		return spawner;
	}

	public void play() {
		float currentTime = 0f;
		Spawner spawner = null;

		foreach (ScreenPlayItem item in _screenPlay.getItems()) {
			spawner = getSpawnerForItem(item);

			currentTime = getNextTimestamp(currentTime, item.delay);
			if (spawner != null) {
				Debug.Log("Spawning " + item + " after " + currentTime);
				spawner.setDelay(currentTime);
				spawner.spawnDelayed();
			}
		}
	}
}
