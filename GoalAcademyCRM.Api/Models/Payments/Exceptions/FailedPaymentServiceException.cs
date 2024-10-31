// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class FailedPaymentServiceException : Xeption
    {
        public FailedPaymentServiceException(Exception innerException)
            : base(message: "Failed payment service error occurred, please contact support.", innerException)
        { }
    }
}