using UnityEngine;
using System.Collections.Generic;

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

    public float _imageSyncOffset = -7.0f;

	public string screenPlayName;
	public bool _autostart = false;

	private RingList<AudioSource> _audioSourcesRing;
	private RingList<GameObject> _imageSourcesRing;

	private List<Spawner> _spawners = new List<Spawner>();

	public void Start() {
		Spawner.init(this);
		_screenPlay = ScreenPlay.loadFromFile(screenPlayName);
		_audioSourcesRing = new RingList<AudioSource>(_audioSources);
		_imageSourcesRing = new RingList<GameObject> (_imageSources);

		instanciateSpawners();

		if (_autostart)
			play();
	}

	private AudioSource getNextAudioSource() {
		return _audioSourcesRing.getNext();
	}

	private GameObject getNextImageSource() {
		return _imageSourcesRing.getNext();
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

	private void instanciateSpawners() {
		float currentTime = 0f;
		Spawner spawner = null;

		foreach (ScreenPlayItem item in _screenPlay.getItems()) {
			spawner = getSpawnerForItem(item);

            //currentTime = getNextTimestamp(currentTime, item.delay);
            // Get next timestamp and spawn time

            float spawnTime = currentTime;

            switch (item.delay.type) {
                case DelayType.absolute:
                    currentTime = item.delay.seconds;
                    spawnTime = currentTime;
                    break;
                case DelayType.relativeToPrevious:
                    currentTime += item.delay.seconds;
                    spawnTime = currentTime;
                    break;
                case DelayType.relativeSynchronized:
                    currentTime += item.delay.seconds;
                    spawnTime = currentTime + _imageSyncOffset;
                    break;
                default:
                    Debug.Log("Unknown delay type: " + item.delay);
                    break;
            }




            if (spawner != null) {
				Debug.Log("Spawning " + item + " after " + spawnTime);
				spawner.setDelay(spawnTime);
				_spawners.Add(spawner);
			}
		}
	}

	public void play() {
		foreach (Spawner spawner in _spawners)
			spawner.spawnDelayed();
	}
}
