// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;

namespace GoalAcademyCRM.Api.Services.Foundations.Courses
{
    public interface ICourseService  
    {
        /// <exception cref="Models.Courses.Exceptions.CourseValidationException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseDependencyValidationException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseDependencyException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseServiceException"></exception>
        ValueTask<Course> AddCourseAsync(Course course);

        /// <exception cref="Models.Courses.Exceptions.CourseDependencyException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseServiceException"></exception>
        IQueryable<Course> RetrieveAllCourses();

        /// <exception cref="Models.Courses.Exceptions.CourseDependencyException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseServiceException"></exception>
        ValueTask<Course> RetrieveCourseByIdAsync(Guid courseId);

        /// <exception cref="Models.Courses.Exceptions.CourseValidationException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseDependencyValidationException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseDependencyException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseServiceException"></exception>
        ValueTask<Course> ModifyCourseAsync(Course course);

        /// <exception cref="Models.Courses.Exceptions.CourseDependencyValidationException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseDependencyException"></exception>
        /// <exception cref="Models.Courses.Exceptions.CourseServiceException"></exception>
        ValueTask<Course> RemoveCourseByIdAsync(Guid courseId);
    }
}