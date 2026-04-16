namespace Further.Weigh.Permissions;

public static class WeighPermissions
{
    public const string GroupName = "Weigh";

    public static class TransportContracts
    {
        public const string Default = GroupName + ".TransportContracts";
        public const string Create  = Default + ".Create";
        public const string Edit    = Default + ".Edit";
        public const string Delete  = Default + ".Delete";
    }
}
