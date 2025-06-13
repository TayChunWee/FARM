using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartLoader : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 1. 加载 PersistentScene（包含 UI、Manager）
        yield return SceneManager.LoadSceneAsync("PersistentScene", LoadSceneMode.Additive);

        // 2. 加载 UI 场景（如果 UI 单独放在一个场景）
        yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }
}
