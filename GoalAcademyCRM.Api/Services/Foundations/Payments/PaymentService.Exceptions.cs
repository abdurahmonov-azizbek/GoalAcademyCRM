// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;
using GoalAcademyCRM.Api.Models.Payments.Exceptions;
using GoalAcademyCRM.Api.Models.Payments;

namespace GoalAcademyUpdate001.Api.Services.Foundations.Payments
{
    public partial class PaymentService
    {
        private delegate ValueTask<Payment> ReturningPaymentFunction();
        private delegate IQueryable<Payment> ReturningPaymentsFunction();

        private async ValueTask<Payment> TryCatch(ReturningPaymentFunction returningPaymentFunction)
        {
            try
            {
                return await returningPaymentFunction();
            }
            catch (NullPaymentException nullPaymentException)
            {
                throw CreateAndLogValidationException(nullPaymentException);
            }
            catch (InvalidPaymentException invalidPaymentException)
            {
                throw CreateAndLogValidationException(invalidPaymentException);
            }
            catch (NotFoundPaymentException notFoundPaymentException)
            {
                throw CreateAndLogValidationException(notFoundPaymentException);
            }
            catch (SqlException sqlException)
            {
                var failedPaymentStorageException = new FailedPaymentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPaymentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPaymentException = new AlreadyExistsPaymentException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPaymentException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPaymentException = new LockedPaymentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedPaymentException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPaymentStorageException = new FailedPaymentStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedPaymentStorageException);
            }
            catch (Exception exception)
            {
                var failedPaymentServiceException = new FailedPaymentServiceException(exception);

                throw CreateAndLogServiceException(failedPaymentServiceException);
            }
        }

        private IQueryable<Payment> TryCatch(ReturningPaymentsFunction returningPaymentsFunction)
        {
            try
            {
                return returningPaymentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPaymentStorageException = new FailedPaymentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPaymentStorageException);
            }
            catch (Exception serviceException)
            {
                var failedPaymentServiceException = new FailedPaymentServiceException(serviceException);

                throw CreateAndLogServiceException(failedPaymentServiceException);
            }
        }

        private PaymentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var paymentValidationException = new PaymentValidationException(exception);
            this.loggingBroker.LogError(paymentValidationException);

            return paymentValidationException;
        }

        private PaymentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var PaymentDependencyException = new PaymentDependencyException(exception);
            this.loggingBroker.LogCritical(PaymentDependencyException);

            return PaymentDependencyException;
        }

        private PaymentDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var paymentDependencyException = new PaymentDependencyException(exception);
            this.loggingBroker.LogError(paymentDependencyException);

            return paymentDependencyException;
        }


        private PaymentDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var paymentDependencyValidationException = new PaymentDependencyValidationException(exception);
            this.loggingBroker.LogError(paymentDependencyValidationException);

            return paymentDependencyValidationException;
        }

        private PaymentServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var paymentServiceException = new PaymentServiceException(innerException);
            this.loggingBroker.LogError(paymentServiceException);

            return paymentServiceException;
        }
    }
}