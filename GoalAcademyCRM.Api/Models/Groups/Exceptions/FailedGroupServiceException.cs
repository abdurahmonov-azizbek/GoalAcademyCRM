// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class FailedGroupServiceException : Xeption
    {
        public FailedGroupServiceException(Exception innerException)
            : base(message: "Failed group service error occurred, please contact support.", innerException)
        { }
    }
}