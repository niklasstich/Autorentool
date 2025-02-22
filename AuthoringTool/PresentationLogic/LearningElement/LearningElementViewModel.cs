using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public class LearningElementViewModel : ISerializableViewModel, IDisplayableLearningObject, ILearningElementViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningElementViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning element.</param>
    /// <param name="shortname">The short name (abbreviation) of the learning element (Maybe not relevant).</param>
    /// <param name="parent">Decides whether the learning element belongs to a learning world or a learning space.</param>
    /// <param name="learningContent">Represents the loaded content of the learning element.</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning element.</param>
    /// <param name="description">A description of the learning element and its contents.</param>
    /// <param name="goals">A description of the goals this learning element is supposed to achieve.</param>
    /// <param name="difficulty">Difficulty of the learning element.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="positionX">x-position of the learning element in the workspace.</param>
    /// <param name="positionY">y-position of the learning element in the workspace.</param>

    public LearningElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload = 0,  double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Parent = parent;
        LearningContent = learningContent;
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        PositionX = positionX;
        PositionY = positionY;
    }
    
    public const string fileEnding = "aef";
    public string FileEnding => fileEnding;
    public string Name { get; set; }
    public string Shortname { get; set; }
    public ILearningElementViewModelParent? Parent { get; set; }
    public LearningContentViewModel LearningContent { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public int Workload { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}