namespace ExaltAccountManager
{
    [System.Serializable]
    public class AccessToken
    {
        public string token;
        public string creationTime;
        public string expirationTime;
        public string clientToken;
        public System.DateTime validUntil;

        public AccessToken() { }

        public AccessToken(string _token, string _creationTime, string _expirationTime, string _clientToken)
        {
            token = _token;
            creationTime = _creationTime;
            expirationTime = _expirationTime;
            clientToken = _clientToken;

            try
            {
                validUntil = UnixTimeStampToDateTime((double.Parse(creationTime) + double.Parse(expirationTime)));
            }
            catch
            {
                validUntil = System.DateTime.Now.AddDays(-1);
            }
        }
        public static System.DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}
