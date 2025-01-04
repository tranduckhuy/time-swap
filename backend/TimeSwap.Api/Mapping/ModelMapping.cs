using AutoMapper;
using TimeSwap.Api.Models;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Application.JobApplicants.Commands;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.Payments.Commands;

namespace TimeSwap.Api.Mapping
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<CreateJobPostRequest, CreateJobPostCommand>();
            CreateMap<UpdateJobPostRequest, UpdateJobPostCommand>();
            CreateMap<AssignJobPostRequest, AssignJobCommand>();
            CreateMap<CreatePaymentRequest, CreatePaymentCommand>();
            CreateMap<CreateIndustryRequest, CreateIndustryCommand>();
            CreateMap<UpdateIndustryRequest, UpdateIndustryCommand>();
            CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
            CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>();
            CreateMap<CreateJobApplicantRequest, CreateJobApplicantCommand>();
        }
    }
}
