// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using GoalAcademyCRM.Api.Models.Payments;
using GoalAcademyCRM.Api.Models.Payments.Exceptions;
using System;

namespace GoalAcademyUpdate001.Api.Services.Foundations.Payments
{
    public partial class PaymentService
    {
        private void ValidatePaymentOnAdd(Payment payment)
        {
            ValidatePaymentNotNull(payment);

            Validate(
				(Rule: IsInvalid(payment.Id), Parameter: nameof(Payment.Id)),
				(Rule: IsInvalid(payment.StudentId), Parameter: nameof(Payment.StudentId)),
				(Rule: IsInvalid(payment.CreatedDate), Parameter: nameof(Payment.CreatedDate)),
				(Rule: IsInvalid(payment.UpdatedDate), Parameter: nameof(Payment.UpdatedDate)),

                (Rule: IsNotRecent(payment.CreatedDate), Parameter: nameof(Payment.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: payment.CreatedDate,
                    secondDate: payment.UpdatedDate,
                    secondDateName: nameof(Payment.UpdatedDate)),

                    Parameter: nameof(Payment.CreatedDate)));
        }

        private void ValidatePaymentOnModify(Payment payment)
        {
            ValidatePaymentNotNull(payment);

            Validate(
				(Rule: IsInvalid(payment.Id), Parameter: nameof(Payment.Id)),
				(Rule: IsInvalid(payment.StudentId), Parameter: nameof(Payment.StudentId)),
				(Rule: IsInvalid(payment.CreatedDate), Parameter: nameof(Payment.CreatedDate)),
				(Rule: IsInvalid(payment.UpdatedDate), Parameter: nameof(Payment.UpdatedDate)),

                (Rule: IsNotRecent(payment.UpdatedDate), Parameter: nameof(Payment.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: payment.UpdatedDate,
                    secondDate: payment.CreatedDate,
                    secondDateName: nameof(payment.CreatedDate)),
                    Parameter: nameof(payment.UpdatedDate)));
        }

        private static void ValidateAgainstStoragePaymentOnModify(Payment inputPayment, Payment storagePayment)
        {
            ValidateStoragePayment(storagePayment, inputPayment.Id);
            Validate(
            (Rule: IsNotSame(
                    firstDate: inputPayment.CreatedDate,
                    secondDate: storagePayment.CreatedDate,
                    secondDateName: nameof(Payment.CreatedDate)),
                    Parameter: nameof(Payment.CreatedDate)),

                     (Rule: IsSame(
                        firstDate: inputPayment.UpdatedDate,
                        secondDate: storagePayment.UpdatedDate,
                        secondDateName: nameof(Payment.UpdatedDate)),
                        Parameter: nameof(Payment.UpdatedDate)));
        }

        private static void ValidateStoragePayment(Payment maybePayment, Guid paymentId)
        {
            if (maybePayment is null)
            {
                throw new NotFoundPaymentException(paymentId);
            }
        }

        private void ValidatePaymentId(Guid paymentId) =>
             Validate((Rule: IsInvalid(paymentId), Parameter: nameof(Payment.Id)));

        private void ValidatePaymentNotNull(Payment payment)
        {
            if (payment is null)
            {
                throw new NullPaymentException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDate.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPaymentException = new InvalidPaymentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPaymentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidPaymentException.ThrowIfContainsErrors();
        }
    }
}
