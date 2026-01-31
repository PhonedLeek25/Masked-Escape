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
        redMask.enabled = false;
        blueMask.enabled = false;
    }
    public void OnInteract(InputValue value)
    {
        Debug.Log("value is pressed:" + value.isPressed);
        Debug.Log("Interaction button pressed");
        SwitchMask();
        Debug.Log("Current mask: " + currentMask);
    }

    void SwitchMask()
    {
        //Apply Logic
        if (currentMask == MaskStates.Red) { currentMask = MaskStates.Blue; return; }
        if (currentMask == MaskStates.Blue) { currentMask = MaskStates.Red; return; }

        //Apply to UI
        if (currentMask == MaskStates.Red)
        {

        }
    }
}