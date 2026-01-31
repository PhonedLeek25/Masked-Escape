using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public int index = 1;
    public void LoadScene()
    {
        SceneManager.LoadScene(index);
    }
}
