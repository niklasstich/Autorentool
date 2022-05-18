using System;
using System.Collections;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.View.Toolbox;
using Bunit;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.Toolbox;

[TestFixture]
public class ToolboxRenderFragmentFactoryUt
{
#pragma warning disable CS8618
    private TestContext _testContext;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
    }
    
    [Test]
    [TestCaseSource(typeof(ToolboxRenderFragmentFactoryTestCases))]
    public void ToolboxRenderFragmentFactory_GetRenderFragment_ReturnsCorrectFragment(IDisplayableLearningObject obj, string expectedMarkup)
    {
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory();

        var renderFragment = systemUnderTest.GetRenderFragment(obj);
        var rendered = _testContext.Render(renderFragment);
        
        //see https://bunit.dev/docs/verification/verify-markup.html
        rendered.MarkupMatches(expectedMarkup);
    }

    [Test]
    public void ToolboxRenderFragmentFactory_GetRenderFragment_ThrowsExceptionOnInvalidObject()
    {
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            systemUnderTest.GetRenderFragment(Substitute.For<IDisplayableLearningObject>()));
    }

    private IAbstractToolboxRenderFragmentFactory GetTestableToolboxRenderFragmentFactory()
    {
        return new ToolboxRenderFragmentFactory();
    }

    private class ToolboxRenderFragmentFactoryTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
                new LearningWorldViewModel("myname", "fa", "fa", "fa", "fa", "fa"),
                @"<div class=""col-3 element text-center text-wrap learning-world"">
myname
<br/>
    <i class=""bi bi-globe""></i>
</div>"
            };
            
            yield return new object[]
            {
                new LearningSpaceViewModel("a name", "sn", "authors", "a description", "goals"),
                @"<div class=""col-3 element text-center text-wrap learning-space"">
a name
<br/>
    <p>a description</p>
</div>"
            };

            yield return new object[]
            {
                new LearningElementViewModel("another name", "an", null, "bubu", "baba", null, "author", "description",
                    "goal"),
                @"<div class=""col-3 element text-center text-wrap learning-element"">
another name
<br/>
    <p>I am an element</p>
</div>"
            };
        }
    }
}