using StudyFlow.Application.UseCases.Subject.Create;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Exceptions.ExceptionBase;
using StudyFlow.Exceptions;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Domain.Repositories.Topic;
using AutoMapper;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Subject;
using Sqids;

namespace StudyFlow.Application.UseCases.Topic
{   
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITopicReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly ITopicWriteOnlyRepository _writeOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectReadOnlyRepository _subjectReadOnlyRepository;
        private readonly SqidsEncoder<long> _idEncoder;

        public CreateTopicUseCase(
            ILoggedUser loggedUser,
            ITopicReadOnlyRepository readOnlyRepository,
            ITopicWriteOnlyRepository writeOnlyRepository,
            ISubjectReadOnlyRepository subjectReadOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            SqidsEncoder<long> idEncoder)
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _subjectReadOnlyRepository = subjectReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idEncoder = idEncoder;
        }

        public async Task<ResponseCreatedTopicJson> Execute(RequestCreateTopicJson request)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            await Validate(request, loggedUser);

            var topic = _mapper.Map<Domain.Entities.Topic>(request);

            topic.UserId = loggedUser.Id;

            await _writeOnlyRepository.Add(topic);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseCreatedTopicJson>(topic);
        }

        private async Task Validate(RequestCreateTopicJson request, Domain.Entities.User loggedUser)
        {
            var validator = new CreateTopicValidator();

            var result = validator.Validate(request);

            var topicAlreadyCreated = await _readOnlyRepository.IsTopicCreatedAndActive(request.Name, loggedUser);

            if (topicAlreadyCreated)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Topic already exists.", ResourceMessagesException.TOPIC_ALREADY_CREATED));
            }

            var subjectId = _idEncoder.Decode(request.SubjectId).FirstOrDefault();

            var checkSubjectExists = await _subjectReadOnlyRepository.GetSubjectById(subjectId, loggedUser);

            if (checkSubjectExists is null)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Subject not found.", ResourceMessagesException.SUBJECT_NOT_FOUND));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
