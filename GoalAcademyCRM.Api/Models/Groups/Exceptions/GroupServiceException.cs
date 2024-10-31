// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class GroupServiceException : Xeption
    {
        public GroupServiceException(Exception innerException)
            : base(message: "Group service error occured, contact support.", innerException)
        { }
    }
}