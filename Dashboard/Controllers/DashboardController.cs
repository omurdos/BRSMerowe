using System.Text.Json;
using AutoMapper;
using Core.Entities;
using Dashboard.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<PushNotificationsController> _logger;
        private readonly TSTDBContext _dbContext;

        private readonly IMapper _mapper;
        public DashboardController(ILogger<PushNotificationsController> logger, TSTDBContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index(int? page)
        {

            //Count of registered students
            var registeredStudentsCount = await _dbContext.Students.AsNoTracking().CountAsync(s => s.IsERegistrationComplete);
            var unregisteredStudentCount = await _dbContext.Students.AsNoTracking().CountAsync(s => s.IsERegistrationComplete == false);
            var allStudentsCount = await _dbContext.Students.AsNoTracking().CountAsync();


            var allStudentsWithNoPhotos = await _dbContext.Students.AsNoTracking().CountAsync( s => string.IsNullOrEmpty(s.PersonalPhoto) );




            //Count of students per faculty
            var studentsPerFacultydata = await _dbContext.Students
                                                        .Join(
                                                            _dbContext.Faculties,
                                                            student => student.FacultyNumber,
                                                            faculty => faculty.FacultyNumber,
                                                            (student, faculty) => new { student, faculty }
                                                        )
                                                        .GroupBy(x => x.faculty.FacultyNameE)
                                                        .Select(g => new
                                                        {
                                                            FacultyNameE = g.Key,
                                                            StudentsCount = g.Count()
                                                        })
                                                        .ToListAsync();

            // Extract labels and values
            var labels = studentsPerFacultydata.Select(d => d.FacultyNameE).ToList();
            var values = studentsPerFacultydata.Select(d => d.StudentsCount).ToList();

            //Count of devices per manufacturer
            var devicesPerManufacturer = await _dbContext.Devices

                                                        .GroupBy(x => x.Manufacturer)
                                                        .Select(g => new
                                                        {
                                                            Manufacturer = g.Key,
                                                            ManufacturerCount = g.Count()
                                                        })
                                                        .ToListAsync();

            // Extract labels and values
            var devicesPerManufacturerLabels = devicesPerManufacturer.Select(d => d.Manufacturer).ToList();
            var devicesPerManufacturerValues = devicesPerManufacturer.Select(d => d.ManufacturerCount).ToList();


            //Count of certificate requests per status
            var certificatesRequestPerStatus = await _dbContext.CertificateRequests

                                                        .Join(
                                                            _dbContext.RequestStatuses,
                                                            certificatesRequest => certificatesRequest.RequestStatusId,
                                                            requestStatus => requestStatus.Id.ToString(),
                                                            (certificatesRequest, requestStatus) => new { certificatesRequest, requestStatus }
                                                        )
                                                        .GroupBy(x => x.requestStatus.Name)
                                                        .Select(g => new
                                                        {
                                                            statuses = g.Key,
                                                            statusesCount = g.Count()
                                                        })
                                                        .ToListAsync();

            // Extract labels and values
            var certificatesRequestPerStatusLabels = certificatesRequestPerStatus.Select(d => d.statuses).ToList();
            var certificatesRequestPerStatusValues = certificatesRequestPerStatus.Select(d => d.statusesCount).ToList();


            //Count of certificate requests per status
            var transcriptRequestPerStatus = await _dbContext.TranscriptRequests

                                                        .Join(
                                                            _dbContext.RequestStatuses,
                                                            transcriptRequest => transcriptRequest.RequestStatusId,
                                                            requestStatus => requestStatus.Id.ToString(),
                                                            (transcriptRequest, requestStatus) => new { transcriptRequest, requestStatus }
                                                        )
                                                        .GroupBy(x => x.requestStatus.Name)
                                                        .Select(g => new
                                                        {
                                                            statuses = g.Key,
                                                            statusesCount = g.Count()
                                                        })
                                                        .ToListAsync();

            // Extract labels and values
            var transcriptRequestPerStatusPerStatusLabels = transcriptRequestPerStatus.Select(d => d.statuses).ToList();
            var transcriptRequestPerStatusPerStatusValues = transcriptRequestPerStatus.Select(d => d.statusesCount).ToList();


            //Count of certificate requests per status
            var EnrollmentRequestPerStatus = await _dbContext.EnrollmentRequests

                                                        .Join(
                                                            _dbContext.RequestStatuses,
                                                            enrollmentRequest => enrollmentRequest.RequestStatusId,
                                                            requestStatus => requestStatus.Id.ToString(),
                                                            (enrollmentRequest, requestStatus) => new { enrollmentRequest, requestStatus }
                                                        )
                                                        .GroupBy(x => x.requestStatus.Name)
                                                        .Select(g => new
                                                        {
                                                            statuses = g.Key,
                                                            statusesCount = g.Count()
                                                        })
                                                        .ToListAsync();

            // Extract labels and values
            var enrollmentRequestPerStatusPerStatusLabels = EnrollmentRequestPerStatus.Select(d => d.statuses).ToList();
            var enrollmentRequestPerStatusPerStatusValues = EnrollmentRequestPerStatus.Select(d => d.statusesCount).ToList();



            // Pass to ViewBag (serialize to JSON for direct embedding in JS)
            ViewBag.LabelsJson = JsonSerializer.Serialize(labels);
            ViewBag.DataJson = JsonSerializer.Serialize(values);

            ViewBag.devicesPerManufacturerLabels = JsonSerializer.Serialize(devicesPerManufacturerLabels);
            ViewBag.devicesPerManufacturerValues = JsonSerializer.Serialize(devicesPerManufacturerValues);


            // Certificate Requests
            ViewBag.CertificateRequestLabelsJson = JsonSerializer.Serialize(certificatesRequestPerStatusLabels);
            ViewBag.CertificateRequestDataJson = JsonSerializer.Serialize(certificatesRequestPerStatusValues);

            // Transcript Requests
            ViewBag.TranscriptRequestLabelsJson = JsonSerializer.Serialize(transcriptRequestPerStatusPerStatusLabels);
            ViewBag.TranscriptRequestDataJson = JsonSerializer.Serialize(transcriptRequestPerStatusPerStatusValues);

              // Transcript Requests
            ViewBag.EnrollmentRequestLabelsJson = JsonSerializer.Serialize(enrollmentRequestPerStatusPerStatusLabels);
            ViewBag.EnrollmentRequestDataJson = JsonSerializer.Serialize(enrollmentRequestPerStatusPerStatusValues);

            ViewBag.registeredStudentsCount = registeredStudentsCount;
            ViewBag.unregisteredStudentCount = unregisteredStudentCount;

            ViewBag.allStudentsCount = allStudentsCount;
            ViewBag.allStudentsWithNoPhotos = allStudentsWithNoPhotos;
            

            


            return View();
        }
    }
}
