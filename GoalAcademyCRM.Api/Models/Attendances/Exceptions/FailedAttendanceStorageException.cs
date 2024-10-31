// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class FailedAttendanceStorageException : Xeption
    {
        public FailedAttendanceStorageException(Exception innerException)
            : base(message: "Failed attendance storage error occurred, contact support.", innerException)
        { }
    }
}