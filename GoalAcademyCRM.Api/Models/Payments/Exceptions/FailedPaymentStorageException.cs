// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class FailedPaymentStorageException : Xeption
    {
        public FailedPaymentStorageException(Exception innerException)
            : base(message: "Failed payment storage error occurred, contact support.", innerException)
        { }
    }
}