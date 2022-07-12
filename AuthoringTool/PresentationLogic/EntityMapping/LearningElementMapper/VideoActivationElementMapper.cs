using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningElement.ActivationElement;

namespace AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;

public class VideoActivationElementMapper : IVideoActivationElementMapper
{
    private readonly ILearningContentMapper _contentMapper;
    private ILogger<VideoActivationElementMapper> Logger { get; }

    public VideoActivationElementMapper(ILogger<VideoActivationElementMapper> logger, ILearningContentMapper contentMapper)
    {
        Logger = logger;
        _contentMapper = contentMapper;
    }
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel)
    {
        return new Entities.VideoActivationElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        return new VideoActivationElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}