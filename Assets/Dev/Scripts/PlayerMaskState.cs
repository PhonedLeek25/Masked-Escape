using UnityEngine;
using UnityEngine.InputSystem;

public enum MaskState { Red, Blue , None }

public class PlayerMaskState : MonoBehaviour
{
    public MaskState currentMask = MaskState.Red;

    
}
