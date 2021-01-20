using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes.BInput
{

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }

    // When all is by default, the keybinds is an empty Dictionnary
    [System.Serializable]
    public class KeybindsWrapperSerializable : SerializableDictionary<int, BindableSerializable>
    {

    }

    [System.Serializable]
    public class BindableSerializable
    {
        [SerializeField] public KeyCode[] overridenKeycodes;
        public BindableSerializable(KeyCode[] overridenKeycodes)
        {
            this.overridenKeycodes = overridenKeycodes;
        }
        public KeyCode[] GetOverridenKeycodes()
        {
            return overridenKeycodes;
        }
        public void SetOverridenKeycodes(KeyCode[] overridenKeycodes)
        {
            this.overridenKeycodes = overridenKeycodes;
        }
    }

    public class BytesKeybinds
    {
        static private string PLAYER_PREFS_KEY = "bytesUnityUtils_Keybinds";
        static private KeybindsWrapperSerializable keybindsWrapper;

        /// <summary>
        /// All arrays must be index important and have the same length.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="override1"></param>
        /// <param name="override2"></param>
        static public void SetKeybinds(KeyCode[] original, KeyCode[] override1, KeyCode[] override2)
        {
            keybindsWrapper = new KeybindsWrapperSerializable();
            // All 3 params should have the same length
            if (!(original.Length == override1.Length && override1.Length == override2.Length)) { Debug.Log(" the keybindings arrays arent the same length!"); return; }
            for (int i = 0; i < original.Length; i++)
            {
                if (override1[i] == KeyCode.None && override2[i] == KeyCode.None) { continue; }
                keybindsWrapper.Add((int)original[i], new BindableSerializable(new KeyCode[] { override1[i], override2[i] }) );
            }
            SaveKeybinds();
        }

        static public void SetKeybind(KeyCode original, KeyCode override1, KeyCode override2 = KeyCode.None)
        {
            if (keybindsWrapper.TryGetValue((int)original, out BindableSerializable correctKeybind))
            {
                Debug.Log("EXISTS!!");
                correctKeybind.SetOverridenKeycodes(new KeyCode[] { override1, override2 });
                SaveKeybinds();
                return;
            }
            Debug.Log("DOESNT EXISTS!! ADDING IT!!");
            keybindsWrapper.Add((int)original, new BindableSerializable(new KeyCode[] { override1, override2 }));
            SaveKeybinds();
        }

        static public void ResetKeybinds(bool saveAfter = true)
        {
            keybindsWrapper = new KeybindsWrapperSerializable();
            if (saveAfter) { SaveKeybinds(); }
        }

        /// <summary>
        /// Using playerPrefs.
        /// </summary>
        static public void SaveKeybinds()
        {
            string jsonKeybinds = JsonUtility.ToJson(keybindsWrapper);
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, jsonKeybinds);
            PlayerPrefs.Save();
            Debug.Log(jsonKeybinds);
            Debug.Log("Successfully saved keybinds!");
            return;
        }

        /// <summary>
        /// Using playerPrefs.
        /// </summary>
        static public void LoadKeybinds()
        {
            //keybindsWrapper = new KeybindsWrapperSerializable();
            //SaveKeybinds();

            if (PlayerPrefs.HasKey(PLAYER_PREFS_KEY))
            {
                string jsonKeybinds = PlayerPrefs.GetString(PLAYER_PREFS_KEY);

                Debug.Log(jsonKeybinds);

                keybindsWrapper = JsonUtility.FromJson<KeybindsWrapperSerializable>(jsonKeybinds);

                Debug.Log("keybindsWrapper.keybinds.Count: " + keybindsWrapper.Count);

                Debug.Log("Successfully loaded keybinds!");
                return;
            }
            // Create empty stuff
            keybindsWrapper = new KeybindsWrapperSerializable();
            Debug.Log("No keybinds to load!");
        }

        static public bool GetKeybindDown(KeyCode keyToCheck)
        {
            if (!GetKeybindsReady()) { return false; }

            if (keybindsWrapper.TryGetValue((int)keyToCheck, out BindableSerializable correctKeybind))
            {
                KeyCode[] keyCodes = correctKeybind.GetOverridenKeycodes();
                foreach (KeyCode key in keyCodes) { if (Input.GetKeyDown(key)) { return true; } }
                return false;
            }
            return Input.GetKeyDown(keyToCheck);
        }

        static public bool GetKeybindsReady()
        {
            return (keybindsWrapper != null && keybindsWrapper != null);
        }

        static public KeyCode[] GetKeybindOverrides(KeyCode keyToCheck)
        {
            if (!GetKeybindsReady()) { return null; }

            if (keybindsWrapper.TryGetValue((int)keyToCheck, out BindableSerializable correctKeybind))
            {
                return correctKeybind.GetOverridenKeycodes();
            }
            return null;
        }

    }
}
