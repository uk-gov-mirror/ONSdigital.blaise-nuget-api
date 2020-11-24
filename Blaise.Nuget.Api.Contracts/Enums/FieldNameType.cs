using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
    public enum FieldNameType
    {
        NotSpecified = 0,
        
        [Description("QHAdmin.HOut")]
        HOut,

        [Description("Mode")]
        Mode,
    }
}