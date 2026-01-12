using AutoMapper;
using REST_API.DTOs;
using REST_API.Models;

namespace REST_API.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // Source -> Target
            CreateMap<Student, StudentReadDto>();
            CreateMap<StudentCreateDto, Student>();
            CreateMap<StudentUpdateDto, Student>();
        }
    }
}
