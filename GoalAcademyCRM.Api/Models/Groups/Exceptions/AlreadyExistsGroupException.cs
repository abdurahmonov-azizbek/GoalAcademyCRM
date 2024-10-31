// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class AlreadyExistsGroupException : Xeption
    {
        public AlreadyExistsGroupException(Exception innerException)
            : base(message: "Group already exists.", innerException)
        { }
    }
}
