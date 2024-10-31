// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Users.Exceptions
{
    public class NotFoundUserException : Xeption
    {
        public NotFoundUserException(Guid userId)
            : base(message: $"Couldn't find user with id: {userId}.")
        { }
    }
}
