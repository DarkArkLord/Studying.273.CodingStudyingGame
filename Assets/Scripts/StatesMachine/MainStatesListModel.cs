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

        Map_Forest_1 = 40,
        Map_Forest_2 = 41,

        Battle_test = 50,
        Battle_NumbersOrder = 51,
        Battle_Equations = 52,
        Battle_WeightBalancer = 53,
        Battle_ExecutorNumbers = 54,
    }
}
