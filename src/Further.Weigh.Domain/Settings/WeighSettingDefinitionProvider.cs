using Volo.Abp.Settings;

namespace Further.Weigh.Settings;

public class WeighSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WeighSettings.MySetting1));
    }
}
