using System.Collections;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    // BGM stuff
    public AudioSource dayBGM;
    public AudioSource nightBGM;
    public float fadeDurationBGM = 2.0f;
    private float originalDayBGMVolume;
    private float originalNightBGMVolume;
    private bool isDayBGMPlaying = false;
    private bool isNightBGMPlaying = false;
    private bool isPaused = false;


    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve reflectionsIntensityMultiplier;
    public AnimationCurve lightingIntensityMultiplier;


    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;

        // Store the original BGM volumes
        originalDayBGMVolume = dayBGM.volume;
        originalNightBGMVolume = nightBGM.volume;

        PlayInitialBGM();
    }

    private void Update()
    {
        time += timeRate * Time.deltaTime;

        if (time >= 1.0f)
            time = 0.0f;

        // Check if the game is paused
        if (PauseMenu.GameIsPaused)
        {
            if (!isPaused)
            {
                // If paused and not previously paused, set BGM volume to below half
                dayBGM.volume = originalDayBGMVolume * 0.4f;
                nightBGM.volume = originalNightBGMVolume * 0.4f;
                isPaused = true;
            }
        }
        else
        {
            if (isPaused)
            {
                // If unpaused and previously paused, restore BGM volume to the original values
                dayBGM.volume = originalDayBGMVolume;
                nightBGM.volume = originalNightBGMVolume;
                isPaused = false;
            }
        }

        // BGM
        if (time >= 0.25f && time < 0.75f)
        {
            if (!isDayBGMPlaying)
            {
                // Switch to dayBGM.
                SwitchBGM(dayBGM);
            }
        }
        else
        {
            if (!isNightBGMPlaying)
            {
                // Switch to nightBGM.
                SwitchBGM(nightBGM);
            }
        }







        // light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        // light intensity
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        // change colors
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        // enable and disable the sun
        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
        }
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
        }

        // enable and disable the moon
        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
        }

        // lighting and reflections intensity
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);
    }


    void PlayInitialBGM()
    {
        if (time >= 0.25f && time < 0.75f)
        {
            // Play dayBGM initially.
            PlayDayBGM();
        }
        else
        {
            // Play nightBGM initially.
            PlayNightBGM();
        }
    }

    void PlayDayBGM()
    {
        if (isNightBGMPlaying)
        {
            StartCoroutine(FadeOutBGM(nightBGM));
        }

        StartCoroutine(FadeInBGM(dayBGM));
    }

    void PlayNightBGM()
    {
        if (isDayBGMPlaying)
        {
            StartCoroutine(FadeOutBGM(dayBGM));
        }

        StartCoroutine(FadeInBGM(nightBGM));
    }

    void SwitchBGM(AudioSource newBGM)
    {
        // Ensure the new BGM is not already playing.
        if (!newBGM.isPlaying)
        {
            if (isDayBGMPlaying)
            {
                StartCoroutine(FadeOutBGM(dayBGM));
            }
            else if (isNightBGMPlaying)
            {
                StartCoroutine(FadeOutBGM(nightBGM));
            }

            StartCoroutine(FadeInBGM(newBGM));
        }
    }

    IEnumerator FadeInBGM(AudioSource bgm)
    {
        float startVolume = 0.0f;
        float endVolume = 1.0f;

        float startTime = Time.time;

        bgm.volume = startVolume;
        bgm.Play();

        while (bgm.volume < endVolume)
        {
            float elapsedTime = Time.time - startTime;
            bgm.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDurationBGM);
            yield return null;
        }

        bgm.volume = endVolume;

        if (bgm == dayBGM)
        {
            isDayBGMPlaying = true;
            isNightBGMPlaying = false;
        }
        else
        {
            isNightBGMPlaying = true;
            isDayBGMPlaying = false;
        }
    }

    IEnumerator FadeOutBGM(AudioSource bgm)
    {
        float startVolume = bgm.volume;
        float endVolume = 0.0f;

        float startTime = Time.time;

        while (bgm.volume > endVolume)
        {
            float elapsedTime = Time.time - startTime;
            bgm.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDurationBGM);
            yield return null;
        }

        bgm.volume = endVolume;
        bgm.Stop();

        if (bgm == dayBGM)
        {
            isDayBGMPlaying = false;
        }
        else
        {
            isNightBGMPlaying = false;
        }
    }
}
