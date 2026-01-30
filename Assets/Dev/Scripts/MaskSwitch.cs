using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class MaskSwitch : MonoBehaviour
{
    public MaskTypes currentMask = MaskTypes.mask1;
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
        if (currentMask == MaskTypes.mask2) {currentMask = MaskTypes.mask1; return; }
        if (currentMask == MaskTypes.mask1) {currentMask = MaskTypes.mask2; return; }
    }
}
