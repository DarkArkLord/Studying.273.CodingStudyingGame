using Newtonsoft.Json;

namespace Assets.Scripts.DataKeeper
{
    public class ProgressStateKeeper
    {
        public ProgressStateKeeper()
        {
            KilledEmeniesCounter = 0;

            QuestsInfo = new QuestsInfoKeeper();
        }

        [JsonProperty]
        public int KilledEmeniesCounter { get; set; }

        [JsonProperty]
        public QuestsInfoKeeper QuestsInfo {  get; set; }
    }
}
