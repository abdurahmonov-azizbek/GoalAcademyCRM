// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Users.Exceptions
{
    public class UserServiceException : Xeption
    {
        public UserServiceException(Exception innerException)
            : base(message: "User service error occured, contact support.", innerException)
        { }
    }
}