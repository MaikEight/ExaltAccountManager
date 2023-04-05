using System.Security.Cryptography;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class AccountSaveFile
    {
        public int version = 2;
        public byte[] accountsData;
        public byte[] entropy = new byte[20];
        public string error = string.Empty;

        public static AccountSaveFile Encrypt(AccountSaveFile save, byte[] accounts)
        {
            if (save != null)
            {
                try
                {
                    if (save.version == 2)
                    {
                        save.error = string.Empty;
                        save.entropy = new byte[20];
                        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                            rng.GetBytes(save.entropy);
                        save.accountsData = ProtectedData.Protect(accounts, save.entropy, DataProtectionScope.CurrentUser);
                    }
                }
                catch
                {
                    save.error = "Failed to encrypt accounts!";
                }
            }
            return save;
        }

        //public static System.ComponentModel.BindingList<MK_EAM_Lib.AccountInfo> Decrypt(AccountSaveFile save)
        public static System.Collections.Generic.List<MK_EAM_Lib.AccountInfo> Decrypt(AccountSaveFile save)
        {
            if (save != null)
            {
                try
                {
                    if (save.version == 2)
                        return (System.Collections.Generic.List<AccountInfo>)ByteArrayToObject(ProtectedData.Unprotect(save.accountsData, save.entropy, DataProtectionScope.CurrentUser));
                        //return (System.ComponentModel.BindingList<MK_EAM_Lib.AccountInfo>)ByteArrayToObject(ProtectedData.Unprotect(save.accountsData, save.entropy, DataProtectionScope.CurrentUser));
                }
                catch { }
            }
            return null;
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
