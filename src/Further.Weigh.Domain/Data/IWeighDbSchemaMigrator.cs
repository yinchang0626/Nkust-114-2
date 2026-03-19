using System.Threading.Tasks;

namespace Further.Weigh.Data;

public interface IWeighDbSchemaMigrator
{
    Task MigrateAsync();
}
