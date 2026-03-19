using Further.Weigh.Localization;
using Volo.Abp.Application.Services;

namespace Further.Weigh;

/* Inherit your application services from this class.
 */
public abstract class WeighAppService : ApplicationService
{
    protected WeighAppService()
    {
        LocalizationResource = typeof(WeighResource);
    }
}
