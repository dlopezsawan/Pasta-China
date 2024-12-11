using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipGroup
    {
        public string groupName; //"Ambient", "Music", "Event", "UI"
        public AudioClip[] clips;
    }

    public AudioClipGroup[] audioClipGroups;

    private Dictionary<string, AudioClip> audioClipDict;

    private void Awake()
    {
        // Initialize dictionary
        audioClipDict = new Dictionary<string, AudioClip>();

        foreach (var group in audioClipGroups)
        {
            foreach (var clip in group.clips)
            {
                string key = $"{group.groupName}_{clip.name}";
                if (!audioClipDict.ContainsKey(key))
                {
                    audioClipDict[key] = clip;
                }
            }
        }
    }

    public void PlayAudio(AudioSource source, string groupName, string clipName, bool loop = false)
    {
        string key = $"{groupName}_{clipName}";

        if (audioClipDict.TryGetValue(key, out AudioClip clip))
        {
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip '{clipName}' not found in group '{groupName}'.");
        }
    }
}
