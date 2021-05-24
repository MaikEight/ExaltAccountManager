namespace MK_EAM_Lib
{
    [System.Serializable]
    public class CharListStats
    {
        public System.Collections.Generic.List<CharacterStats> chars = new System.Collections.Generic.List<CharacterStats>();
        public System.DateTime date;

        public CharListStats() { date = System.DateTime.Now; }
    }
}
