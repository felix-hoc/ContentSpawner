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
	private GameObject _container;
	private GameObject _imageSource;

	public ImageSpawner setImageSource(GameObject imageSource) {
		_imageSource = imageSource;
		return this;
	}

	public ImageSpawner setContainer(GameObject container) {
		_container = container;
		return this;
	}

	public override void spawn() {
		GameObject content = null;
		Texture tex = getMediaObject();

		Debug.Log ("Spawn texture: " + tex.name);
		content = GameObject.Instantiate (_container, _imageSource.transform.position, _imageSource.transform.rotation) as GameObject;

		content.GetComponent<Renderer> ().material.SetTexture ("_MainTex", tex);
		content.GetComponent<Renderer> ().material.SetTexture ("_EmissionMap", tex);
	}
}
