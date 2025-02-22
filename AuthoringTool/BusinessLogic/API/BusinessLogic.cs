﻿using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;
using ElectronWrapper;

namespace AuthoringTool.BusinessLogic.API;

internal class BusinessLogic : IBusinessLogic
{

    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess,
        IHybridSupportWrapper hybridSupport)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        HybridSupport = hybridSupport;
    }
    
    
    
    public IDataAccess DataAccess { get;  }
    public IHybridSupportWrapper HybridSupport { get; }

    public bool RunningElectron => HybridSupport.IsElectronActive;

    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        DataAccess.ConstructBackup(learningWorld, filepath);
    }

    public void SaveLearningWorld(LearningWorld learningWorld, string filepath)
    {
        DataAccess.SaveLearningWorldToFile(learningWorld, filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        return DataAccess.LoadLearningWorldFromFile(filepath);
    }
    
    public void SaveLearningSpace(LearningSpace learningSpace, string filepath)
    {
        DataAccess.SaveLearningSpaceToFile(learningSpace, filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        return DataAccess.LoadLearningSpaceFromFile(filepath);
    }
    
    public void SaveLearningElement(LearningElement learningElement, string filepath)
    {
        DataAccess.SaveLearningElementToFile(learningElement, filepath);
    }

    public LearningElement LoadLearningElement(string filepath)
    {
        return DataAccess.LoadLearningElementFromFile(filepath);
    }

    public LearningContent LoadLearningContent(string filepath)
    {
        return DataAccess.LoadLearningContentFromFile(filepath);
    }

    public LearningContent LoadLearningContentFromStream(string name, Stream stream)
    {
        return DataAccess.LoadLearningContentFromStream(name, stream);
    }

    public LearningWorld LoadLearningWorldFromStream(Stream stream)
    {
        return DataAccess.LoadLearningWorldFromStream(stream);
    }

    public LearningSpace LoadLearningSpaceFromStream(Stream stream)
    {
        return DataAccess.LoadLearningSpaceFromStream(stream);
    }

    public LearningElement LoadLearningElementFromStream(Stream stream)
    {
        return DataAccess.LoadLearningElementFromStream(stream);
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        return DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
    }
    public IAuthoringToolConfiguration Configuration { get; }
  
}