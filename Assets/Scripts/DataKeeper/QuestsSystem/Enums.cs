namespace Assets.Scripts.DataKeeper.QuestsSystem
{
    public enum QuestIdEnum
    {
        Q1_F1_GatherFlowers = 0,
        Q2_F1_HealFriends = 1,
        Q3_F1_KillWolfes = 2,

        Q4_F2_GatherFlowers = 3,
        Q5_F2_HealFriends = 4,
        Q6_F2_KillWolfes = 5,

        Q7_F3_KillWolfes = 6,
    }

    public enum QuestState
    {
        NotAvailable = 0,
        InProgress = 1,
        Complete = 2,
    }
}
