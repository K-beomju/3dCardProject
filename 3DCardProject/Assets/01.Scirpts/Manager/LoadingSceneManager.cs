using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingSceneManager
{
    private int delayTime = 1;
    public AsyncOperation operation;

    private string nextScene;

    private CanvasGroup cg;
    private bool isIntroLoading = false;
    private System.Action OnLoadScene;
    public void Init()
    {
        GameObject canvas = Resources.Load<GameObject>("UI/LoadingCanvas");

        canvas = Object.Instantiate(canvas, null);
        cg = canvas.GetComponent<CanvasGroup>();
        Object.DontDestroyOnLoad(canvas);
    }

    public void LoadScene(string sceneName,System.Action act = null)
    {
        OnLoadScene = act;

        nextScene = sceneName;

        cg.gameObject.SetActive(true);
        cg.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(LoadAsynSceneCoroutine());
        isIntroLoading = false;

        cg.DOFade(1, 1f).SetUpdate(true);
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        yield return new WaitUntil(() => !isIntroLoading);

        while (!operation.isDone)
        {
            float time = Time.timeSinceLevelLoad;

            if (time > delayTime)
            {
                yield return new WaitForSeconds(2f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        OnLoadScene?.Invoke();
        cg.DOFade(0, 1f).OnComplete(() => cg.gameObject.SetActive(false)).SetUpdate(true);
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
