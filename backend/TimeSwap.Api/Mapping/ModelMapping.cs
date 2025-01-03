using AutoMapper;
using TimeSwap.Api.Models;
using TimeSwap.Application.JobApplicants.Commands;
using TimeSwap.Application.JobPosts.Commands;

namespace TimeSwap.Api.Mapping
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<CreateJobPostRequest, CreateJobPostCommand>();
            CreateMap<UpdateJobPostRequest, UpdateJobPostCommand>();
            CreateMap<AssignJobPostRequest, AssignJobCommand>();
            CreateMap<CreateJobApplicantRequest, CreateJobApplicantCommand>();
        }
    }
}
