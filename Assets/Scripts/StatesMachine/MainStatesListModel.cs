namespace Assets.Scripts.StatesMachine
{
    public class MainStatesListModel : BaseStatesListModel<MainStateCode>
    {
        //
    }

    public enum MainStateCode
    {
        None = 0,
        Init = 10,
        MainMenu = 20,
        Exit = 30,
    }
}
