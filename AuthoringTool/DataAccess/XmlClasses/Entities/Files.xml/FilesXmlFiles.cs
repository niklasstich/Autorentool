﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;


namespace AuthoringTool.DataAccess.XmlClasses.Entities;


[XmlRoot(ElementName = "files")]
public class FilesXmlFiles : IFilesXmlFiles
{

    public FilesXmlFiles()
    {
        File = new List<FilesXmlFile>();
    }
    
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "files.xml");
    }
    
    [XmlElement(ElementName="file")]
    public List<FilesXmlFile> File { get; set; }
    
}
