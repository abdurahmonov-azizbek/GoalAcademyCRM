// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class AttendanceDependencyException : Xeption
    {
        public AttendanceDependencyException(Exception innerException)
            : base(message: "Attendance dependency error occured, contact support.", innerException)
        { }
    }
}