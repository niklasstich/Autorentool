﻿using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesGradesXmlUt
{
    [Test]
    public void ActivitiesGradesXmlGradeItem_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new ActivitiesGradesXmlGradeItem();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CategoryId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemName, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemType, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemModule, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemInstance, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ItemNumber, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ItemInfo, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.IdNumber, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Calculation, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.GradeType, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grademax, Is.EqualTo("100.00000"));
            Assert.That(systemUnderTest.Grademin, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.ScaleId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.OutcomeId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Gradepass, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Multfactor, Is.EqualTo("1.00000"));
            Assert.That(systemUnderTest.Plusfactor, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Aggregationcoef, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Aggregationcoef2, Is.EqualTo("1.00000"));
            Assert.That(systemUnderTest.Weightoverride, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Sortorder, Is.EqualTo("2"));
            Assert.That(systemUnderTest.Display, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Decimals, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Hidden, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Locked, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Locktime, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Needsupdate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.GradeGrades, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }

    [Test]
    public void ActivitiesGradesXmlGradeItems_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        var systemUnderTest = new ActivitiesGradesXmlGradeItems();
        
        //Act
        systemUnderTest.GradeItem= gradeitem;

        //Assert
        Assert.That(systemUnderTest.GradeItem, Is.EqualTo(gradeitem));

    }

    [Test]
    public void ActivitiesGradesXmlActivityGradebook_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        var gradeitems = Substitute.For<ActivitiesGradesXmlGradeItems>();
        gradeitems.GradeItem = gradeitem;
        var systemUnderTest = new ActivitiesGradesXmlActivityGradebook();

        //Act
        systemUnderTest.GradeItems = gradeitems;

        //Assert
        Assert.That(systemUnderTest.GradeItems, Is.EqualTo(gradeitems));
        Assert.That(systemUnderTest.GradeLetters, Is.EqualTo(""));
    }

    [Test]
    public void ActivitiesGradesXmlActivityGradebook_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2"));
        
        var gradeitem = new ActivitiesGradesXmlGradeItem();
        var gradeitems = new ActivitiesGradesXmlGradeItems();
        gradeitems.GradeItem = gradeitem;
        
        var gradeActivityGradebook = new ActivitiesGradesXmlActivityGradebook();
        gradeActivityGradebook.GradeItems = gradeitems;
        
        //Act 
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        gradeActivityGradebook.Serialize("h5pactivity", "2");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2", "grades.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}