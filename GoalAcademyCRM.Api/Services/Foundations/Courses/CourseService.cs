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
using GoalAcademyCRM.Api.Models.Courses;

namespace GoalAcademyCRM.Api.Services.Foundations.Courses
{
    public partial class CourseService : ICourseService
    {

        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CourseService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Course> AddCourseAsync(Course course) =>
        TryCatch(async () =>
        {
            ValidateCourseOnAdd(course);

            return await this.storageBroker.InsertCourseAsync(course);
        });

        public IQueryable<Course> RetrieveAllCourses() =>
            TryCatch(() => this.storageBroker.SelectAllCourses());

        public ValueTask<Course> RetrieveCourseByIdAsync(Guid courseId) =>
           TryCatch(async () =>
           {
               ValidateCourseId(courseId);

               Course maybeCourse =
                   await storageBroker.SelectCourseByIdAsync(courseId);

               ValidateStorageCourse(maybeCourse, courseId);

               return maybeCourse;
           });

        public ValueTask<Course> ModifyCourseAsync(Course course) =>
            TryCatch(async () =>
            {
                ValidateCourseOnModify(course);

                Course maybeCourse =
                    await this.storageBroker.SelectCourseByIdAsync(course.Id);

                ValidateAgainstStorageCourseOnModify(inputCourse: course, storageCourse: maybeCourse);

                return await this.storageBroker.UpdateCourseAsync(course);
            });

        public ValueTask<Course> RemoveCourseByIdAsync(Guid courseId) =>
           TryCatch(async () =>
           {
               ValidateCourseId(courseId);

               Course maybeCourse =
                   await this.storageBroker.SelectCourseByIdAsync(courseId);

               ValidateStorageCourse(maybeCourse, courseId);

               return await this.storageBroker.DeleteCourseAsync(maybeCourse);
           });
    }
}