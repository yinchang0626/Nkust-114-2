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

        //Define your own permissions here. Example:
        //myGroup.AddPermission(WeighPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WeighResource>(name);
    }
}
