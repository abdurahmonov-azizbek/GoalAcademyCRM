// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class PaymentDependencyException : Xeption
    {
        public PaymentDependencyException(Exception innerException)
            : base(message: "Payment dependency error occured, contact support.", innerException)
        { }
    }
}