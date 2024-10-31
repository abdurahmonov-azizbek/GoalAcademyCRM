// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;
using GoalAcademyCRM.Api.Models.StudentGroups.Exceptions;
using GoalAcademyCRM.Api.Services.Foundations.StudentGroups;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace GoalAcademyCRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentGroupsController : RESTFulController
    {
        private readonly IStudentGroupService studentGroupService;

        public StudentGroupsController(IStudentGroupService studentGroupService) =>
            this.studentGroupService = studentGroupService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentGroup>> PostStudentGroupAsync(StudentGroup studentGroup)
        {
            try
            {
                StudentGroup addedStudentGroup = await this.studentGroupService.AddStudentGroupAsync(studentGroup);

                return Created(addedStudentGroup);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
            {
                return BadRequest(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupDependencyValidationException studentGroupDependencyValidationException)
                when (studentGroupDependencyValidationException.InnerException is AlreadyExistsStudentGroupException)
            {
                return Conflict(studentGroupDependencyValidationException.InnerException);
            }
            catch (StudentGroupDependencyException studentGroupDependencyException)
            {
                return InternalServerError(studentGroupDependencyException.InnerException);
            }
            catch (StudentGroupServiceException studentGroupServiceException)
            {
                return InternalServerError(studentGroupServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentGroup>> GetAllStudentGroups()
        {
            try
            {
                IQueryable<StudentGroup> allStudentGroups = this.studentGroupService.RetrieveAllStudentGroups();

                return Ok(allStudentGroups);
            }
            catch (StudentGroupDependencyException studentGroupDependencyException)
            {
                return InternalServerError(studentGroupDependencyException.InnerException);
            }
            catch (StudentGroupServiceException studentGroupServiceException)
            {
                return InternalServerError(studentGroupServiceException.InnerException);
            }
        }

        [HttpGet("{studentGroupId}")]
        public async ValueTask<ActionResult<StudentGroup>> GetStudentGroupByIdAsync(Guid studentGroupId)
        {
            try
            {
                return await this.studentGroupService.RetrieveStudentGroupByIdAsync(studentGroupId);
            }
            catch (StudentGroupDependencyException studentGroupDependencyException)
            {
                return InternalServerError(studentGroupDependencyException);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
                when (studentGroupValidationException.InnerException is InvalidStudentGroupException)
            {
                return BadRequest(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
                 when (studentGroupValidationException.InnerException is NotFoundStudentGroupException)
            {
                return NotFound(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupServiceException studentGroupServiceException)
            {
                return InternalServerError(studentGroupServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<StudentGroup>> PutStudentGroupAsync(StudentGroup studentGroup)
        {
            try
            {
                StudentGroup modifiedStudentGroup =
                    await this.studentGroupService.ModifyStudentGroupAsync(studentGroup);

                return Ok(modifiedStudentGroup);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
                when (studentGroupValidationException.InnerException is NotFoundStudentGroupException)
            {
                return NotFound(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
            {
                return BadRequest(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupDependencyValidationException studentGroupDependencyValidationException)
            {
                return BadRequest(studentGroupDependencyValidationException.InnerException);
            }
            catch (StudentGroupDependencyException studentGroupDependencyException)
            {
                return InternalServerError(studentGroupDependencyException.InnerException);
            }
            catch (StudentGroupServiceException studentGroupServiceException)
            {
                return InternalServerError(studentGroupServiceException.InnerException);
            }
        }

        [HttpDelete("{studentGroupId}")]
        public async ValueTask<ActionResult<StudentGroup>> DeleteStudentGroupByIdAsync(Guid studentGroupId)
        {
            try
            {
                StudentGroup deletedStudentGroup = await this.studentGroupService.RemoveStudentGroupByIdAsync(studentGroupId);

                return Ok(deletedStudentGroup);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
                when (studentGroupValidationException.InnerException is NotFoundStudentGroupException)
            {
                return NotFound(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupValidationException studentGroupValidationException)
            {
                return BadRequest(studentGroupValidationException.InnerException);
            }
            catch (StudentGroupDependencyValidationException studentGroupDependencyValidationException)
                when (studentGroupDependencyValidationException.InnerException is LockedStudentGroupException)
            {
                return Locked(studentGroupDependencyValidationException.InnerException);
            }
            catch (StudentGroupDependencyValidationException studentGroupDependencyValidationException)
            {
                return BadRequest(studentGroupDependencyValidationException.InnerException);
            }
            catch (StudentGroupDependencyException studentGroupDependencyException)
            {
                return InternalServerError(studentGroupDependencyException.InnerException);
            }
            catch (StudentGroupServiceException studentGroupServiceException)
            {
                return InternalServerError(studentGroupServiceException.InnerException);
            }
        }
    }
}