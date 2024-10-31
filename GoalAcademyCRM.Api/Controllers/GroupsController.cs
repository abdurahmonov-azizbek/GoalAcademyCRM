// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using GoalAcademyCRM.Api.Services.Foundations.Groups;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace GoalAcademyCRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : RESTFulController
    {
        private readonly IGroupService groupService;

        public GroupsController(IGroupService groupService) =>
            this.groupService = groupService;

        [HttpPost]
        public async ValueTask<ActionResult<Group>> PostGroupAsync(Group group)
        {
            try
            {
                Group addedGroup = await this.groupService.AddGroupAsync(group);

                return Created(addedGroup);
            }
            catch (GroupValidationException groupValidationException)
            {
                return BadRequest(groupValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
                when (groupDependencyValidationException.InnerException is AlreadyExistsGroupException)
            {
                return Conflict(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Group>> GetAllGroups()
        {
            try
            {
                IQueryable<Group> allGroups = this.groupService.RetrieveAllGroups();

                return Ok(allGroups);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }

        [HttpGet("{groupId}")]
        public async ValueTask<ActionResult<Group>> GetGroupByIdAsync(Guid groupId)
        {
            try
            {
                return await this.groupService.RetrieveGroupByIdAsync(groupId);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException);
            }
            catch (GroupValidationException groupValidationException)
                when (groupValidationException.InnerException is InvalidGroupException)
            {
                return BadRequest(groupValidationException.InnerException);
            }
            catch (GroupValidationException groupValidationException)
                 when (groupValidationException.InnerException is NotFoundGroupException)
            {
                return NotFound(groupValidationException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Group>> PutGroupAsync(Group group)
        {
            try
            {
                Group modifiedGroup =
                    await this.groupService.ModifyGroupAsync(group);

                return Ok(modifiedGroup);
            }
            catch (GroupValidationException groupValidationException)
                when (groupValidationException.InnerException is NotFoundGroupException)
            {
                return NotFound(groupValidationException.InnerException);
            }
            catch (GroupValidationException groupValidationException)
            {
                return BadRequest(groupValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
            {
                return BadRequest(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }

        [HttpDelete("{groupId}")]
        public async ValueTask<ActionResult<Group>> DeleteGroupByIdAsync(Guid groupId)
        {
            try
            {
                Group deletedGroup = await this.groupService.RemoveGroupByIdAsync(groupId);

                return Ok(deletedGroup);
            }
            catch (GroupValidationException groupValidationException)
                when (groupValidationException.InnerException is NotFoundGroupException)
            {
                return NotFound(groupValidationException.InnerException);
            }
            catch (GroupValidationException groupValidationException)
            {
                return BadRequest(groupValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
                when (groupDependencyValidationException.InnerException is LockedGroupException)
            {
                return Locked(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyValidationException groupDependencyValidationException)
            {
                return BadRequest(groupDependencyValidationException.InnerException);
            }
            catch (GroupDependencyException groupDependencyException)
            {
                return InternalServerError(groupDependencyException.InnerException);
            }
            catch (GroupServiceException groupServiceException)
            {
                return InternalServerError(groupServiceException.InnerException);
            }
        }
    }
}