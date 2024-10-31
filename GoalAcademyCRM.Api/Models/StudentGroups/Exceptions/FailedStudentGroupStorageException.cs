// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class FailedStudentGroupStorageException : Xeption
    {
        public FailedStudentGroupStorageException(Exception innerException)
            : base(message: "Failed studentGroup storage error occurred, contact support.", innerException)
        { }
    }
}