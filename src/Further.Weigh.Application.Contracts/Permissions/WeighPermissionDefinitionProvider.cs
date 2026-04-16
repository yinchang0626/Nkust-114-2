using Further.Weigh.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Further.Weigh.Permissions;

public class WeighPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WeighPermissions.GroupName);

        var transportContracts = myGroup.AddPermission(
            WeighPermissions.TransportContracts.Default,
            L("Permission:TransportContracts"));
        transportContracts.AddChild(
            WeighPermissions.TransportContracts.Create,
            L("Permission:TransportContracts.Create"));
        transportContracts.AddChild(
            WeighPermissions.TransportContracts.Edit,
            L("Permission:TransportContracts.Edit"));
        transportContracts.AddChild(
            WeighPermissions.TransportContracts.Delete,
            L("Permission:TransportContracts.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeighResource>(name);
    }
}
