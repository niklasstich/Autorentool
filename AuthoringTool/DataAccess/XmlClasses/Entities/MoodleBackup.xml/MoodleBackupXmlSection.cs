﻿using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="section")]
public partial class MoodleBackupXmlSection : IMoodleBackupXmlSection
{

    public MoodleBackupXmlSection()
    {
        SectionId = "";
        Title = "";
        Directory = "";
    }
    
    
    [XmlElement(ElementName = "sectionid")]
    public string SectionId { get; set; }
		
    [XmlElement(ElementName="title")]
    public string Title { get; set; }

    [XmlElement(ElementName = "directory")]
    public string Directory { get; set; }
}