using Newtonsoft.Json;

namespace Assets.Scripts.DataKeeper
{
    public class ProgressStateKeeper
    {
        public ProgressStateKeeper()
        {
            KilledEmeniesCounter = 0;
        }

        [JsonProperty]
        public int KilledEmeniesCounter { get; set; }
    }
}
