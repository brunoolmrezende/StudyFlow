﻿using AutoMapper;
using StudyFlow.Application.Services.AutoMapper;

namespace CommonTestUtilities.AutoMapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            return new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper();
        }
    }
}
