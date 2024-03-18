using Assets.Scripts.StatesMachine;

namespace Assets.Scripts.DataKeeper
{
    public class TextMenuDataKeeper
    {
        public TextMenuDataKeeper()
        {
            Clear();
        }

        public string Text { get; private set; }
        public MainStateCode? NextState { get; private set; }

        public void SetText(string text, MainStateCode? stateCode = null)
        {
            Text = text;
            NextState = stateCode;
        }

        public void Clear()
        {
            Text = string.Empty;
            NextState = null;
        }
    }
}
