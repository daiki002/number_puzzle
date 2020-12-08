using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ManagerBase : MonoBehaviour
{


   
        /// <Summary>
        /// 引数のマスク画像のアルファを変化させるアニメーション処理です。
        /// </Summary>
        /// <param name="maskObj">マスク用オブジェクト</param>
        /// <param name="targetAlpha">アニメーション完了時のアルファの値</param>
        /// <param name="animTime">フェードにかかる時間</param>
        protected IEnumerator FadeAnimation(GameObject maskObj, float targetAlpha, float animTime)
        {
            // アニメーションの完了予定時刻を取得します。
            float finishTime = Time.time + animTime;

            // Imageコンポーネントへの参照を取得します。
            Image maskImage = maskObj.GetComponent<Image>();

            // 現在の画像の色を保存します。
            Color initColor = maskImage.color;

            // マスク画像にセットする色を定義します。現在の色から透明度の値だけ変更します。
            Color targetColor = initColor;
            targetColor.a = targetAlpha;

            while (true)
            {
                // 完了予定時刻までの残り時間を取得します。
                float remainingTime = finishTime - Time.time;

                // 完了予定時刻を過ぎている場合、または対象のゲームオブジェクトがnullの場合は処理を中断します。
                if (remainingTime <= 0)
                {
                    break;
                }

                // フェードにかかる時間に対する経過時間の割合を計算します。
                float rate = 1 - Mathf.Clamp01(remainingTime / animTime);

                // 変化前と変化後の透明度から割合に応じて透明度を変化させます。
                maskImage.color = Color.Lerp(initColor, targetColor, rate);

                // 次のフレームまで処理を待ちます。
                yield return null;
            }

            // 最終的なアルファの値をセットします。
            maskImage.color = targetColor;
        }

        /// <Summary>
        /// タイトルシーンに戻る処理です。
        /// </Summary>
        protected IEnumerator SwitchSceneProcess(GameObject screenMask, string sceneName)
        {
            // 画面をフェードアウトさせます。
            float maskAlpha = 1.0f;
            float fadeTime = 0.5f;
            yield return StartCoroutine(FadeAnimation(screenMask, maskAlpha, fadeTime));

            // シーンをロードします。
            SceneManager.LoadScene(sceneName);
        }
    }


