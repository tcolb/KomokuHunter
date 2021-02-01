using System;
using UnityEngine;
using TMPro;


public class GameTimer : MonoBehaviour
{
    [SerializeField] float MaxTimeSeconds;
    TimeSpan MaxTime;
    [NonSerialized] public TimeSpan ElapsedTime;
    [SerializeField] TextMeshPro TimerText;
    [SerializeField] AudioSource MainAudio;
    [SerializeField] float MainAudioPitchModifier;


    // Start is called before the first frame update
    void Start()
    {
        MaxTime = TimeSpan.FromSeconds(MaxTimeSeconds);
        ElapsedTime = TimeSpan.FromSeconds(0);
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(Time.deltaTime));

        if (ElapsedTime >= MaxTime)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
        }

        TimerText.text = $"Timer: {MaxTime - ElapsedTime:mm\\:ss\\:ff}";
        MainAudio.pitch += Time.deltaTime * MainAudioPitchModifier;
    }
}
