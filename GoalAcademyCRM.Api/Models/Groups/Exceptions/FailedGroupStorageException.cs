// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class FailedGroupStorageException : Xeption
    {
        public FailedGroupStorageException(Exception innerException)
            : base(message: "Failed group storage error occurred, contact support.", innerException)
        { }
    }
}