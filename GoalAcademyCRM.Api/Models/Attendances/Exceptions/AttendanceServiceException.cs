// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class AttendanceServiceException : Xeption
    {
        public AttendanceServiceException(Exception innerException)
            : base(message: "Attendance service error occured, contact support.", innerException)
        { }
    }
}