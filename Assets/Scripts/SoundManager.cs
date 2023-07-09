using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip audio;
    public static SoundManager instance;
    private AudioSource track01, track02;
    private bool isPlayingTrack01;
    void Awake() {
        if(instance == null) {
          instance = this;
          DontDestroyOnLoad(gameObject);
        } else {
          Destroy(gameObject);
        }
    }

    void Start() {
      track01 = gameObject.AddComponent<AudioSource>();
      track02 = gameObject.AddComponent<AudioSource>();
      // track03 = gameObject.AddComponent<AudioSource>();

      track01.loop = false;
      track02.loop = true;


      isPlayingTrack01 = true;
      SwapTrack(audio, audio);
    }

    public void SwapTrack(AudioClip startClip, AudioClip repeatClip) {
      StopAllCoroutines();
      StartCoroutine(FadeTrack(startClip));

      double startTime = AudioSettings.dspTime + 0.5f;
      double duration = startClip.samples / startClip.frequency;


      isPlayingTrack01 = !isPlayingTrack01;
    }

    private IEnumerator FadeTrack(AudioClip newClip) {
      float timeToFade = 3.25f;
      float timeElapsed = 0f;
      if(isPlayingTrack01) {
        track02.clip = newClip;
        track02.Play();

        while(timeElapsed <= timeToFade) {
          track02.volume = Mathf.Lerp(0, 0.7f, timeElapsed/timeToFade);
          track01.volume = Mathf.Lerp(0.7f, 0, timeElapsed/timeToFade);
          timeElapsed += Time.deltaTime;
          yield return null;
        }
        track01.Stop();

      } else {
        track01.clip = newClip;
        track01.Play();
        while(timeElapsed <= timeToFade) {
          track01.volume = Mathf.Lerp(0, 0.7f, timeElapsed/timeToFade);
          track02.volume = Mathf.Lerp(0.7f, 0, timeElapsed/timeToFade);
          timeElapsed += Time.deltaTime;
          yield return null;
        }
        track02.Stop();
      }
    }
}
