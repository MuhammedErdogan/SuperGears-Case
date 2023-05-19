using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Resource
{
    public static class Extension
    {
        public static void ScaleTo(this Transform transform, Vector3 targetScale, float duration, Action callBack = null)
        {
            CoroutineManager.Instance.StartCoroutine(ScaleToCoroutine(transform, targetScale, duration, callBack));
        }

        private static IEnumerator ScaleToCoroutine(Transform transform, Vector3 targetScale, float duration, Action callBack)
        {
            Vector3 startScale = transform.localScale;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float progress = time / duration;
                transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
                yield return null;
            }

            transform.localScale = targetScale;  // Ensure the target scale is set exactly at the end
            callBack?.Invoke();
        }

        public static void ChangeTo(this MonoBehaviour behaviour, float startValue, float targetValue, float duration, Action<float> onUpdate, Action<float> onComplete = null)
        {
            CoroutineManager.Instance.StartCoroutine(ChangeToCoroutine(behaviour, startValue, targetValue, duration, onUpdate, onComplete));
        }

        private static IEnumerator ChangeToCoroutine(MonoBehaviour behaviour, float startValue, float targetValue, float duration, Action<float> onUpdate, Action<float> onComplete)
        {
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float progress = time / duration;
                float currentValue = Mathf.Lerp(startValue, targetValue, progress);

                onUpdate(currentValue);

                yield return null;
            }

            onUpdate(targetValue);  // Ensure the target value is set exactly at the end

            onComplete?.Invoke(targetValue);
        }
    }
}

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;
    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CoroutineManager");
                _instance = go.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
}