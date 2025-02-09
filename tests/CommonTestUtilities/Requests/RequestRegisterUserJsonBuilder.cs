﻿using Bogus;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterUserJsonBuilder
    {
        public static RequestRegisterUserJson Build(int passwordLength = 12)
        {
            return new Faker<RequestRegisterUserJson>()
                .RuleFor(user => user.Name, faker => faker.Person.FirstName)
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
                .RuleFor(user => user.Password, faker => faker.Internet.Password(length: passwordLength));
        }
    }
}