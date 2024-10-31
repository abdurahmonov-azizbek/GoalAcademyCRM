// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;
using GoalAcademyCRM.Api.Models.Attendances.Exceptions;
using GoalAcademyCRM.Api.Services.Foundations.Attendances;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace GoalAcademyCRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : RESTFulController
    {
        private readonly IAttendanceService attendanceService;

        public AttendancesController(IAttendanceService attendanceService) =>
            this.attendanceService = attendanceService;

        [HttpPost]
        public async ValueTask<ActionResult<Attendance>> PostAttendanceAsync(Attendance attendance)
        {
            try
            {
                Attendance addedAttendance = await this.attendanceService.AddAttendanceAsync(attendance);

                return Created(addedAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.InnerException);
            }
            catch (AttendanceDependencyValidationException attendanceDependencyValidationException)
                when (attendanceDependencyValidationException.InnerException is AlreadyExistsAttendanceException)
            {
                return Conflict(attendanceDependencyValidationException.InnerException);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return InternalServerError(attendanceDependencyException.InnerException);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return InternalServerError(attendanceServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Attendance>> GetAllAttendances()
        {
            try
            {
                IQueryable<Attendance> allAttendances = this.attendanceService.RetrieveAllAttendances();

                return Ok(allAttendances);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return InternalServerError(attendanceDependencyException.InnerException);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return InternalServerError(attendanceServiceException.InnerException);
            }
        }

        [HttpGet("{attendanceId}")]
        public async ValueTask<ActionResult<Attendance>> GetAttendanceByIdAsync(Guid attendanceId)
        {
            try
            {
                return await this.attendanceService.RetrieveAttendanceByIdAsync(attendanceId);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return InternalServerError(attendanceDependencyException);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is InvalidAttendanceException)
            {
                return BadRequest(attendanceValidationException.InnerException);
            }
            catch (AttendanceValidationException attendanceValidationException)
                 when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                return NotFound(attendanceValidationException.InnerException);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return InternalServerError(attendanceServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Attendance>> PutAttendanceAsync(Attendance attendance)
        {
            try
            {
                Attendance modifiedAttendance =
                    await this.attendanceService.ModifyAttendanceAsync(attendance);

                return Ok(modifiedAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                return NotFound(attendanceValidationException.InnerException);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.InnerException);
            }
            catch (AttendanceDependencyValidationException attendanceDependencyValidationException)
            {
                return BadRequest(attendanceDependencyValidationException.InnerException);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return InternalServerError(attendanceDependencyException.InnerException);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return InternalServerError(attendanceServiceException.InnerException);
            }
        }

        [HttpDelete("{attendanceId}")]
        public async ValueTask<ActionResult<Attendance>> DeleteAttendanceByIdAsync(Guid attendanceId)
        {
            try
            {
                Attendance deletedAttendance = await this.attendanceService.RemoveAttendanceByIdAsync(attendanceId);

                return Ok(deletedAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                return NotFound(attendanceValidationException.InnerException);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.InnerException);
            }
            catch (AttendanceDependencyValidationException attendanceDependencyValidationException)
                when (attendanceDependencyValidationException.InnerException is LockedAttendanceException)
            {
                return Locked(attendanceDependencyValidationException.InnerException);
            }
            catch (AttendanceDependencyValidationException attendanceDependencyValidationException)
            {
                return BadRequest(attendanceDependencyValidationException.InnerException);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return InternalServerError(attendanceDependencyException.InnerException);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return InternalServerError(attendanceServiceException.InnerException);
            }
        }
    }
}