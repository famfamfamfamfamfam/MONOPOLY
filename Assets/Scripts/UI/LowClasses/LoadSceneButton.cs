using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField]
    int targetSceneIndex;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(targetSceneIndex));
    }
    void OnDisable()
    {
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
