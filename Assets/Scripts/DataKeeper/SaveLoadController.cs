using System.IO;
using UnityEngine;

namespace Assets.Scripts.DataKeeper
{
    public static class SaveLoadController
    {
        private const string SaveFilePath = "/SavedData.txt";
        private static string FullSaveFilePath => Application.dataPath + SaveFilePath;

        public static void Save(MainDataKeeper dataKeeper)
        {
            var file = FullSaveFilePath;
            File.WriteAllText(file, dataKeeper.KilledEmeniesCounter.ToString());
        }

        public static bool IsSaveFileExists
            => File.Exists(FullSaveFilePath);

        public static void Load(MainDataKeeper dataKeeper)
        {
            var file = FullSaveFilePath;
            var content = File.ReadAllText(file);
            dataKeeper.KilledEmeniesCounter = int.Parse(content);
        }
    }
}
