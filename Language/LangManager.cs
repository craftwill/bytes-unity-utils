using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes.Language
{
    /// <summary>
    /// Folder tree should look like this:
    /// 
    /// *you can rename your files if you set fileName in LoadLang params*
    /// Resources/
    ///     Lang/
    ///         fr/lang.json
    ///         en/lang.json
    /// 
    /// </summary>
    //[ExecuteAlways]
    public class LangManager : MonoBehaviour
    {
        static string TEXT_NOT_FOUND = "NULLPTR_TEXT";

        static protected LangManager instance;
        static protected LangManager GetInstance() { return instance; }

        [Header("Params")]
        public string[] languages;
        public bool keepInScene;
        public bool loadLangInAwake;
        public bool updateAllLangTextInScene;

        [Header("State")]
        public string currentLangage;
        LangDataInstance[] currentLangDataInstance;

        protected virtual void Awake()
        {
            Inititialize();
        }

        public virtual void Inititialize()
        {
            instance = this;
            if (keepInScene) { DontDestroyOnLoad(this.gameObject); }
            if (loadLangInAwake) { LoadLangMultipleFiles(currentLangage, new string[] { currentLangage + "-lang", "quests", "wandererRants" }); }

            print("LangManager loaded langs!");
            //print(GetText(1) + " WORKED!");
            //print(GetText(2, 1) + " WORKED!");
        }

        static public void LoadLang(string lang, string fileName = "-lang")
        {
            GetInstance().currentLangDataInstance = new LangDataInstance[1];
            GetInstance().currentLangage = lang;
            LoadLangDataInstance("Lang/" + lang + "/" + fileName + ".json");
            UpdateAllLangTextInScene();
        }

        static public void LoadLangMultipleFiles(string lang, string[] fileNames)
        {
            GetInstance().currentLangDataInstance = new LangDataInstance[fileNames.Length];
            GetInstance().currentLangage = lang;
            for (int i = 0; i < fileNames.Length; i++)
            {
                LoadLangDataInstance("Lang/" + lang + "/" + fileNames[i] + ".json", true, i);
            }
            UpdateAllLangTextInScene();
        }

        static protected void UpdateAllLangTextInScene()
        {
            if (!GetInstance().updateAllLangTextInScene) { return; }

            foreach (LangText txt in GameObject.FindObjectsOfType<LangText>())
            {
                txt.UpdateText();
            }
        }

        static public string GetText(int id)
        {
            string text = "";
            for (int i = 0; i < GetInstance().currentLangDataInstance.Length; i++)
            {
                text = GetText(id, i);
                if (text != TEXT_NOT_FOUND && text != null) { return text; }
            }
            return text;
        }

        static public string GetText(int id, int specificFileIndex)
        {
            if (GetInstance().currentLangDataInstance.Length <= specificFileIndex) { return TEXT_NOT_FOUND; }
            LangDataInstance langData = GetInstance().currentLangDataInstance[specificFileIndex];
            if (langData == null) { Debug.LogWarning("Lang File index: " + specificFileIndex + " does not exits. There is " + GetInstance().currentLangDataInstance.Length + " files loaded."); return TEXT_NOT_FOUND; }
            return GetText(langData, id);
        }

        static protected string GetText(LangDataInstance langData, int id)
        {
            string text = TEXT_NOT_FOUND;
            text = langData.texts.Find(x => x.id == id)?.textValue;
            return text;
        }

        static protected void LoadLangDataInstance(string resourcePath, bool additive = false, int index = 0)
        {
            string filePath = resourcePath.Replace(".json", "");
            try
            {
                TextAsset targetFile = Resources.Load<TextAsset>(filePath);
                LangDataInstance langData = JsonUtility.FromJson<LangDataInstance>(targetFile.text);

                if (additive) { GetInstance().currentLangDataInstance[index] = langData; }
                else { GetInstance().currentLangDataInstance[0] = langData; }

                print("Loaded: " + filePath);

            }
            catch (System.Exception exc)
            {
                print("Error! While loading currentLangDataInstance from path: " + filePath);
                print("and error msg is: " + exc);
            }
        }

        static public bool GetIsReady()
        {
            return GetInstance() != null && GetInstance().currentLangDataInstance.Length > 0;
        }

    }
}
