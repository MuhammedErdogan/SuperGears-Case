using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Others
{
    public static class Extension
    {
        public static void ScaleTo(this Transform transform, Vector3 targetScale, float duration, Action callBack = null)
        {
            transform.GetComponent<MonoBehaviour>().StartCoroutine(ScaleToCoroutine(transform, targetScale, duration, callBack));
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

        public static void ChangeTo(this MonoBehaviour behaviour, float startValue, float targetValue, float duration, Action<float> onUpdate, out Coroutine coroutine, Action<float> onComplete = null)
        {
            coroutine = behaviour.StartCoroutine(ChangeToCoroutine(startValue, targetValue, duration, onUpdate, onComplete));
        }

        private static IEnumerator ChangeToCoroutine(float startValue, float targetValue, float duration, Action<float> onUpdate, Action<float> onComplete)
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
        public static void DelayedAction(this MonoBehaviour monoBehaviour, float delay, Action action, out Coroutine coroutine)
        {
            coroutine = monoBehaviour.StartCoroutine(DelayedCoroutine(delay, action));
        }

        private static IEnumerator DelayedCoroutine(float delay, Action action)
        {
            yield return BetterWaitForSeconds.Wait(delay);
            action();
        }
    }

}