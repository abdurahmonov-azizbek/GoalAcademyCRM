// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class FailedAttendanceServiceException : Xeption
    {
        public FailedAttendanceServiceException(Exception innerException)
            : base(message: "Failed attendance service error occurred, please contact support.", innerException)
        { }
    }
}