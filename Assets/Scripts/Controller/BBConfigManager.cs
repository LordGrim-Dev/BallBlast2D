using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace BallBlast.config
{
    public class BBConfigManager : SingletonObserverBase<BBConfigManager>
    {
        public LocalisationDetailes ConfigLocalisation { get; private set; }
        public GameSetting GameSetting { get; private set; }

        public void LoadConfig()
        {
            string path = String.Concat("ConfigData/" + BBConstants.CONFIG_FILE_NAME);

            string jsonString = Resources.Load(path).ToString();

            if (jsonString != null)
            {

                ConfigData data = JsonConvert.DeserializeObject<ConfigData>(jsonString);

                GameSetting = data.GameSetting;
                ConfigLocalisation = data.LocalisationDetails;
            }
            else
            {

#if DEBUG
                GameUtilities.ShowLog("jsonString is NULL!");
#endif
            }

#if DEBUG
            GameUtilities.ShowLog("Json--> " + jsonString);
#endif
        }


        public string GetLocalisedStringForKey(string inKey, bool isReverie = false)
        {
            string localisedString;

            if (ConfigLocalisation.LocalisationDetails_en.ContainsKey(inKey))
            {
                localisedString = ConfigLocalisation.LocalisationDetails_en[inKey];
            }
            else
            {
#if DEBUG
                GameUtilities.ShowLog("Key Not Found--> " + inKey);
#endif
                localisedString = inKey;
            }

            return localisedString;

        }


        public LevelDetails GetLevelData(int inLevel)
        {
            LevelDetails returnDetails = null;

            GameSetting.Levels.TryGetValue(inLevel, out returnDetails);

            return returnDetails;

        }

        public string GetLocalisedStringForKey(string inKey)
        {
            string localisedString = inKey.ToUpper();
            ConfigLocalisation.LocalisationDetails_en.TryGetValue(inKey, out localisedString);

            return localisedString;
        }

        public void SavePlayerProgress(PlayerProgressData inProgressData)
        {
            string jsonStr = JsonConvert.SerializeObject(inProgressData);
            GameUtilities.SaveToFile(BBConstants.CONFIG_PLAYR_PRGRSS, jsonStr);

#if DEBUG
            GameUtilities.ShowLog(" Player Progress Saved :" + jsonStr);
# endif
        }

        public PlayerProgressData GetPlayerProgress()
        {
            PlayerProgressData progress;
            var jsonData = GameUtilities.LoadFromFile(BBConstants.CONFIG_PLAYR_PRGRSS);
            progress = JsonConvert.DeserializeObject<PlayerProgressData>(jsonData);
            return progress;
        }


        public override void OnDestroy()
        {
            GameSetting = null;
            ConfigLocalisation = null; ;
            base.OnDestroy();
        }


        internal void CleanUP()
        {
            ConfigLocalisation = null;
            GameSetting.Levels.Clear();
            GameSetting = null;
        }

        internal LevelDetails GetCurrentLevelData(int cureentLeve)
        {
            LevelDetails Levdata;
            GameSetting.Levels.TryGetValue(cureentLeve, out Levdata);
            return Levdata;
        }
    }
}