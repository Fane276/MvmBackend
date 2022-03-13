namespace MvManagement.VehicleData
{
    public static class VehiclePermissionNames
    {
        private static string VehiclePermissionDomain = "VehiclePermission";
        public static class UserRoles
        {
            private static string BasePermissionName = $"{VehiclePermissionDomain}.UserRoles";
            public static string View = $"{BasePermissionName}.View";
            public static string Add = $"{BasePermissionName}.Add";
            public static string Remove = $"{BasePermissionName}.Remove";
            public static string Update = $"{BasePermissionName}.Update";
        }

        public static class VehicleInfo
        {
            private static string BasePermissionName = $"{VehiclePermissionDomain}.VehicleInfo";
            public static string View = $"{BasePermissionName}.View";
            public static string Edit = $"{BasePermissionName}.Edit";
        }
        public static class VehicleDocuments
        {
            private static string BasePermissionName = $"{VehiclePermissionDomain}.VehicleDocuments";
            
            public static class Insurance
            {
                public static string View = $"{BasePermissionName}.Insurance.View";
                public static string Edit = $"{BasePermissionName}.Insurance.Edit";
            }
            public static class PeriodicalDocuments
            {
                public static string View = $"{BasePermissionName}.PeriodicalDocuments.View";
                public static string Edit = $"{BasePermissionName}.PeriodicalDocuments.Edit";
            }
            public static class StorageDocuments
            {
                public static string View = $"{BasePermissionName}.StorageDocuments.View";
                public static string Edit = $"{BasePermissionName}.StorageDocuments.Edit";
            }
        }
    }
}