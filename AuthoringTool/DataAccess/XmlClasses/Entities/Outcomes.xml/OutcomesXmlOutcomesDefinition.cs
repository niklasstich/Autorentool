﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="outcomes_definition")]
public class OutcomesXmlOutcomesDefinition : IOutcomesXmlOutcomesDefinition
{

    public OutcomesXmlOutcomesDefinition()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "outcomes.xml");
    }
}