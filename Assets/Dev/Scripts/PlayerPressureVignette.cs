using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class PlayerPressureEffects : MonoBehaviour
{
    [Header("REFERENCES")]
    public Volume globalVolume;
    public Camera playerCamera;
    public AudioSource heartbeatSource;

    Vignette vignette;

    [Header("NPC DETECTION")]
    public float detectRadius = 2.5f;
    public string npcTag = "NPC";

    [Header("VIGNETTE")]
    public float maxVignetteIntensity = 0.75f;
    public float vignetteSmoothSpeed = 5f;

    [Header("CAMERA FOV")]
    public float normalFOV = 60f;
    public float stressedFOV = 52f;

    [Header("HEARTBEAT")]
    public float maxHeartbeatVolume = 0.9f;
    public float minHeartbeatPitch = 0.9f;
    public float maxHeartbeatPitch = 1.5f;

    float currentVignette;

    void Start()
    {
        // Camera
        if (playerCamera == null)
            playerCamera = Camera.main;

        // Vignette
        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette not found in Volume!");
        }

        // Audio
        if (heartbeatSource != null)
        {
            heartbeatSource.volume = 0f;
            heartbeatSource.loop = true;
        }

      
        

    }

    void Update()
    {
        int nearbyNPCs = CountNearbyNPCs();

        HandleVignette(nearbyNPCs);
        HandleHeartbeat(nearbyNPCs);
        HandleCameraFOV(nearbyNPCs);
    }

    // ---------------- NPC COUNT ----------------
    int CountNearbyNPCs()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius);
        int count = 0;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            if (hit.CompareTag(npcTag))
                count++;
        }

        return count;
    }

    // ---------------- VIGNETTE ----------------
    void HandleVignette(int npcCount)
    {
        if (vignette == null)
            return;

        float target = Mathf.Clamp01(npcCount / 4f) * maxVignetteIntensity;

        currentVignette = Mathf.Lerp(
            currentVignette,
            target,
            Time.deltaTime * vignetteSmoothSpeed
        );

        vignette.intensity.value = currentVignette;
    }

    // ---------------- HEARTBEAT ----------------
    void HandleHeartbeat(int npcCount)
    {
        if (heartbeatSource == null)
            return;

        if (npcCount > 0)
        {
            if (!heartbeatSource.isPlaying)
                heartbeatSource.Play();
        }
        else
        {
            if (heartbeatSource.isPlaying)
                heartbeatSource.Stop();
        }

        float stress01 = Mathf.Clamp01(npcCount / 4f);

        float targetVolume = Mathf.Lerp(0.25f, maxHeartbeatVolume, stress01);
        heartbeatSource.volume = Mathf.Lerp(
            heartbeatSource.volume,
            targetVolume,
            Time.deltaTime * 3f
        );

        heartbeatSource.pitch = Mathf.Lerp(
            minHeartbeatPitch,
            maxHeartbeatPitch,
            stress01
        );
    }

    // ---------------- CAMERA FOV ----------------
    void HandleCameraFOV(int npcCount)
    {
        if (playerCamera == null)
            return;

        float stress01 = Mathf.Clamp01(npcCount / 4f);
        float targetFOV = Mathf.Lerp(normalFOV, stressedFOV, stress01);

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * 4f
        );
    }

    // ---------------- DEBUG ----------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
