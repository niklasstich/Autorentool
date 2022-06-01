﻿using System;
using System.IO;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.API;

[TestFixture]
public class DataAccessUt
{
    [Test]
    public void DataAccess_Standard_AllPropertiesInitialized()
    {
        //Arrange 
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockBackupFileConstructor = Substitute.For<IBackupFileGenerator>();
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();

        //Act 
        var systemUnderTest = CreateTestableDataAccess(mockConfiguration, mockBackupFileConstructor,
            mockFileSaveHandlerWorld);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BackupFile, Is.EqualTo(mockBackupFileConstructor));
            Assert.That(systemUnderTest.XmlHandlerWorld, Is.EqualTo(mockFileSaveHandlerWorld));
        });
    }

    [Test]
    public void DataAccess_ConstructBackup_CallsBackupFileGenerator()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockCreateDsl = Substitute.For<ICreateDSL>();
        
        var mockBackupFile = Substitute.For<IBackupFileGenerator>();
        var systemUnderTest = new AuthoringTool.DataAccess.API.DataAccess(null, mockBackupFile, 
            null, null, null, null, mockCreateDsl, mockReadDsl);
        var filepath = "this/path";
        var mockLearningWorld = Substitute.For<ILearningWorld>();


        //Act
        systemUnderTest.ConstructBackup(mockLearningWorld as LearningWorld, filepath);

        //Assert
        mockCreateDsl.Received().WriteLearningWorld(mockLearningWorld as LearningWorld);
        mockReadDsl.Received().ReadLearningWorld();
        mockBackupFile.Received().CreateBackupFolders();
        mockBackupFile.Received().WriteXmlFiles(mockReadDsl as ReadDSL);
        mockBackupFile.Received().WriteBackupFile(filepath);

    }

    [Test]
    public void DataAccess_SaveLearningWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        var learningWorld = new LearningWorld("f", "f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningWorldToFile(
            learningWorld,
            "C:/nonsense");

        mockFileSaveHandlerWorld.Received().SaveToDisk(learningWorld, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorldFromFile("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningWorldFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorldFromStream(stream);

        mockFileSaveHandlerWorld.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveLearningSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpace>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        var learningSpace = new LearningSpace("f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningSpaceToFile(
            learningSpace,
            "C:/nonsense");

        mockFileSaveHandlerSpace.Received().SaveToDisk(learningSpace, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningSpaceFromFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpace>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadLearningSpaceFromFile("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningSpaceFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpace>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningSpaceFromStream(stream);

        mockFileSaveHandlerSpace.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveLearningElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var learningContent = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("f","f", "f", learningContent, "f", "f", "f");
        systemUnderTest.SaveLearningElementToFile(
            learningElement,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(learningElement, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadLearningElementFromFile("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningElementFromStream_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningElementFromStream(stream);

        mockFileSaveHandlerElement.Received().LoadFromStream(stream);
    }

    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(
        IAuthoringToolConfiguration? configuration = null,
        IBackupFileGenerator? backupFileConstructor = null,
        IXmlFileHandler<LearningWorld>? fileSaveHandlerWorld = null,
        IXmlFileHandler<LearningSpace>? fileSaveHandlerSpace = null,
        IXmlFileHandler<LearningElement>? fileSaveHandlerElement = null,
        IContentFileHandler? contentHandler = null,
        ICreateDSL? createDsl = null,
        IReadDSL? readDsl = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        backupFileConstructor ??= Substitute.For<IBackupFileGenerator>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<LearningWorld>>();
        fileSaveHandlerSpace ??= Substitute.For<IXmlFileHandler<LearningSpace>>();
        fileSaveHandlerElement ??= Substitute.For<IXmlFileHandler<LearningElement>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        createDsl ??= Substitute.For<ICreateDSL>();
        readDsl ??= Substitute.For<IReadDSL>();
        return new AuthoringTool.DataAccess.API.DataAccess(configuration, backupFileConstructor, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement, contentHandler, createDsl, readDsl);
    }
}