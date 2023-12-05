using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class VoiceManeger : NetworkBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private NetworkManager networkManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }

    private void Update()
    {
        
    }
}
