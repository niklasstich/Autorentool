﻿namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlActivity : IXmlSerializablePath
{

    ActivitiesLessonXmlLesson Lesson { get; set; }
        
    string Id { get; set; }
        
    string ModuleId { get; set; }
        
    string ModuleName { get; set; }
    
    string ContextId { get; set; }
    

}