// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class GroupDependencyException : Xeption
    {
        public GroupDependencyException(Exception innerException)
            : base(message: "Group dependency error occured, contact support.", innerException)
        { }
    }
}