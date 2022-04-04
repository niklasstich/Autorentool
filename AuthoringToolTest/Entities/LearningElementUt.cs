using AuthoringTool.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.Entities;

[TestFixture]
public class LearningElementUt
{
    [Test]
    public void LearningElement_Constructor_InitializesAllProperties()
    {
        var Name = "asdf";
        var Shortname = "jkl;";
        var Type = "h5p";
        var Content = "foo = bar";
        var Authors = "ben and jerry";
        var Description = "very cool element";
        var Goals = "learn very many things";
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new LearningElement(Name, Shortname, Type, Content, Authors, Description, Goals,
            PositionX, PositionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Type, Is.EqualTo(Type));
            Assert.That(systemUnderTest.Content, Is.EqualTo(Content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
    }
}