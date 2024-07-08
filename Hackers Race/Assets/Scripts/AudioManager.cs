using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource BGMusicSource;
    [SerializeField] private AudioClip BGMusicClip;
    [SerializeField] private List<Sound> GameSounds = new List<Sound>();
    [SerializeField] private AudioSource m_Audio;

    #region Singleton

    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            //If this is the first instance, makes it the singleton.
            _instance = this;
            //Tells unity not to destroy this object when loading a new scene.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (this != _instance)
        {
            //If a Singleton already exists and you find another reference in scene, destroy it.
            Destroy(this.gameObject);
        }
    }

    #endregion

    private void Start()
    {
        if (BGMusicClip != null)
        {
            BGMusicSource.clip = BGMusicClip;
            BGMusicSource.loop = true;
            BGMusicSource.Play();
        }
    }

    public void PlaySound(string soundID, float volume = 1)
    {
        if (GameSounds.Exists(x => x._id == soundID))
        {
            AudioClip audioClip = GameSounds.First(x => x._id == soundID)._clip;
            m_Audio.PlayOneShot(audioClip, volume);
        }
        else
            Debug.Log(string.Format("<color=red3>Sound ID not found: {0}</color>", soundID));
    }

    public void PlaySoundWithDelay(string soundID, float delay)
    {
        if (GameSounds.Exists(x => x._id == soundID))
        {
            AudioClip audioClip = GameSounds.First(x => x._id == soundID)._clip;
            StartCoroutine(WaitToPlay(audioClip, delay));
        }
        else
            Debug.Log(string.Format("<color=red3>Sound ID not found: {0}</color>", soundID));
    }

    private IEnumerator WaitToPlay(AudioClip clip, float wait, float volume = 1)
    {
        yield return new WaitForSeconds(wait);
        m_Audio.PlayOneShot(clip, volume);
    }

    public void SetPitch(float pitch)
    {
        m_Audio.pitch = pitch;
    }

    public void SetMusicVolume(float volume, bool fade = true)
    {
        if (fade)
            BGMusicSource.DOFade(volume, 1f).Play(); // Fade en AudioSource modifica el volumen.
        else
            BGMusicSource.volume = volume;
    }

    public List<string> GetSoundsIDs()
    {
        List<string> ids = new List<string>();
        foreach (Sound sound in GameSounds)
            ids.Add(sound._id);
        return ids;
    }

    public void PlayBGMusic(AudioClip clip = null, bool loop = true, float fadeInTime = 0, float volume = 1)
    {     
        if(clip!=BGMusicClip)
        {
            if (clip != null) BGMusicClip = clip;
            if (BGMusicClip != null)
            {
                BGMusicSource.clip = BGMusicClip;
                BGMusicSource.loop = loop;
                BGMusicSource.Play();
                BGMusicSource.DOFade(volume, fadeInTime).Play();
            }
        }        
    }

    public void StopBGMusic(float fadeOutTime = .5f)
    {
        BGMusicSource.DOFade(0, fadeOutTime).OnComplete(() => BGMusicSource.Stop()).Play();
    }

    public void ChangeBGMusic(AudioClip newMusic, float volume = 1)
    {
        BGMusicSource.DOFade(0, .5f).OnComplete(() => PlayBGMusic(newMusic, true, .5f, volume)).Play();
    }
}

[System.Serializable]
public class Sound
{
    public string _id;
    public AudioClip _clip;
}

