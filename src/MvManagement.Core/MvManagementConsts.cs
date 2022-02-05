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
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "166cae859c244766bfb5724738d5e34f";
    }
}
