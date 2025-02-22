﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="activity_gradebook")]
public class ActivitiesGradesXmlActivityGradebook : IActivitiesGradesXmlActivityGradebook {

    public ActivitiesGradesXmlActivityGradebook()
    {
        GradeItems = new ActivitiesGradesXmlGradeItems();
        GradeLetters = "";
    }
    
  
    public void Serialize(string activityName, string moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this,Path.Join("activities", activityName + "_" + moduleId, "grades.xml"));
    }

    [XmlElement(ElementName="grade_items")]
    public ActivitiesGradesXmlGradeItems GradeItems { get; set; }
    
    [XmlElement(ElementName="grade_letters")]
    public string GradeLetters { get; set; }
}