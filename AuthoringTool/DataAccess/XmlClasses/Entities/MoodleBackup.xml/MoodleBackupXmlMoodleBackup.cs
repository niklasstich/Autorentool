﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="moodle_backup")]
public partial class MoodleBackupXmlMoodleBackup : IMoodleBackupXmlMoodleBackup {

    public MoodleBackupXmlMoodleBackup()
    {
        Information = new MoodleBackupXmlInformation();
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "moodle_backup.xml"); 
    }
    
    [XmlElement(ElementName="information")]
    public MoodleBackupXmlInformation Information { get; set; }
}