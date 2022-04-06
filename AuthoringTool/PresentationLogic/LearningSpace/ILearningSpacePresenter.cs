﻿namespace AuthoringTool.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenter
{
    LearningSpaceViewModel CreateNewLearningSpace(string name, string shortname, string authors, string description,
        string goals);

    LearningSpaceViewModel EditLearningSpace(LearningSpaceViewModel space, string name, string shortname,
        string authors, string description, string goals);
}