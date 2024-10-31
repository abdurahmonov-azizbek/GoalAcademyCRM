// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace GoalAcademyCRM.Api.Services.Foundations.Courses
{
    public partial class CourseService
    {
        private delegate ValueTask<Course> ReturningCourseFunction();
        private delegate IQueryable<Course> ReturningCoursesFunction();

        private async ValueTask<Course> TryCatch(ReturningCourseFunction returningCourseFunction)
        {
            try
            {
                return await returningCourseFunction();
            }
            catch (NullCourseException nullCourseException)
            {
                throw CreateAndLogValidationException(nullCourseException);
            }
            catch (InvalidCourseException invalidCourseException)
            {
                throw CreateAndLogValidationException(invalidCourseException);
            }
            catch (NotFoundCourseException notFoundCourseException)
            {
                throw CreateAndLogValidationException(notFoundCourseException);
            }
            catch (SqlException sqlException)
            {
                var failedCourseStorageException = new FailedCourseStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCourseStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCourseException = new AlreadyExistsCourseException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsCourseException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCourseException = new LockedCourseException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedCourseException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedCourseStorageException = new FailedCourseStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedCourseStorageException);
            }
            catch (Exception exception)
            {
                var failedCourseServiceException = new FailedCourseServiceException(exception);

                throw CreateAndLogServiceException(failedCourseServiceException);
            }
        }

        private IQueryable<Course> TryCatch(ReturningCoursesFunction returningCoursesFunction)
        {
            try
            {
                return returningCoursesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedCourseStorageException = new FailedCourseStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCourseStorageException);
            }
            catch (Exception serviceException)
            {
                var failedCourseServiceException = new FailedCourseServiceException(serviceException);

                throw CreateAndLogServiceException(failedCourseServiceException);
            }
        }

        private CourseValidationException CreateAndLogValidationException(Xeption exception)
        {
            var courseValidationException = new CourseValidationException(exception);
            this.loggingBroker.LogError(courseValidationException);

            return courseValidationException;
        }

        private CourseDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var CourseDependencyException = new CourseDependencyException(exception);
            this.loggingBroker.LogCritical(CourseDependencyException);

            return CourseDependencyException;
        }

        private CourseDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var courseDependencyException = new CourseDependencyException(exception);
            this.loggingBroker.LogError(courseDependencyException);

            return courseDependencyException;
        }


        private CourseDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var courseDependencyValidationException = new CourseDependencyValidationException(exception);
            this.loggingBroker.LogError(courseDependencyValidationException);

            return courseDependencyValidationException;
        }

        private CourseServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var courseServiceException = new CourseServiceException(innerException);
            this.loggingBroker.LogError(courseServiceException);

            return courseServiceException;
        }
    }
}