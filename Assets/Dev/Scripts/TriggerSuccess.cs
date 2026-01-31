using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSuccess : MonoBehaviour
{
    public int SuccessSceneIndex = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }
        SceneManager.LoadScene(SuccessSceneIndex);
    }
}
