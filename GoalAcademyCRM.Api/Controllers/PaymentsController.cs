// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Payments;
using GoalAcademyCRM.Api.Models.Payments.Exceptions;
using GoalAcademyCRM.Api.Services.Foundations.Payments;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace GoalAcademyCRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : RESTFulController
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService) =>
            this.paymentService = paymentService;

        [HttpPost]
        public async ValueTask<ActionResult<Payment>> PostPaymentAsync(Payment payment)
        {
            try
            {
                Payment addedPayment = await paymentService.AddPaymentAsync(payment);

                return Created(addedPayment);
            }
            catch (PaymentValidationException paymentValidationException)
            {
                return BadRequest(paymentValidationException.InnerException);
            }
            catch (PaymentDependencyValidationException paymentDependencyValidationException)
                when (paymentDependencyValidationException.InnerException is AlreadyExistsPaymentException)
            {
                return Conflict(paymentDependencyValidationException.InnerException);
            }
            catch (PaymentDependencyException paymentDependencyException)
            {
                return InternalServerError(paymentDependencyException.InnerException);
            }
            catch (PaymentServiceException paymentServiceException)
            {
                return InternalServerError(paymentServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Payment>> GetAllPayments()
        {
            try
            {
                IQueryable<Payment> allPayments = paymentService.RetrieveAllPayments();

                return Ok(allPayments);
            }
            catch (PaymentDependencyException paymentDependencyException)
            {
                return InternalServerError(paymentDependencyException.InnerException);
            }
            catch (PaymentServiceException paymentServiceException)
            {
                return InternalServerError(paymentServiceException.InnerException);
            }
        }

        [HttpGet("{paymentId}")]
        public async ValueTask<ActionResult<Payment>> GetPaymentByIdAsync(Guid paymentId)
        {
            try
            {
                return await paymentService.RetrievePaymentByIdAsync(paymentId);
            }
            catch (PaymentDependencyException paymentDependencyException)
            {
                return InternalServerError(paymentDependencyException);
            }
            catch (PaymentValidationException paymentValidationException)
                when (paymentValidationException.InnerException is InvalidPaymentException)
            {
                return BadRequest(paymentValidationException.InnerException);
            }
            catch (PaymentValidationException paymentValidationException)
                 when (paymentValidationException.InnerException is NotFoundPaymentException)
            {
                return NotFound(paymentValidationException.InnerException);
            }
            catch (PaymentServiceException paymentServiceException)
            {
                return InternalServerError(paymentServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Payment>> PutPaymentAsync(Payment payment)
        {
            try
            {
                Payment modifiedPayment =
                    await paymentService.ModifyPaymentAsync(payment);

                return Ok(modifiedPayment);
            }
            catch (PaymentValidationException paymentValidationException)
                when (paymentValidationException.InnerException is NotFoundPaymentException)
            {
                return NotFound(paymentValidationException.InnerException);
            }
            catch (PaymentValidationException paymentValidationException)
            {
                return BadRequest(paymentValidationException.InnerException);
            }
            catch (PaymentDependencyValidationException paymentDependencyValidationException)
            {
                return BadRequest(paymentDependencyValidationException.InnerException);
            }
            catch (PaymentDependencyException paymentDependencyException)
            {
                return InternalServerError(paymentDependencyException.InnerException);
            }
            catch (PaymentServiceException paymentServiceException)
            {
                return InternalServerError(paymentServiceException.InnerException);
            }
        }

        [HttpDelete("{paymentId}")]
        public async ValueTask<ActionResult<Payment>> DeletePaymentByIdAsync(Guid paymentId)
        {
            try
            {
                Payment deletedPayment = await paymentService.RemovePaymentByIdAsync(paymentId);

                return Ok(deletedPayment);
            }
            catch (PaymentValidationException paymentValidationException)
                when (paymentValidationException.InnerException is NotFoundPaymentException)
            {
                return NotFound(paymentValidationException.InnerException);
            }
            catch (PaymentValidationException paymentValidationException)
            {
                return BadRequest(paymentValidationException.InnerException);
            }
            catch (PaymentDependencyValidationException paymentDependencyValidationException)
                when (paymentDependencyValidationException.InnerException is LockedPaymentException)
            {
                return Locked(paymentDependencyValidationException.InnerException);
            }
            catch (PaymentDependencyValidationException paymentDependencyValidationException)
            {
                return BadRequest(paymentDependencyValidationException.InnerException);
            }
            catch (PaymentDependencyException paymentDependencyException)
            {
                return InternalServerError(paymentDependencyException.InnerException);
            }
            catch (PaymentServiceException paymentServiceException)
            {
                return InternalServerError(paymentServiceException.InnerException);
            }
        }
    }
}