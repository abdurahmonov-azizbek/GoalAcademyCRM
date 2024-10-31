// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;
using GoalAcademyCRM.Api.Models.Attendances.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace GoalAcademyCRM.Api.Services.Foundations.Attendances
{
    public partial class AttendanceService
    {
        private delegate ValueTask<Attendance> ReturningAttendanceFunction();
        private delegate IQueryable<Attendance> ReturningAttendancesFunction();

        private async ValueTask<Attendance> TryCatch(ReturningAttendanceFunction returningAttendanceFunction)
        {
            try
            {
                return await returningAttendanceFunction();
            }
            catch (NullAttendanceException nullAttendanceException)
            {
                throw CreateAndLogValidationException(nullAttendanceException);
            }
            catch (InvalidAttendanceException invalidAttendanceException)
            {
                throw CreateAndLogValidationException(invalidAttendanceException);
            }
            catch (NotFoundAttendanceException notFoundAttendanceException)
            {
                throw CreateAndLogValidationException(notFoundAttendanceException);
            }
            catch (SqlException sqlException)
            {
                var failedAttendanceStorageException = new FailedAttendanceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttendanceStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAttendanceException = new AlreadyExistsAttendanceException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAttendanceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAttendanceException = new LockedAttendanceException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAttendanceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAttendanceStorageException = new FailedAttendanceStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedAttendanceStorageException);
            }
            catch (Exception exception)
            {
                var failedAttendanceServiceException = new FailedAttendanceServiceException(exception);

                throw CreateAndLogServiceException(failedAttendanceServiceException);
            }
        }

        private IQueryable<Attendance> TryCatch(ReturningAttendancesFunction returningAttendancesFunction)
        {
            try
            {
                return returningAttendancesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAttendanceStorageException = new FailedAttendanceStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttendanceStorageException);
            }
            catch (Exception serviceException)
            {
                var failedAttendanceServiceException = new FailedAttendanceServiceException(serviceException);

                throw CreateAndLogServiceException(failedAttendanceServiceException);
            }
        }

        private AttendanceValidationException CreateAndLogValidationException(Xeption exception)
        {
            var attendanceValidationException = new AttendanceValidationException(exception);
            this.loggingBroker.LogError(attendanceValidationException);

            return attendanceValidationException;
        }

        private AttendanceDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var AttendanceDependencyException = new AttendanceDependencyException(exception);
            this.loggingBroker.LogCritical(AttendanceDependencyException);

            return AttendanceDependencyException;
        }

        private AttendanceDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var attendanceDependencyException = new AttendanceDependencyException(exception);
            this.loggingBroker.LogError(attendanceDependencyException);

            return attendanceDependencyException;
        }


        private AttendanceDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var attendanceDependencyValidationException = new AttendanceDependencyValidationException(exception);
            this.loggingBroker.LogError(attendanceDependencyValidationException);

            return attendanceDependencyValidationException;
        }

        private AttendanceServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var attendanceServiceException = new AttendanceServiceException(innerException);
            this.loggingBroker.LogError(attendanceServiceException);

            return attendanceServiceException;
        }
    }
}