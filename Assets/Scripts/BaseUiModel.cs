using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseUiModel : BaseModel
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        public CanvasGroup CurrentCanvas => _canvasGroup;

        public virtual float TimeToShowHide => 0.5f;

        public void SetCanvasAlpha(float alpha)
        {
            CurrentCanvas.alpha = alpha;
        }

        public IEnumerator ShowPanelCorutine()
        {
            var progress = 0f;
            var endProgress = 1f;
            while (progress < endProgress)
            {
                SetCanvasAlpha(progress);
                yield return null;
                progress += Time.deltaTime / TimeToShowHide;
            }
            SetCanvasAlpha(endProgress);
            yield break;
        }

        public IEnumerator HidePanelCorutine()
        {
            var progress = 1f;
            var endProgress = 0f;
            while (progress > endProgress)
            {
                SetCanvasAlpha(progress);
                yield return null;
                progress -= Time.deltaTime / TimeToShowHide;
            }
            SetCanvasAlpha(endProgress);
            yield break;
        }

        public void EnableCanvas()
        {
            CurrentCanvas.gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            CurrentCanvas.gameObject.SetActive(false);
        }
    }
}
