namespace Assets.Scripts.StatesMachine
{
    public class MainStatesListModel : BaseStatesListModel<MainStateCode>
    {
        //
    }

    public enum MainStateCode
    {
        None = 0,
        Exit = 10,
        Init = 20,
        MainMenu = 30,
        Map = 40,
    }
}
