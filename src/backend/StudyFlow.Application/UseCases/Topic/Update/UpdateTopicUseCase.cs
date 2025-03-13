using AutoMapper;
using Sqids;
using StudyFlow.Communication.Requests;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Topic.Update
{
    public class UpdateTopicUseCase : IUpdateTopicUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly ITopicReadOnlyRepository _readOnlyRepository;
        private readonly ISubjectReadOnlyRepository _subjectReadOnlyRepository;
        private readonly ITopicUpdateOnlyRepository _updateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SqidsEncoder<long> _idEncoder;

        public UpdateTopicUseCase(
            ILoggedUser loggedUser,
            ITopicReadOnlyRepository readOnlyRepository,
            ISubjectReadOnlyRepository subjectReadOnlyRepository,
            ITopicUpdateOnlyRepository updateOnlyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            SqidsEncoder<long> idEncoder
            )
        {
            _loggedUser = loggedUser;
            _readOnlyRepository = readOnlyRepository;
            _subjectReadOnlyRepository = subjectReadOnlyRepository;
            _updateOnlyRepository = updateOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idEncoder = idEncoder;
        }

        public async Task Execute(long id, RequestUpdateTopicJson request)
        {
            var loggedUser = await _loggedUser.GetLoggedUser();

            await Validate(request, loggedUser, id);

            var topic = await _updateOnlyRepository.GetTopicById(id, loggedUser);

            if (topic is null)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            _mapper.Map(request, topic);

            _updateOnlyRepository.Update(topic);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateTopicJson request, Domain.Entities.User loggedUser, long id)
        {
            var validator = new UpdateTopicValidator();

            var result = validator.Validate(request);

            var topicAlreadyCreated = await _readOnlyRepository.IsTopicAlreadyCreated(request.Name, loggedUser, id);

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
