// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class LockedGroupException : Xeption
    {
        public LockedGroupException(Exception innerException)
            : base(message: "Group is locked, please try again.", innerException)
        { }
    }
}
