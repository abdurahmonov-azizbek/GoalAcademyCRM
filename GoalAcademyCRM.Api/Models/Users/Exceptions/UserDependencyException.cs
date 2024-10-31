// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Users.Exceptions
{
    public class UserDependencyException : Xeption
    {
        public UserDependencyException(Exception innerException)
            : base(message: "User dependency error occured, contact support.", innerException)
        { }
    }
}