using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Farm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;

        private CanvasGroup fadeCanvasGroup;

        private bool isFade;

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private IEnumerator Start()
        {
            if (!SceneManager.GetSceneByName("UI").isLoaded)
            {
                yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            }
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return LoadSceneSetActive(startSceneName);
            EventHandler.CallAfterSceneLoadedEvent();
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
            {
                StartCoroutine(Transition(sceneToGo, positionToGo));
            }
        }

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();

            yield return Fade(1 ,Settings.fadeInOutDuration);// 快速淡入（黑屏）

            //yield return new WaitForSeconds(Settings.fadeHoldDuration);// 黑屏保持一段时间
            // 场景卸载与加载
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);
            //移动人物坐标
            EventHandler.CallMoveToPosition(targetPosition);

            EventHandler.CallAfterSceneLoadedEvent();

            yield return Fade(0, Settings.fadeInOutDuration);// 快速淡出（亮屏）


        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            //自己更改的讓他的scene載入完整后才觸發事件
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // 等待場景加載完成
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // 取得新場景
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            // 設為活動場景
            SceneManager.SetActiveScene(newScene);

            // 現在場景已激活，觸發事件
            EventHandler.CallAfterSceneLoadedEvent();

            //原本的
            //yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            //
            //Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            //
            //SceneManager.SetActiveScene(newScene);
        }

        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha, float duration)
        {
            isFade = true;

            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / duration;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;

            isFade = false;
            Debug.Log("Fade to alpha: " + targetAlpha + " | CanvasGroup: " + fadeCanvasGroup);

        }
    }
}

