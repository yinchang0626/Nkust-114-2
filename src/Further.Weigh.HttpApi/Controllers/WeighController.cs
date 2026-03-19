using Further.Weigh.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Further.Weigh.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WeighController : AbpControllerBase
{
    protected WeighController()
    {
        LocalizationResource = typeof(WeighResource);
    }
}
