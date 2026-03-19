using Microsoft.Extensions.Localization;
using Further.Weigh.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Further.Weigh;

[Dependency(ReplaceServices = true)]
public class WeighBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<WeighResource> _localizer;

    public WeighBrandingProvider(IStringLocalizer<WeighResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
