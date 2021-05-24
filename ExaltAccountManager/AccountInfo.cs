using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaltAccountManager
{   
    [System.Serializable]
    public class AccountInfo
    {
        public string name;
        public string email;
        public string password;

        public bool performSave;

        public AccessToken accessToken;

        public bool requestSuccessfull = true;

        public AccountInfo() { }
        public AccountInfo(MuledumpAccounts muledump)
        {
            name = muledump.mail;
            email = muledump.mail;
            password = muledump.password;
            performSave = false;
        }
    }
}
