using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipGroup
    {
        public string groupName;
        public AudioClip[] clips;
    }

    public AudioClipGroup[] audioClipGroups;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int poolSize = 10;

    private Dictionary<string, AudioClip> audioClipDict;
    private Queue<AudioSource> audioSourcePool;

    private void Awake()
    {
        // Initialize dictionary
        audioClipDict = new Dictionary<string, AudioClip>();
        foreach (var group in audioClipGroups)
        {
            foreach (var clip in group.clips)
            {
                audioClipDict[$"{group.groupName}_{clip.name}"] = clip;
            }
        }

        // Initialize the audio source pool
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            obj.SetActive(false);
            audioSourcePool.Enqueue(obj.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(string groupName, string clipName, Vector3 position, bool loop = false)
    {
        if (audioSourcePool.Count == 0)
        {
            return;
        }

        AudioSource source = audioSourcePool.Dequeue();
        source.transform.position = position;

        if (audioClipDict.TryGetValue($"{groupName}_{clipName}", out AudioClip clip))
        {
            source.clip = clip;
            source.loop = loop;
            source.gameObject.SetActive(true);
            source.Play();

            if (!loop) 
            {
                StartCoroutine(HandleAudioClipDuration(source, clip.length));
            }
        }
        else
        {
            Debug.LogWarning($"Clip '{clipName}' not found.");
            ReturnToPool(source);
        }
    }

    private IEnumerator HandleAudioClipDuration(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        StopAndReturn(source);
    }

    private void StopAndReturn(AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
        ReturnToPool(source);
    }

    private void ReturnToPool(AudioSource source)
    {
        source.Stop();
        source.gameObject.SetActive(false);
        audioSourcePool.Enqueue(source);
    }
}
