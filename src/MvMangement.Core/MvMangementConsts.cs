using MvMangement.Debugging;

namespace MvMangement
{
    public class MvMangementConsts
    {
        public const string LocalizationSourceName = "MvMangement";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "3d5b2fab7c204e64830f3c46f01f39d0";
    }
}
