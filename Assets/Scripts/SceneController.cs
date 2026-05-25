using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EScene
{
    Menu,
    Play
}

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [SerializeField] private UIObject _loadingBackground;

    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async UniTask Load(EScene scene)
    {
        _asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        _asyncOperation.allowSceneActivation = false;

        AnimateStartLoading();
        while (_asyncOperation.progress < 0.9f)
        {
            await UniTask.Yield();
        }

        await UniTask.Delay(1000);
        AnimateFinishedLoading();

        _asyncOperation.allowSceneActivation = true;
    }

    private void AnimateStartLoading()
    {
        _loadingBackground.Scale(_loadingBackground.Rect, _loadingBackground.LocalScale, new Vector3(1, 1, 1), 0.1f);
    }

    private void AnimateFinishedLoading()
    {
        _loadingBackground.Scale(_loadingBackground.Rect, _loadingBackground.LocalScale, new Vector3(1, 0, 1), 0.1f);
    }
}
