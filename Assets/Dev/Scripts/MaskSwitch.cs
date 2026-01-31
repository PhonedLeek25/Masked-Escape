using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerInput))]
public class MaskSwitch : MonoBehaviour
{
    public MaskStates currentMask = MaskStates.Red;
    [Header("UI Mask Reference")]
    public Image redMask;
    public Image blueMask;

    void Start()
    {
        ApplyUI();
    }
    public void OnInteract(InputValue value)
    {
        SwitchMask();
        ApplyUI();
    }

    void SwitchMask()
    {
        //Apply Logic
        if (currentMask == MaskStates.Red) { currentMask = MaskStates.Blue; return; }
        if (currentMask == MaskStates.Blue) { currentMask = MaskStates.Red; return; }
    }
    void ApplyUI()
    {
        if (currentMask == MaskStates.Red)
        {
            redMask.gameObject.SetActive(true);
            blueMask.gameObject.SetActive(false);
            Debug.Log("Enabled Red");
        }
        if (currentMask == MaskStates.Blue)
        {
            blueMask.gameObject.SetActive(true);
            redMask.gameObject.SetActive(false);
            Debug.Log("Enabled Blue");
        }


        Debug.Log("RED -- gameObject: " + redMask.gameObject.activeSelf +
            " gameObject in Heirarchy: " + redMask.gameObject.activeInHierarchy +
            " Image: " + redMask.enabled);

        Debug.Log("BLUE - gameObject: " + blueMask.gameObject.activeSelf +
            " gameObject in Heirarchy: " + blueMask.gameObject.activeInHierarchy +
                " Image: " + blueMask.enabled);
    }
}