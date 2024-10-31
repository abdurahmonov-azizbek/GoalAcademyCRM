// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class LockedPaymentException : Xeption
    {
        public LockedPaymentException(Exception innerException)
            : base(message: "Payment is locked, please try again.", innerException)
        { }
    }
}
