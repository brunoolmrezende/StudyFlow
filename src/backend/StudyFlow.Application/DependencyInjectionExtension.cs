using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sqids;
using StudyFlow.Application.Services.AutoMapper;
using StudyFlow.Application.UseCases.Subject.Create;
using StudyFlow.Application.UseCases.User.ChangePassword;
using StudyFlow.Application.UseCases.User.GetProfile;
using StudyFlow.Application.UseCases.User.Login.DoLogin;
using StudyFlow.Application.UseCases.User.Register;
using StudyFlow.Application.UseCases.User.Update;
using StudyFlow.Application.UseCases.Subject.GetAll;
using StudyFlow.Application.UseCases.Subject.GetById;
using StudyFlow.Application.UseCases.Subject.Update;
using StudyFlow.Application.UseCases.Topic.Create;
using StudyFlow.Application.UseCases.Topic.GetAll;
using StudyFlow.Application.UseCases.Topic.GetById;
using StudyFlow.Application.UseCases.Topic.Update;
using StudyFlow.Application.UseCases.Review.Create;
using StudyFlow.Application.UseCases.Review.Update;
using StudyFlow.Application.UseCases.Review.Deactivate;

namespace StudyFlow.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddUseCases(services);
            AddIdEncoder(services, configuration);
            AddAutoMapper(services, configuration);
        }

        private static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

            services.AddScoped<ICreateSubjectUseCase, CreateSubjectUseCase>();
            services.AddScoped<IGetAllSubjectsUseCase, GetAllSubjectsUseCase>();
            services.AddScoped<IGetSubjectByIdUseCase, GetSubjectByIdUseCase>();
            services.AddScoped<IUpdateSubjectUseCase, UpdateSubjectUseCase>();

            services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
            services.AddScoped<IGetAllTopicsUseCase, GetAllTopicsUseCase>();
            services.AddScoped<IGetTopicByIdUseCase, GetTopicByIdUseCase>();
            services.AddScoped<IUpdateTopicUseCase, UpdateTopicUseCase>();

            services.AddScoped<ICreateReviewUseCase, CreateReviewUseCase>();
            services.AddScoped<IUpdateReviewUseCase, UpdateReviewUseCase>();
            services.AddScoped<IDeactivateReviewUseCase, DeactivateReviewUseCase>();
        }

        private static void AddAutoMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(options => new MapperConfiguration(autoMapperOptions =>
            {
                var sqids = options.GetService<SqidsEncoder<long>>()!;

                autoMapperOptions.AddProfile(new AutoMapping(sqids));
            }).CreateMapper());
        }

        private static void AddIdEncoder(this IServiceCollection services, IConfiguration configuration)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
            });

            services.AddSingleton(sqids);
        }
    }
}
