using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace OmiyaGames.Settings
{
    ///-----------------------------------------------------------------------
    /// <copyright file="GameSettings.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2017 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <author>Taro Omiya</author>
    /// <date>5/17/2017</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A singleton script that retrieves and saves settings.
    /// </summary>
    /// <seealso cref="Singleton"/>
    /// <seealso cref="ISettingsRecorder"/>
    public partial class GameSettings : ISingletonScript
    {
        public enum AppStatus
        {
            FirstTimeOpened,
            RecentlyUpdated,
            Replaying
        }

        public event Action<GameSettings> OnBeforeRetrieveSettings;
        public event Action<GameSettings> OnAfterRetrieveSettings;
        public event Action<GameSettings> OnBeforeSaveSettings;
        public event Action<GameSettings> OnAfterSaveSettings;
        public event Action<GameSettings> OnBeforeClearSettings;
        public event Action<GameSettings> OnAfterClearSettings;

        public const string VersionKey = "AppVersion";
        public static readonly ISettingsRecorder DefaultSettings = new PlayerPrefsSettingsRecorder();
        private AppStatus status = AppStatus.Replaying;
        private bool isSettingsRetrieved = false;


        #region Properties
        public static UserScope DefaultLeaderboardUserScope
        {
            get
            {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WINRT)
                return UserScope.FriendsOnly;
#else
                return UserScope.Global;
#endif
            }
        }

        public AppStatus Status
        {
            get
            {
                return status;
            }
        }

        public virtual ISettingsRecorder Settings
        {
            get
            {
                return DefaultSettings;
            }
        }
        #endregion

        #region Singleton Overrides
        public override void SingletonAwake(Singleton instance)
        {
            // Load settings
            RetrieveFromSettings();
        }

        public override void SceneAwake(Singleton instance)
        {
        }
        #endregion

        #region Unity Events
        void OnApplicationQuit()
        {
            SaveSettings();
        }
        #endregion

        public void RetrieveFromSettings()
        {
            RetrieveFromSettings(true);
        }

        public void SaveSettings()
        {
            SaveSettings(true);
        }

        public void ClearSettings()
        {
            ClearSettings(true);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Generates the C# file, "GameSettings.AutoGenerated.cs"
        /// </summary>
        [ContextMenu("Generate GameSettings.AutoGenerated.cs")]
        public void AutoGenerateSettingsCode()
        {
            // First grab all the versions
            VersionGeneratorArgs versionsArgs = GameSettingsGenerator.GetAllVersions();

            // Setup namespaces and arguments
            int index = 0;
            NamespaceGeneratorArgs usingsArgs = new NamespaceGeneratorArgs();
            SettingsGeneratorArgs settingsArgs = new SettingsGeneratorArgs();
            foreach (ISettingsVersionGenerator version in versionsArgs)
            {
                version.UpdateNamespaceArgs(usingsArgs);
                version.UpdateSettingArgs(index, settingsArgs);
                ++index;
            }

            // Next, write to a file
            GameSettingsGenerator.WriteCode(versionsArgs, usingsArgs, settingsArgs);
        }
#endif

        #region Helper Methods
        protected void RetrieveFromSettings(bool runEvent)
        {
            // Grab the the app version
            int currentVersion = Settings.GetInt(VersionKey, 0);
            if (runEvent == true)
            {
                // Update the app status
                status = AppStatus.Replaying;
                if (currentVersion < 0)
                {
                    status = AppStatus.FirstTimeOpened;
                }
                else if (currentVersion < AppVersion)
                {
                    status = AppStatus.RecentlyUpdated;
                }

                // Run events
                if (OnBeforeRetrieveSettings != null)
                {
                    OnBeforeRetrieveSettings(this);
                }
            }

            // Retrieve from all ISingleSettings here
            foreach(IStoredSetting singleSetting in AllSingleSettings)
            {
                singleSetting.OnRetrieveSetting(Settings, currentVersion, AppVersion);
            }

            // Indicate the settings has been retrieved
            isSettingsRetrieved = true;

            // Run events
            if ((runEvent == true) && (OnAfterRetrieveSettings != null))
            {
                OnAfterRetrieveSettings(this);
            }
        }

        protected void SaveSettings(bool runEvent)
        {
            // Before saving settings, retrieve them first
            if(isSettingsRetrieved == false)
            {
                RetrieveFromSettings(runEvent);
            }

            // Set the version
            Settings.SetInt(VersionKey, AppVersion);

            // Run events
            if ((runEvent == true) && (OnBeforeSaveSettings != null))
            {
                OnBeforeSaveSettings(this);
            }

            // Save each version here
            foreach (IStoredSetting singleSetting in AllSingleSettings)
            {
                singleSetting.OnSaveSetting(Settings, AppVersion);
            }

            // Save the preferences
            Settings.Save();

            // Run events
            if ((runEvent == true) && (OnAfterSaveSettings != null))
            {
                OnAfterSaveSettings(this);
            }
        }

        protected void ClearSettings(bool runEvent)
        {
            // Grab the the app version
            int currentVersion = Settings.GetInt(VersionKey, 0);

            // Run events
            if ((runEvent == true) && (OnBeforeClearSettings != null))
            {
                OnBeforeClearSettings(this);
            }

            // Delete all stored preferences
            Settings.DeleteAll();

            // Store settings that are part of options.
            // Since member variables are unchanged up to this point, we can re-use them here.
            foreach (IStoredSetting singleSetting in AllSingleSettings)
            {
                singleSetting.OnClearSetting(Settings, currentVersion, AppVersion);
            }

            // Set the version
            Settings.SetInt(VersionKey, AppVersion);

            // Retrieve the rest of the settings member variables
            RetrieveFromSettings(false);

            // Run events
            if ((runEvent == true) && (OnAfterClearSettings != null))
            {
                OnAfterClearSettings(this);
            }
        }
        #endregion
    }
}
