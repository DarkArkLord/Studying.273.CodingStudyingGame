using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseUiModel : BaseModel
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        public CanvasGroup CurrentCanvas => _canvasGroup;

        public virtual float TimeToShowHide => 1f;

        public IEnumerator ShowPanelCorutine()
        {
            var progress = 0f;
            var endProgress = 1f;
            while (progress < endProgress)
            {
                CurrentCanvas.alpha = progress;
                yield return null;
                progress += Time.deltaTime / TimeToShowHide;
            }
            CurrentCanvas.alpha = endProgress;
            yield break;
        }

        public IEnumerator HidePanelCorutine()
        {
            var progress = 1f;
            var endProgress = 0f;
            while (progress > endProgress)
            {
                CurrentCanvas.alpha = progress;
                yield return null;
                progress -= Time.deltaTime / TimeToShowHide;
            }
            CurrentCanvas.alpha = endProgress;
            yield break;
        }
    }
}
