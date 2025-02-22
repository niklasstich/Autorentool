﻿using System.Xml.Serialization;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

[XmlInclude(typeof(ImageTransferElement))]
[XmlInclude(typeof(VideoTransferElement))]
[XmlInclude(typeof(PdfTransferElement))]
[XmlInclude(typeof(VideoActivationElement))]
[XmlInclude(typeof(H5PActivationElement))]
[XmlInclude(typeof(H5PInteractionElement))]
[XmlInclude(typeof(H5PTestElement))]
[Serializable]
public class LearningElement : ILearningElement
{
    internal LearningElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload = 0,
        double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Content = content ?? new LearningContent();
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        PositionX = positionX;
        PositionY = positionY;
        ParentName = parentName;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    internal LearningElement()
    {
        Name = "";
        Shortname = "";
        Content = new LearningContent();
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.Medium;
        Workload = 0;
        PositionX = 0;
        PositionY = 0;
        ParentName = null;
    }


    public string Name { get; set; }
    public string Shortname { get; set; }
    public LearningContent Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string? ParentName { get; set; }
}

