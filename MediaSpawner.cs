using UnityEngine;
using System.Collections;

public abstract class Spawner {
	private static MonoBehaviour _monoBehaviour;
	
	public static void init(MonoBehaviour monoBehaviour) {
		_monoBehaviour = monoBehaviour;
	}
	
	protected static MonoBehaviour getMonoBehaviour() {
		return _monoBehaviour;
	}

	public void spawnDelayed(float delaySeconds = 0) {
		getMonoBehaviour().StartCoroutine(_spawnDelayed(delaySeconds));
	}
	
	private IEnumerator _spawnDelayed(float delaySeconds = 0) {
		yield return new WaitForSeconds(delaySeconds);
		this.spawn();
	}

	public abstract void spawn();
}

public abstract class MediaSpawner<MediaObject> : Spawner{
	private MediaObject _mediaObject;

	public MediaObject getMediaObject() {
		return _mediaObject;
	}
	
	public MediaSpawner<MediaObject> setMediaObject(MediaObject mediaObject) {
		_mediaObject = mediaObject;
		return this;
	}
}

public class AudioSpawner : MediaSpawner<AudioClip> {
	private AudioSource _audioSource;

	public AudioSpawner setAudioSource(AudioSource audioSource) {
		_audioSource = audioSource;
		return this;
	}

	public override void spawn() {
		AudioClip ac = getMediaObject() as AudioClip;
		Debug.Log("Play audio: " + ac.name);
		_audioSource.PlayOneShot(getMediaObject());
	}
}

public class ImageSpawner : MediaSpawner<Texture> {
	public override void spawn() {
		Debug.Log("TODO: Spawn image");
	}
}
