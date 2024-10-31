// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Brokers.DateTimes;
using GoalAcademyCRM.Api.Brokers.Loggings;
using GoalAcademyCRM.Api.Brokers.Storages;
using GoalAcademyCRM.Api.Models.Payments;
using GoalAcademyCRM.Api.Services.Foundations.Payments;

namespace GoalAcademyUpdate001.Api.Services.Foundations.Payments
{
    public partial class PaymentService : IPaymentService
    {

        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PaymentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Payment> AddPaymentAsync(Payment payment) =>
        TryCatch(async () =>
        {
            ValidatePaymentOnAdd(payment);

            return await this.storageBroker.InsertPaymentAsync(payment);
        });

        public IQueryable<Payment> RetrieveAllPayments() =>
            TryCatch(() => this.storageBroker.SelectAllPayments());

        public ValueTask<Payment> RetrievePaymentByIdAsync(Guid paymentId) =>
           TryCatch(async () =>
           {
               ValidatePaymentId(paymentId);

               Payment maybePayment =
                   await storageBroker.SelectPaymentByIdAsync(paymentId);

               ValidateStoragePayment(maybePayment, paymentId);

               return maybePayment;
           });

        public ValueTask<Payment> ModifyPaymentAsync(Payment payment) =>
            TryCatch(async () =>
            {
                ValidatePaymentOnModify(payment);

                Payment maybePayment =
                    await this.storageBroker.SelectPaymentByIdAsync(payment.Id);

                ValidateAgainstStoragePaymentOnModify(inputPayment: payment, storagePayment: maybePayment);

                return await this.storageBroker.UpdatePaymentAsync(payment);
            });

        public ValueTask<Payment> RemovePaymentByIdAsync(Guid paymentId) =>
           TryCatch(async () =>
           {
               ValidatePaymentId(paymentId);

               Payment maybePayment =
                   await this.storageBroker.SelectPaymentByIdAsync(paymentId);

               ValidateStoragePayment(maybePayment, paymentId);

               return await this.storageBroker.DeletePaymentAsync(maybePayment);
           });
    }
}