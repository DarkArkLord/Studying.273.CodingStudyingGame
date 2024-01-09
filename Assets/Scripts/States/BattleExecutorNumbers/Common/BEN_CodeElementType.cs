namespace Assets.Scripts.States.BattleExecutorNumbers.Common
{
    public enum BEN_CodeElementType
    {
        IO_ReadInput = 0,
        IO_WriteOutput = 1,
        IO_SetValue = 2,
        Numeric_Add = 3,
        Numeric_Sub = 4,
        Numeric_Mult = 5,
        Numeric_Div = 6,
        Numeric_Mod = 7,
    }

    public static class BEN_CodeElementType_Ext
    {
        public static string GetName(this BEN_CodeElementType type)
        {
            switch (type)
            {
                case BEN_CodeElementType.IO_ReadInput: return "Ввод";
                case BEN_CodeElementType.IO_WriteOutput: return "Вывод";
                case BEN_CodeElementType.IO_SetValue: return "Уст.Знач.";
                case BEN_CodeElementType.Numeric_Add: return "Сложить";
                case BEN_CodeElementType.Numeric_Sub: return "Вычесть";
                case BEN_CodeElementType.Numeric_Mult: return "Умножить";
                case BEN_CodeElementType.Numeric_Div: return "Цел.Дел.";
                case BEN_CodeElementType.Numeric_Mod: return "Ост.Дел.";
                default: return "";
            }
        }

        public static string GetOper(this BEN_CodeElementType type)
        {
            switch (type)
            {
                case BEN_CodeElementType.IO_ReadInput: return "Ввод";
                case BEN_CodeElementType.IO_WriteOutput: return "Вывод";
                case BEN_CodeElementType.IO_SetValue: return ":=";
                case BEN_CodeElementType.Numeric_Add: return "+";
                case BEN_CodeElementType.Numeric_Sub: return "-";
                case BEN_CodeElementType.Numeric_Mult: return "*";
                case BEN_CodeElementType.Numeric_Div: return "/";
                case BEN_CodeElementType.Numeric_Mod: return "%";
                default: return "";
            }
        }
    }
}
