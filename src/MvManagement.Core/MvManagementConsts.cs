using MvManagement.Debugging;

namespace MvManagement
{
    public class MvManagementConsts
    {
        public const string LocalizationSourceName = "MvManagement";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "743f79a73b57493c9675be75b10cd985";
    }
}
