namespace MK_EAM_Lib
{
    [System.Serializable]
    public class NotificationOptions
    {
        public bool useTaskTrayTool = true;

        public bool useNotifications = true;
        public bool showNotificationOnStart = false;
        public bool showNotificationOnDone = true;
        public bool showNotificationOnError = true;

        public int joinTime = 90;
        public int killTime = 30;
        public System.DateTime execTime;
    }
}
