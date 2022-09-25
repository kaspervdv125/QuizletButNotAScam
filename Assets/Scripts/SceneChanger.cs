using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;

    public int sceneToLoad;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToScene (int sceneIndex)
    {
        Debug.Log("SceneChanger");

        sceneToLoad = sceneIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
