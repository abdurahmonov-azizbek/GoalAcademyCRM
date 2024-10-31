// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class FailedStudentGroupServiceException : Xeption
    {
        public FailedStudentGroupServiceException(Exception innerException)
            : base(message: "Failed studentGroup service error occurred, please contact support.", innerException)
        { }
    }
}