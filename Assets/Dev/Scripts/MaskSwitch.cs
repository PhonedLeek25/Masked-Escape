using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class MaskSwitch : MonoBehaviour
{
    public MaskStates currentMask = MaskStates.Red;
    void Start()
    {
        Debug.Log("ON");
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
        if (currentMask == MaskStates.Red) { currentMask = MaskStates.Blue; return; }
        if (currentMask == MaskStates.Blue) { currentMask = MaskStates.Red; return; }
    }
}