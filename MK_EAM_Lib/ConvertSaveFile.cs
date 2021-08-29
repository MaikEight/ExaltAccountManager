using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    public class ConvertSaveFile
    {

        public void ConvertAccounts()
        {

        }
    }

    sealed class Version1ToVersion2DeserializationBinder : SerializationBinder
    {
        private Dictionary<string, string[]> assemblyNameToNewType = new Dictionary<string, string[]>()
        {
            { "ExaltAccountManager.AccessTokenExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null", new string[]{ "MK_EAM_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "MK_EAM_Lib.AccessToken" }},
            { "ExaltAccountManager.AccountInfoExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null", new string[]{ "MK_EAM_Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "MK_EAM_Lib.AccountInfo" }},
            { "System.Collections.Generic.List`1[[ExaltAccountManager.AccountInfo, ExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null]]mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", new string[]{ "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Collections.Generic.List`1[[MK_EAM_Lib.AccountInfo, MK_EAM_Lib, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null]]" }}
        };

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            String assemVer1 = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            String typeVer1 = "System.Collections.Generic.List`1[[ExaltAccountManager.AccountInfo, ExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null]]";

            String assemVer2 = "ExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null";
            String typeVer2 = "ExaltAccountManager.AccountInfo";

            String assemVer3 = "ExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null";
            String typeVer3 = "ExaltAccountManager.AccessToken";

            if (assemblyName == assemVer1 && typeName == typeVer1)
            {
                // To use a type from a different assembly version,
                // change the version number.
                // To do this, uncomment the following line of code.
                // assemblyName = assemblyName.Replace("1.0.0.0", "2.0.0.0");

                // To use a different type from the same assembly,
                // change the type name.
                typeName = "System.Collections.Generic.List`1[[EAM.AccountInfo, LoadNamespaceSimulatedSavefile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]";
            }
            else if (assemblyName == assemVer2 && typeName == typeVer2)
            {
                assemblyName = "LoadNamespaceSimulatedSavefile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                typeName = "EAM.AccountInfo";
            }
            else if (assemblyName == assemVer3 && typeName == typeVer3)
            {
                assemblyName = "LoadNamespaceSimulatedSavefile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                typeName = "EAM.AccessToken";
            }
            else
            {
                Console.WriteLine(String.Format("{0}, {1}", typeName, assemblyName));
                Console.ReadKey();
            }

            // The following line of code returns the type. "ExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null" 
            //type: System.Collections.Generic.List`1[[ExaltAccountManager.AccountInfo, ExaltAccountManager, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null]]
            //name: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
            //NEW
            //System.Collections.Generic.List`1[[ExaltAccountManager.AccountInfo, LoadNamespaceSimulatedSavefile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
            //mscorlib, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = b77a5c561934e089
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

            return typeToDeserialize;
        }
    }
}
