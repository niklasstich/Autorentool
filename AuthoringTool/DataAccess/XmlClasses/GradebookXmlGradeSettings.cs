﻿using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="grade_settings")]
public partial class GradebookXmlGradeSettings
{
    [XmlElement(ElementName = "grade_setting")]
    public GradebookXmlGradeSetting Grade_setting;
}
