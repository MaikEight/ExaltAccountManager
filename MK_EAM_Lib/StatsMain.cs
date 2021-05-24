namespace MK_EAM_Lib
{
    [System.Serializable]
    public class StatsMain
    {
        public string email;
        public System.Collections.Generic.List<AccountStats> stats = new System.Collections.Generic.List<AccountStats>();
        public System.Collections.Generic.List<CharListStats> charList = new System.Collections.Generic.List<CharListStats>();
        public System.Collections.Generic.List<LoginStats> logins = new System.Collections.Generic.List<LoginStats>();

        public StatsMain() { }       
    }
}
