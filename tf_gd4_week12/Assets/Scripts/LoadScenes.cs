using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    public void LoadScene()
    {
        Debug.Log($"button pressed");
        SceneManager.LoadScene(1);
    }
}
