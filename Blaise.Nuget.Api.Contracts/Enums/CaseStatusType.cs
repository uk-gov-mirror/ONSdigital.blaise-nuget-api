using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
    public enum CaseStatusType
    {
        NotSpecified = 0,

        [Description("NISRA record imported")]
        NisraCaseImported
    }
}