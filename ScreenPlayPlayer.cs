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
	private static readonly string SCREENPLAY_FILE_NAME = "screenPlay.json";

	private ScreenPlay _screenPlay;

	public AudioSource[] _audioSources;
	public string screenPlayName;

	private RingList<AudioSource> _audioSourcesRing;

	public void Start() {
		Spawner.init(this);
		_screenPlay = ScreenPlay.loadFromFile(SCREENPLAY_FILE_NAME);
		_audioSourcesRing = new RingList<AudioSource>(_audioSources);

		// TODO: Move to action section
		play();
	}

	private AudioSource getNextAudioSource() {
		return _audioSourcesRing.getNext();
	}

	// TODO: Get next image spawner

	public void play() {
		foreach (ScreenPlayItem item in _screenPlay.getItems()) {
			Spawner spawner = null;

			switch (item.type) {
			case ItemType.audio:
				spawner = new AudioSpawner()
					.setAudioSource(getNextAudioSource())
					.setMediaObject(Resources.Load(item.url) as AudioClip);
				break;
			case ItemType.image:
				spawner = new ImageSpawner()
					// TODO: Set image source
					.setMediaObject(Resources.Load(item.url) as Texture);
				break;
			default:
				Debug.Log("Unknown item: " + item);
				break;
			}

			if (spawner != null)
				spawner.spawnDelayed(item.delay);
		}
	}
}
