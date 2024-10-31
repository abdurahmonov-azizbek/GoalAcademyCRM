// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class PaymentServiceException : Xeption
    {
        public PaymentServiceException(Exception innerException)
            : base(message: "Payment service error occured, contact support.", innerException)
        { }
    }
}