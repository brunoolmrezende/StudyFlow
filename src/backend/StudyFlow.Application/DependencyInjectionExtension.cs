using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using StudyFlow.Application.Services.AutoMapper;
using StudyFlow.Application.UseCases.User.GetProfile;
using StudyFlow.Application.UseCases.User.Login.DoLogin;
using StudyFlow.Application.UseCases.User.Register;
using StudyFlow.Application.UseCases.User.Update;

namespace StudyFlow.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddUseCases(services);
            AddAutoMapper(services);
        }

        private static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddScoped(options => new MapperConfiguration(autoMapperOptions =>
            {
                autoMapperOptions.AddProfile(new AutoMapping());
            }).CreateMapper());
        }
    }
}
