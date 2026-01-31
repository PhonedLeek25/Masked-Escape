using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerPressureController : MonoBehaviour
{
    [Header("REFERENCES")]
    public Camera playerCamera;
    public Volume globalVolume;
    public AudioSource heartbeatSource;

    Vignette vignette;

    [Header("NPC DETECTION")]
    public float detectRadius = 2.5f;
    public string npcTag = "NPC";

    [Header("VIGNETTE")]
    public float maxVignetteIntensity = 0.6f;
    public float vignetteSmooth = 6f;

    [Header("HEART PULSE")]
    public float heartPulseSpeed = 2.2f;
    public float maxVignettePulse = 0.12f;

    [Header("CAMERA SHAKE")]
    public float maxShakeAmount = 0.06f;

    [Header("HEARTBEAT AUDIO")]
    public float maxHeartbeatVolume = 0.9f;
    public float minHeartbeatPitch = 0.95f;
    public float maxHeartbeatPitch = 1.4f;

    Vector3 camStartLocalPos;
    float pulseTimer;
    float currentVignette;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        camStartLocalPos = playerCamera.transform.localPosition;

        if (!globalVolume.profile.TryGet(out vignette))
            Debug.LogError("Vignette not found in Global Volume!");

        if (heartbeatSource != null)
        {
            heartbeatSource.loop = true;
            heartbeatSource.volume = 0f;
        }
    }

    void Update()
    {
        int nearbyNPCs = CountNearbyNPCs();

        HandleHeartbeat(nearbyNPCs);
        HandlePulseEffects(nearbyNPCs);
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

    // ---------------- HEART + CAMERA + VIGNETTE ----------------
    void HandlePulseEffects(int npcCount)
    {
        if (vignette == null || playerCamera == null)
            return;

        float stress01 = Mathf.Clamp01(npcCount / 4f);

        if (stress01 <= 0f)
        {
            // reset
            currentVignette = Mathf.Lerp(currentVignette, 0f, Time.deltaTime * vignetteSmooth);
            vignette.intensity.value = currentVignette;

            playerCamera.transform.localPosition =
                Vector3.Lerp(playerCamera.transform.localPosition, camStartLocalPos, Time.deltaTime * 6f);

            return;
        }

        // heart rhythm
        pulseTimer += Time.deltaTime * heartPulseSpeed * Mathf.Lerp(1f, 1.8f, stress01);
        float pulse = Mathf.Abs(Mathf.Sin(pulseTimer));

        // CAMERA SHAKE (Y axis)
        float shake = pulse * maxShakeAmount * stress01;
        playerCamera.transform.localPosition =
            camStartLocalPos + new Vector3(0f, shake, 0f);

        // VIGNETTE BASE + PULSE
        float baseVignette = stress01 * maxVignetteIntensity;
        float targetVignette = baseVignette + pulse * maxVignettePulse;

        currentVignette = Mathf.Lerp(
            currentVignette,
            targetVignette,
            Time.deltaTime * vignetteSmooth
        );

        vignette.intensity.value = currentVignette;
    }

    // ---------------- HEARTBEAT AUDIO ----------------
    void HandleHeartbeat(int npcCount)
    {
        if (heartbeatSource == null)
            return;

        float stress01 = Mathf.Clamp01(npcCount / 4f);

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

    // ---------------- DEBUG ----------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
