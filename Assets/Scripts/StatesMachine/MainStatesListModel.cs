namespace Assets.Scripts.StatesMachine
{
    public class MainStatesListModel : BaseStatesListModel<MainStateCode>
    {
        //
    }

    public enum MainStateCode
    {
        None = 0,
        Init = 1,
        Exit = 2,

        MainMenu = 10,
        TownMenu = 11,
        TextMenu = 12,

        Map_Forest_1 = 20,
        Map_Forest_2 = 21,

        Battle_Test = 30,
        Battle_NumbersOrder = 31,
        Battle_Equations = 32,
        Battle_WeightBalancer = 33,
        Battle_ExecutorNumbers = 34,
    }
}
