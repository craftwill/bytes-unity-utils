﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

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
        Dictionary<string, string>[] currentLangDataInstances;

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
            GetInstance().currentLangDataInstances = new Dictionary<string, string>[1];
            GetInstance().currentLangage = lang;
            LoadLangDataInstance("Lang/" + lang + "/" + fileName + ".json");
            UpdateAllLangTextInScene();
        }

        static public void LoadLangMultipleFiles(string lang, string[] fileNames)
        {
            GetInstance().currentLangDataInstances = new Dictionary<string, string>[fileNames.Length];
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

        static public string GetText(string id)
        {
            string text = "";
            for (int i = 0; i < GetInstance().currentLangDataInstances.Length; i++)
            {
                text = GetText(id, i);
                if (text != TEXT_NOT_FOUND && text != null) { return text; }
            }
            return text;
        }

        static public string GetText(string id, int specificFileIndex)
        {
            if (GetInstance().currentLangDataInstances.Length <= specificFileIndex) { return TEXT_NOT_FOUND; }
            Dictionary<string, string> langData = GetInstance().currentLangDataInstances[specificFileIndex];
            if (langData == null) { Debug.LogWarning("Lang File index: " + specificFileIndex + " does not exits. There is " + GetInstance().currentLangDataInstances.Length + " files loaded."); return TEXT_NOT_FOUND; }
            return GetText(langData, id);
        }

        static protected string GetText(Dictionary<string, string> langData, string id)
        {
            string text = TEXT_NOT_FOUND;
            try
            {
                langData.TryGetValue(id, out text);
            }
            catch (Exception e)
            {
                return TEXT_NOT_FOUND;
            }
            return text;
        }

        static protected void LoadLangDataInstance(string resourcePath, bool additive = false, int index = 0)
        {
            string filePath = resourcePath.Replace(".json", "");
            try
            {
                TextAsset targetFile = Resources.Load<TextAsset>(filePath);
                //Dictionary<string, string> langData = JsonUtility.FromJson<Dictionary<string, string>>(targetFile.text);

                // Newtonsoft json serializer needed
                var langData = JsonConvert.DeserializeObject<Dictionary<string, string>>(targetFile.text);

                if (additive) { GetInstance().currentLangDataInstances[index] = langData; }
                else { GetInstance().currentLangDataInstances[0] = langData; }

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
            return GetInstance() != null && GetInstance().currentLangDataInstances.Length > 0;
        }

    }
}
