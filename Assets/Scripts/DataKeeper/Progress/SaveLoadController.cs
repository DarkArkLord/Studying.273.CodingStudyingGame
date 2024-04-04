using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DataKeeper.Progress
{
    public static class SaveLoadController
    {
        private const string SaveFilePath = "/SavedData.txt";
        private static string FullSaveFilePath => Application.dataPath + SaveFilePath;

        private const string IsSaveInitedAttribute = "isInited";
        private const string DataAttribute = "data";

        public static void Save(MainDataKeeper dataKeeper)
        {
            var file = FullSaveFilePath;

            var saveData = new JObject();
            saveData[IsSaveInitedAttribute] = true;
            saveData[DataAttribute] = JObject.FromObject(dataKeeper.Progress);

            var saveText = saveData.ToString();

            File.WriteAllText(file, saveText);
        }

        public static ProgressStateKeeper Load()
        {
            var saveData = TryGetSaveFile();

            if (saveData == null) return null;

            var isInited = CheckIsSaveInited(saveData);

            if (!isInited) return null;

            return saveData[DataAttribute]?.ToObject<ProgressStateKeeper>();
        }

        public static bool HasSaveFile
            => CheckIsSaveInited(TryGetSaveFile());

        private static JObject TryGetSaveFile()
        {
            if (!File.Exists(FullSaveFilePath)) return null;

            var saveText = File.ReadAllText(FullSaveFilePath);

            if (saveText == null || saveText.Length < 2) return null;

            return JObject.Parse(saveText);
        }

        private static bool CheckIsSaveInited(JObject obj)
            => obj?.Value<bool>(IsSaveInitedAttribute) ?? false;
    }
}
