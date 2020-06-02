﻿using System.ComponentModel;

namespace Blaise.Nuget.Api.Contracts.Enums
{
    public enum SurveyType
    {
        [Description("Cati Dial record type")]
        CatiDial,

        [Description("Appointment")]
        Appointment,

        [Description("Not mapped")]
        NotMapped
    }
}