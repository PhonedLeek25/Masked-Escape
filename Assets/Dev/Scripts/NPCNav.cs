using UnityEngine;

public enum CurrentMask { mask1 , mask2 };
public class NPCNav : MonoBehaviour
{
    //mask 1: attracts NPC A, Repells B, neutral C
    // mask 2: attracts NPC C, Repells A, neutral B
    public enum NPCTypes { A, B, C };

    [Header("NPC based references")]
    public NPCTypes ThisNPCType;

    [Header("Player based references")]
    public CurrentMask currentMask;
    public GameObject playerObj;


    public void TakeActionBasedOnCurrentMask()
    {
        switch (currentMask)
        {
            case CurrentMask.mask1:
                if (ThisNPCType == NPCTypes.A)
                {
                    //Attract to player
                }
                else if (ThisNPCType == NPCTypes.B)
                {
                    //Repell from player
                }
                else
                {
                    //Neutral behavior
                }
                break;
            case CurrentMask.mask2:
                if (ThisNPCType == NPCTypes.C)
                {
                    //Attract to player
                }
                else if (ThisNPCType == NPCTypes.A)
                {
                    //Repell from player
                }
                else
                {
                    //Neutral behavior
                }
                break;
        }
    }
    private void Update()
    {
        TakeActionBasedOnCurrentMask();
    }
    
}
