using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneChange : MonoBehaviour
{
    public int SceneIndex = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }
        SceneManager.LoadScene(SceneIndex);
    }
}
