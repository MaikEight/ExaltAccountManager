using DiscordRPC;
using System;
using System.Collections.Generic;

namespace MK_EAM_Discord_Lib
{
    public static class Discord
    {
        public static bool IsInitialized { get; internal set; } = false;
        public static bool UpdateOnChange { get; set; } = false;

        private static DiscordRpcClient client;

        #region Values 

        #region Details 

        public static string Details
        {
            get => details;
            set
            {
                details = value;
                if (UpdateOnChange)
                    client.UpdateDetails(details);
            }
        }
        private static string details = string.Empty;

        #endregion

        #region State 

        public static string State
        {
            get => state;
            set
            {
                state = value;
                if (UpdateOnChange)
                    client.UpdateState(state);
            }
        }
        private static string state = string.Empty;

        #endregion

        #region Buttons 

        public static List<DiscordRPC.Button> Buttons
        {
            get => buttons;
            set
            {
                buttons = value;
                if (UpdateOnChange)
                    client.UpdateButtons(buttons.Count != 0 ? buttons.ToArray() : null);
            }
        }
        private static List<DiscordRPC.Button> buttons = new List<DiscordRPC.Button>();

        #endregion

        #region LargeImageKey 

        public static string LargeImageKey
        {
            get => largeImageKey;
            set
            {
                largeImageKey = value;
                if (UpdateOnChange)
                    client.UpdateLargeAsset(largeImageKey, largeImageText);
            }
        }
        private static string largeImageKey = string.Empty;

        #endregion

        #region LargeImageText 

        public static string LargeImageText
        {
            get => largeImageText;
            set
            {
                largeImageText = value;
                if (UpdateOnChange)
                    client.UpdateLargeAsset(largeImageKey, largeImageText);
            }
        }
        private static string largeImageText = string.Empty;

        #endregion

        #region SmallImageKey 

        public static string SmallImageKey
        {
            get => smallImageKey;
            set
            {
                smallImageKey = value;
                if (UpdateOnChange)
                    client.UpdateSmallAsset(smallImageKey, smallImageText);
            }
        }
        private static string smallImageKey = string.Empty;

        #endregion

        #region SmallImageText 

        public static string SmallImageText
        {
            get => smallImageText;
            set
            {
                smallImageText = value;
                if (UpdateOnChange)
                    client.UpdateSmallAsset(smallImageKey, smallImageText);
            }
        }
        private static string smallImageText = string.Empty;

        #endregion

        #region Timestamp 

        public static DateTime Timestamp
        {
            get => timestamp;
            set
            {
                timestamp = value;
                if (UpdateOnChange && UseTimestamp)
                    client.UpdateStartTime(timestamp.ToUniversalTime());
            }
        }
        private static DateTime timestamp = DateTime.Now;

        #endregion

        #region UseTimestamp 

        public static bool UseTimestamp
        {
            get => useTimestamp;
            set
            {
                useTimestamp = value;
                if (UpdateOnChange)
                {
                    if (useTimestamp)
                    {
                        client.UpdateStartTime(timestamp.ToUniversalTime());
                        return;
                    }
                    client.UpdateStartTime();
                }
            }
        }
        private static bool useTimestamp = false;

        #endregion

        #endregion        

        public static void Initialize(string applicationId, bool autoEvents = true)
        {
            if (IsInitialized) throw new InvalidOperationException("Discord is already initialized!");

            client = new DiscordRpcClient(applicationId, autoEvents: autoEvents);
            client.Initialize();

            timestamp = DateTime.Now;
            IsInitialized =
            UpdateOnChange = true;
        }

        public static void ApplyPresence()
        {
            if (!IsInitialized) throw new InvalidOperationException("Discord is not yet initialized!");

            client.SetPresence(new RichPresence()
            {
                Details = Details,
                State = State,
                Buttons = Buttons.Count == 0 ? null : Buttons.ToArray(),
                Timestamps = UseTimestamp ? new Timestamps(Timestamp.ToUniversalTime()) : new Timestamps(),
                Assets = new Assets()
                {
                    LargeImageKey = LargeImageKey,
                    LargeImageText = LargeImageText,
                    SmallImageKey = SmallImageKey,
                    SmallImageText = SmallImageText
                }
            });

            if (client.AutoEvents) return;

            client.Invoke();
        }

        public static void AddButton(string label, string url)
        {
            if (!IsInitialized) throw new InvalidOperationException("Discord is not yet initialized!");

            Buttons.Add(new DiscordRPC.Button() { Label = label, Url = url });
            if (UpdateOnChange)
                client.UpdateButtons(buttons.Count != 0 ? buttons.ToArray() : null);
        }

        public static void RemoveButton(string label)
        {
            if (!IsInitialized) throw new InvalidOperationException("Discord is not yet initialized!");

            Buttons.RemoveAll(x => x.Label == label);
            if (UpdateOnChange)
                client.UpdateButtons(buttons.Count != 0 ? buttons.ToArray() : null);
        }

        public static void Shutdown()
        {
            if (!IsInitialized) return;

            client.ClearPresence();
            client.Dispose();
            IsInitialized = false;
        }
    }
}
