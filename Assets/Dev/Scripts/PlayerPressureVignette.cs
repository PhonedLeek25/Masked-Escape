using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerPressureVignette : MonoBehaviour
{
    [Header("References")]
    public Volume globalVolume;
    public Camera playerCamera;
    public PlayerMaskState playerMask; // senin mask scriptin

    Vignette vignette;

    [Header("NPC Detection")]
    public float detectRadius = 2.2f;
    public string npcTag = "NPC";

    [Header("Vignette Settings")]
    public float maxIntensity = 0.75f;
    public float smoothSpeed = 5f;
    public float pulseSpeed = 4f;
    public float pulseAmount = 0.12f;

    [Header("Camera FOV Pressure")]
    public float normalFOV = 60f;
    public float stressedFOV = 52f;

    float currentIntensity;

    void Start()
    {
        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette not found in Volume Profile!");
        }

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (vignette == null)
            return;

        int nearbyNPCs = CountNearbyNPCs();

        // NPC sayısına göre temel baskı
        float targetIntensity = Mathf.Clamp01(nearbyNPCs / 4f) * maxIntensity;

        // 🔥 NO MASK → daha fazla baskı
        if (playerMask != null && playerMask.currentMask == MaskState.None)
        {
            targetIntensity *= 1.3f;
        }

        // Nefes / panik pulse
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        float finalIntensity = Mathf.Clamp(targetIntensity + pulse, 0f, maxIntensity);

        // Yumuşak geçiş
        currentIntensity = Mathf.Lerp(
            currentIntensity,
            finalIntensity,
            Time.deltaTime * smoothSpeed
        );

        vignette.intensity.value = currentIntensity;

        // 🎥 Kamera FOV baskısı
        float fovTarget = Mathf.Lerp(
            normalFOV,
            stressedFOV,
            currentIntensity / maxIntensity
        );

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            fovTarget,
            Time.deltaTime * 4f
        );
    }

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

    // Scene view'da algılama alanını görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
