using Newtonsoft.Json;

namespace Assets.Scripts.DataKeeper.Progress
{
    public class ProgressStateKeeper
    {
        public ProgressStateKeeper()
        {
            QuestsInfo = new QuestsInfoKeeper();
        }

        [JsonProperty]
        public QuestsInfoKeeper QuestsInfo { get; set; }
    }
}
