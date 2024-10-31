// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;
using GoalAcademyCRM.Api.Models.StudentGroups.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace GoalAcademyCRM.Api.Services.Foundations.StudentGroups
{
    public partial class StudentGroupService
    {
        private delegate ValueTask<StudentGroup> ReturningStudentGroupFunction();
        private delegate IQueryable<StudentGroup> ReturningStudentGroupsFunction();

        private async ValueTask<StudentGroup> TryCatch(ReturningStudentGroupFunction returningStudentGroupFunction)
        {
            try
            {
                return await returningStudentGroupFunction();
            }
            catch (NullStudentGroupException nullStudentGroupException)
            {
                throw CreateAndLogValidationException(nullStudentGroupException);
            }
            catch (InvalidStudentGroupException invalidStudentGroupException)
            {
                throw CreateAndLogValidationException(invalidStudentGroupException);
            }
            catch (NotFoundStudentGroupException notFoundStudentGroupException)
            {
                throw CreateAndLogValidationException(notFoundStudentGroupException);
            }
            catch (SqlException sqlException)
            {
                var failedStudentGroupStorageException = new FailedStudentGroupStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentGroupStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentGroupException = new AlreadyExistsStudentGroupException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsStudentGroupException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedStudentGroupException = new LockedStudentGroupException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedStudentGroupException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedStudentGroupStorageException = new FailedStudentGroupStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedStudentGroupStorageException);
            }
            catch (Exception exception)
            {
                var failedStudentGroupServiceException = new FailedStudentGroupServiceException(exception);

                throw CreateAndLogServiceException(failedStudentGroupServiceException);
            }
        }

        private IQueryable<StudentGroup> TryCatch(ReturningStudentGroupsFunction returningStudentGroupsFunction)
        {
            try
            {
                return returningStudentGroupsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStudentGroupStorageException = new FailedStudentGroupStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentGroupStorageException);
            }
            catch (Exception serviceException)
            {
                var failedStudentGroupServiceException = new FailedStudentGroupServiceException(serviceException);

                throw CreateAndLogServiceException(failedStudentGroupServiceException);
            }
        }

        private StudentGroupValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentGroupValidationException = new StudentGroupValidationException(exception);
            this.loggingBroker.LogError(studentGroupValidationException);

            return studentGroupValidationException;
        }

        private StudentGroupDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var StudentGroupDependencyException = new StudentGroupDependencyException(exception);
            this.loggingBroker.LogCritical(StudentGroupDependencyException);

            return StudentGroupDependencyException;
        }

        private StudentGroupDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentGroupDependencyException = new StudentGroupDependencyException(exception);
            this.loggingBroker.LogError(studentGroupDependencyException);

            return studentGroupDependencyException;
        }


        private StudentGroupDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var studentGroupDependencyValidationException = new StudentGroupDependencyValidationException(exception);
            this.loggingBroker.LogError(studentGroupDependencyValidationException);

            return studentGroupDependencyValidationException;
        }

        private StudentGroupServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var studentGroupServiceException = new StudentGroupServiceException(innerException);
            this.loggingBroker.LogError(studentGroupServiceException);

            return studentGroupServiceException;
        }
    }
}