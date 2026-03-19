namespace Further.WeighGov.S03.TransportTask.TransportContracts;

public static class TransportContractErrorCodes
{
    private const string Prefix = "WeighGov.S03.TransportTask.TransportContracts:";

    public const string CodeRequired = Prefix + "CodeRequired";
    public const string NameRequired = Prefix + "NameRequired";
    public const string VendorIdRequired = Prefix + "VendorIdRequired";
    public const string InvalidDateRange = Prefix + "InvalidDateRange";
    public const string InvalidStatusTransition = Prefix + "InvalidStatusTransition";
    public const string CodeAlreadyExists = Prefix + "CodeAlreadyExists";
}
