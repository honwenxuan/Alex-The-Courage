using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Xml.Linq;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private bool isMuted = false;
    private float originalVolume;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            originalVolume = s.volume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Pause();
    }

    public void MuteOnlyOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }

        if (!isMuted)
        {
            isMuted = true;

            // Mute audio by setting volume to zero
            s.source.volume = 0f;

            // Start a coroutine to restore volume after a delay
            StartCoroutine(RestoreVolumeAfterDelay(s, 0.3f));
        }
    }

    private IEnumerator RestoreVolumeAfterDelay(Sound s, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restore the original volume
        s.source.volume = originalVolume;

        isMuted = false; // Reset the muted flag
    }
}
