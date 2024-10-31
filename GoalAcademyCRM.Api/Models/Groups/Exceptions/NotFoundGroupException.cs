// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class NotFoundGroupException : Xeption
    {
        public NotFoundGroupException(Guid groupId)
            : base(message: $"Couldn't find group with id: {groupId}.")
        { }
    }
}
