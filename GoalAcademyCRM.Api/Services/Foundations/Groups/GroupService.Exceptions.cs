// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace GoalAcademyCRM.Api.Services.Foundations.Groups
{
    public partial class GroupService
    {
        private delegate ValueTask<Group> ReturningGroupFunction();
        private delegate IQueryable<Group> ReturningGroupsFunction();

        private async ValueTask<Group> TryCatch(ReturningGroupFunction returningGroupFunction)
        {
            try
            {
                return await returningGroupFunction();
            }
            catch (NullGroupException nullGroupException)
            {
                throw CreateAndLogValidationException(nullGroupException);
            }
            catch (InvalidGroupException invalidGroupException)
            {
                throw CreateAndLogValidationException(invalidGroupException);
            }
            catch (NotFoundGroupException notFoundGroupException)
            {
                throw CreateAndLogValidationException(notFoundGroupException);
            }
            catch (SqlException sqlException)
            {
                var failedGroupStorageException = new FailedGroupStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGroupStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGroupException = new AlreadyExistsGroupException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsGroupException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedGroupException = new LockedGroupException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedGroupException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedGroupStorageException = new FailedGroupStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedGroupStorageException);
            }
            catch (Exception exception)
            {
                var failedGroupServiceException = new FailedGroupServiceException(exception);

                throw CreateAndLogServiceException(failedGroupServiceException);
            }
        }

        private IQueryable<Group> TryCatch(ReturningGroupsFunction returningGroupsFunction)
        {
            try
            {
                return returningGroupsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGroupStorageException = new FailedGroupStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGroupStorageException);
            }
            catch (Exception serviceException)
            {
                var failedGroupServiceException = new FailedGroupServiceException(serviceException);

                throw CreateAndLogServiceException(failedGroupServiceException);
            }
        }

        private GroupValidationException CreateAndLogValidationException(Xeption exception)
        {
            var groupValidationException = new GroupValidationException(exception);
            this.loggingBroker.LogError(groupValidationException);

            return groupValidationException;
        }

        private GroupDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var GroupDependencyException = new GroupDependencyException(exception);
            this.loggingBroker.LogCritical(GroupDependencyException);

            return GroupDependencyException;
        }

        private GroupDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var groupDependencyException = new GroupDependencyException(exception);
            this.loggingBroker.LogError(groupDependencyException);

            return groupDependencyException;
        }


        private GroupDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var groupDependencyValidationException = new GroupDependencyValidationException(exception);
            this.loggingBroker.LogError(groupDependencyValidationException);

            return groupDependencyValidationException;
        }

        private GroupServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var groupServiceException = new GroupServiceException(innerException);
            this.loggingBroker.LogError(groupServiceException);

            return groupServiceException;
        }
    }
}