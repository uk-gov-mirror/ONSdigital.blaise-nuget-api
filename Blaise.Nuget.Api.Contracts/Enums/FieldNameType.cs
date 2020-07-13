using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
    public enum FieldNameType
    {
        NotSpecified = 0,
        Completed,
        Processed,
        WebFormStatus,
        
        [Description("QHAdmin.HOut")]
        HOut,

        [Description("QID.Case_ID")]
        CaseId
    }
}