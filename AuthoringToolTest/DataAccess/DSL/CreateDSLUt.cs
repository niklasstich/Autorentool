﻿using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class CreateDslUt
{

    [Test]
    public void CreateDSL_WriteLearningWorld_DSLDocumentWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        var content1 = new LearningContent("a", ".h5p", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h", LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz", LearningElementDifficultyEnum.Easy, 444, double.MaxValue);
        var ele3 = new LearningElement("a", "b", "e",content1, "pupup", "g","h", LearningElementDifficultyEnum.Easy, 17, 23);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpace> { space1, space2 };

        var learningWorld = new LearningWorld(name, shortname, authors, language, description, goals,
            learningElements, learningSpaces);

        var createDsl = GetCreateDslForTest(mockFileSystem);
        
        var allLearningElements = new List<LearningElement> { ele3, ele1, ele2 };
       
        //Act
        createDsl.WriteLearningWorld(learningWorld);
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "DSL_Document.json");
        Assert.That(createDsl.learningWorldJson, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createDsl.learningWorldJson!.identifier, Is.Not.Null);
            Assert.That(createDsl.learningWorldJson.identifier!.value, Is.EqualTo(learningWorld.Name));
            Assert.That(createDsl.learningWorldJson.identifier.type, Is.EqualTo("name"));
            Assert.That(createDsl.listLearningElements, Is.EqualTo(allLearningElements));
            Assert.That(createDsl.listLearningSpaces, Is.EqualTo(learningSpaces));
            Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
        });
    }

    private CreateDSL GetCreateDslForTest(IFileSystem? fileSystem = null)
    {
        fileSystem ??= new MockFileSystem();

        return new CreateDSL(fileSystem);
    }
}