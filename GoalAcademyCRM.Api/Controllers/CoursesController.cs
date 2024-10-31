// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;
using GoalAcademyCRM.Api.Services.Foundations.Courses;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace GoalAcademyCRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : RESTFulController
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService) =>
            this.courseService = courseService;

        [HttpPost]
        public async ValueTask<ActionResult<Course>> PostCourseAsync(Course course)
        {
            try
            {
                Course addedCourse = await this.courseService.AddCourseAsync(course);

                return Created(addedCourse);
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.InnerException);
            }
            catch (CourseDependencyValidationException courseDependencyValidationException)
                when (courseDependencyValidationException.InnerException is AlreadyExistsCourseException)
            {
                return Conflict(courseDependencyValidationException.InnerException);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return InternalServerError(courseDependencyException.InnerException);
            }
            catch (CourseServiceException courseServiceException)
            {
                return InternalServerError(courseServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Course>> GetAllCourses()
        {
            try
            {
                IQueryable<Course> allCourses = this.courseService.RetrieveAllCourses();

                return Ok(allCourses);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return InternalServerError(courseDependencyException.InnerException);
            }
            catch (CourseServiceException courseServiceException)
            {
                return InternalServerError(courseServiceException.InnerException);
            }
        }

        [HttpGet("{courseId}")]
        public async ValueTask<ActionResult<Course>> GetCourseByIdAsync(Guid courseId)
        {
            try
            {
                return await this.courseService.RetrieveCourseByIdAsync(courseId);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return InternalServerError(courseDependencyException);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is InvalidCourseException)
            {
                return BadRequest(courseValidationException.InnerException);
            }
            catch (CourseValidationException courseValidationException)
                 when (courseValidationException.InnerException is NotFoundCourseException)
            {
                return NotFound(courseValidationException.InnerException);
            }
            catch (CourseServiceException courseServiceException)
            {
                return InternalServerError(courseServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Course>> PutCourseAsync(Course course)
        {
            try
            {
                Course modifiedCourse =
                    await this.courseService.ModifyCourseAsync(course);

                return Ok(modifiedCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is NotFoundCourseException)
            {
                return NotFound(courseValidationException.InnerException);
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.InnerException);
            }
            catch (CourseDependencyValidationException courseDependencyValidationException)
            {
                return BadRequest(courseDependencyValidationException.InnerException);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return InternalServerError(courseDependencyException.InnerException);
            }
            catch (CourseServiceException courseServiceException)
            {
                return InternalServerError(courseServiceException.InnerException);
            }
        }

        [HttpDelete("{courseId}")]
        public async ValueTask<ActionResult<Course>> DeleteCourseByIdAsync(Guid courseId)
        {
            try
            {
                Course deletedCourse = await this.courseService.RemoveCourseByIdAsync(courseId);

                return Ok(deletedCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is NotFoundCourseException)
            {
                return NotFound(courseValidationException.InnerException);
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.InnerException);
            }
            catch (CourseDependencyValidationException courseDependencyValidationException)
                when (courseDependencyValidationException.InnerException is LockedCourseException)
            {
                return Locked(courseDependencyValidationException.InnerException);
            }
            catch (CourseDependencyValidationException courseDependencyValidationException)
            {
                return BadRequest(courseDependencyValidationException.InnerException);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return InternalServerError(courseDependencyException.InnerException);
            }
            catch (CourseServiceException courseServiceException)
            {
                return InternalServerError(courseServiceException.InnerException);
            }
        }
    }
}