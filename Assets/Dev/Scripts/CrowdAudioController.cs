using UnityEngine;

public class CrowdAudioController : MonoBehaviour
{
    public AudioSource crowdSource;
    public Transform player;

    [Header("Detection")]
    public float detectRadius = 4f;
    public LayerMask npcLayer;

    [Header("Audio")]
    public float maxVolume = 0.6f;
    public float smoothSpeed = 2f;

    [Header("Pitch")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.3f;

    float currentVolume;
    float currentPitch;

    void Start()
    {
        if (!crowdSource)
            crowdSource = GetComponent<AudioSource>();

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        currentVolume = 0.2f;
        currentPitch = 1f;          // 🔥 ÇOK ÖNEMLİ
        crowdSource.volume = 0f;
        crowdSource.pitch = 1f;

        crowdSource.loop = true;
        crowdSource.Play();
    }

    void Update()
    {
        int npcCount = Physics.OverlapSphere(
            player.position,
            detectRadius,
            npcLayer
        ).Length;

        float t = Mathf.Clamp01(npcCount / 10f);

        float targetVolume = t * maxVolume;
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);

        currentVolume = Mathf.Lerp(
            currentVolume,
            targetVolume,
            Time.deltaTime * smoothSpeed
        );

        currentPitch = Mathf.Lerp(
            currentPitch,
            targetPitch,
            Time.deltaTime * smoothSpeed
        );

        crowdSource.volume = currentVolume;
        crowdSource.pitch = currentPitch;   
    }
}
