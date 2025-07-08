using AutoMapper;
using System.Xml.Serialization;
using TMS.Application.Commands;
using TMS.Application.DTOs.Requests;
using TMS.Application.DTOs.Responses;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;

namespace TMS.Application.Mappings
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateTaskMappings();
            CreateProjectMappings();
            CreateUserMappings();
            CreateCommandMappings();
            CreateQueryMappings();
        }

        private void CreateTaskMappings()
        {
            CreateMap<Domain.Entities.Task, TaskResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.Value))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser.Name));
        }

        private void CreateProjectMappings()
        {
            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.GetTaskCount()))
                .ForMember(dest => dest.CompletedTaskCount, opt => opt.MapFrom(src => src.GetCompletedTaskCount()))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue()));
        }

        private void CreateUserMappings()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.ProjectCount, opt => opt.MapFrom(src => src.Projects.Count))
                .ForMember(dest => dest.AssignedTaskCount, opt => opt.MapFrom(src => src.AssignedTasks.Count));
        }

        private void CreateCommandMappings()
        {
            CreateMap<CreateTaskRequest, CreateTaskCommand>();
            CreateMap<UpdateTaskRequest, UpdateTaskCommand>();
            CreateMap<CreateProjectRequest, CreateProjectCommand>();
            CreateMap<CreateUserRequest, CreateUserCommand>();
        }

        private void CreateQueryMappings()
        {

        }
    }
}
