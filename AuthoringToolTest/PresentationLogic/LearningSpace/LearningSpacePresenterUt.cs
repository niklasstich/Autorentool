using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpacePresenterUt
{
    [Test]
    public void LearningSpacePresenter_CreateNewLearningSpace_CreatesCorrectViewModel()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var name = "a";
        var shortname = "b";
        var authors = "d";
        var description = "e";
        var goals = "f";

        var space = systemUnderTest.CreateNewLearningSpace(name, shortname, authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo(name));
            Assert.That(space.Shortname, Is.EqualTo(shortname));
            Assert.That(space.Authors, Is.EqualTo(authors));
            Assert.That(space.Description, Is.EqualTo(description));
            Assert.That(space.Goals, Is.EqualTo(goals));
        });
    }

    [Test]
    public void LearningSpacePresenter_EditLearningSpace_EditsViewModelCorrectly()
    {
        var systemUnderTest = CreatePresenterForTesting();
        var space = new LearningSpaceViewModel("a", "b", "c", "d", "e");

        var name = "new space";
        var shortname = "ns";
        var authors = "marvin";
        var description = "space with learning stuff";
        var goals = "learn";

        space = systemUnderTest.EditLearningSpace(space, name, shortname, authors, description, goals);
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo(name));
            Assert.That(space.Shortname, Is.EqualTo(shortname));
            Assert.That(space.Authors, Is.EqualTo(authors));
            Assert.That(space.Description, Is.EqualTo(description));
            Assert.That(space.Goals, Is.EqualTo(goals));
        });
    }
    
    #region LearningElement

    #region CreateNewLearningElement

    [Test]
    public void LearningSpacePresenter_CreateNewLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("foo", "bar",
            null, "Video", "bar",null, "foo", "foo", "bar"));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningElement("foo",
            "bar", null, "foo", "bar", "foo", "bar",
            "foo"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void LearningSpacePresenter_CreateNewLearningElement_CallsLearningElementPresenter()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("foo", "bar", null,
            "foo", "bar", null,"foo", "bar", "foo"));
        var parent = new LearningWorldViewModel("foo", "boo", "bla", "blub", "bibi", "bubu");

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.CreateNewLearningElement("name", "sn", parent, "type",
            "cont", "aut", "desc", "goal");

        learningElementPresenter.Received()
            .CreateNewLearningElement("name", "sn", parent, "type", "cont", "aut", "desc", "goal");
    }

    [Test]
    public void LearningSpacePresenter_CreateNewLearningElement_AddsLearningElementToSpaceViewModel()
    {
        var learningElementPresenter = new LearningElementPresenter();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);

        Assert.That(space.LearningElements, Is.Empty);

        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.CreateNewLearningElement("foo", "bar", space,
            "foo", "bar", "foo", "bar", "foo");

        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        var element = space.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("foo"));
            Assert.That(element.Shortname, Is.EqualTo("bar"));
            Assert.That(element.Parent, Is.EqualTo(space));
            Assert.That(element.ElementType, Is.EqualTo("foo"));
            Assert.That(element.ContentType, Is.EqualTo("bar"));
            Assert.That(element.Authors, Is.EqualTo("foo"));
            Assert.That(element.Description, Is.EqualTo("bar"));
            Assert.That(element.Goals, Is.EqualTo("foo"));
        });
    }

    #endregion

    #region OnCreateElementDialogClose

    [Test]
    public void LearningWorldPresenter_OnCreateElementDialogClose_ThrowsWhenDialogDataAreNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, null);

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OnCreateElementDialogClose(returnValueTuple));
        Assert.That(ex!.Message, Is.EqualTo("dialog data unexpectedly null after Ok return value"));
    }
    
    [Test]
    public void LearningWorldPresenter_OnCreateElementDialogClose_WithLearningWorld_CallsLearningElementPresenter()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("ba", "ba",
            null, "ba", "ba", null, "ba", "ba", "ba"));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "Learning space";
        dictionary["Assignment"] = "foo";
        dictionary["ElementType"] = "c";
        dictionary["ContentType"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OnCreateElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().CreateNewLearningElement("a", "b", space, "c", "d", "e", "f", "g");
    }

    #endregion

    #region OnEditElementDialogClose

    [Test]
    public void LearningSpacePresenter_OnEditElementDialogClose_CallsLearningElementPresenter()
    {
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<ILearningElementViewModelParent>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("ba", "ba",
            null, "ba", "ba",null, "ba", "ba", "ba"));
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("foo", "bar", null, "bar", "foo",
            null,"bar", "foo", "bar");
        space.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "Learning world";
        dictionary["Assignment"] = "foo";
        dictionary["ElementType"] = "c";
        dictionary["ContentType"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter: learningElementPresenter);
        systemUnderTest.SetLearningSpace(space);
        space.SelectedLearningObject = element;

        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b", space, "c", "d", "e", "f", "g");
    }

    #endregion

    #region DeleteSelectedLearningObject

    [Test]
    public void LearningSpacePresenter_DeleteSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.SelectedLearningObject, Is.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void LearningSpacePresenter_DeleteSelectedLearningObject_WithElement_CallsElementPresenter()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("f", "f", space, "f", "f",null, "f", "f", "f");
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;

        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter:mockElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteSelectedLearningObject();

        mockElementPresenter.Received().RemoveLearningElementFromParentAssignment(element);
    }

    [Test]
    public void LearningSpacePresenter_DeleteSelectedLearningObject_WithUnknownObject_ThrowsNotImplemented()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        space.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    [Test]
    public void LearningSpacePresenter_DeleteSelectedLearningObject_WithElement_MutatesSelectionInViewModel()
    {
        var mockElementPresenter = Substitute.For<ILearningElementPresenter>();
        mockElementPresenter.When(x => x.RemoveLearningElementFromParentAssignment(Arg.Any<LearningElementViewModel>())).Do(x =>
        {
            var ele = x.Arg<LearningElementViewModel>();
            (((LearningSpaceViewModel) ele.Parent!)).LearningElements.Remove(ele);
        });
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element1 = new LearningElementViewModel("f", "f", space, "f", "f", null,"f", "f", "f");
        var element2 = new LearningElementViewModel("e", "e", space, "f", "f", null,"f", "f", "f");
        space.LearningElements.Add(element1);
        space.LearningElements.Add(element2);
        space.SelectedLearningObject = element1;

        Assert.That(space.SelectedLearningObject, Is.EqualTo(element1));

        var systemUnderTest = CreatePresenterForTesting(learningElementPresenter:mockElementPresenter);
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(space.SelectedLearningObject, Is.EqualTo(element2));
        
        systemUnderTest.DeleteSelectedLearningObject();
        
        Assert.That(space.SelectedLearningObject, Is.Null);
        
    }

    #endregion

    #region OpenEditSelectedLearningObjectDialog

    [Test]
    public void LearningSpacePresenter_OpenEditSelectedLearningObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void LearningSpacePresenter_OpenEditSelectedLearningObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceVm,Is.Not.Null);
            Assert.That(systemUnderTest.LearningSpaceVm?.SelectedLearningObject, Is.EqualTo(null));
            Assert.That(systemUnderTest.LearningSpaceVm?.LearningElements, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void LearningSpacePresenter_OpenEditSelectedLearningObjectDialog_WithElement_CallsMethod()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("n", "sn", space, "t","c",null,"a", "d", "g");
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;
        space.EditDialogInitialValues = new Dictionary<string, string>();
        
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);
        
        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.EditLearningElementDialogOpen, Is.True);
            Assert.That(space.EditDialogInitialValues["Name"], Is.EqualTo(element.Name));
            Assert.That(space.EditDialogInitialValues["Shortname"], Is.EqualTo(element.Shortname));
            Assert.That(space.EditDialogInitialValues["Parent"], Is.EqualTo("Learning space"));
            Assert.That(space.EditDialogInitialValues["Assignment"], Is.EqualTo(element.Parent?.Name));
            Assert.That(space.EditDialogInitialValues["ElementType"], Is.EqualTo(element.ElementType));
            Assert.That(space.EditDialogInitialValues["ContentType"], Is.EqualTo(element.ContentType));
            Assert.That(space.EditDialogInitialValues["Authors"], Is.EqualTo(element.Authors));
            Assert.That(space.EditDialogInitialValues["Description"], Is.EqualTo(element.Description));
            Assert.That(space.EditDialogInitialValues["Goals"], Is.EqualTo(element.Goals));
        });
    }

    [Test]
    public void
        LearningWorldPresenter_OpenEditSelectedLearningObjectDialog_WithUnknownObject_ThrowsNotImplemented()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        space.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region SaveSelectedLearningObject

    [Test]
    public void LearningSpacePresenter_SaveSelectedLearningObjectAsync_ThrowsWhenSelectedWorldNull()
    {
        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(null);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningSpace is null"));
    }

    [Test]
    public void LearningSpacePresenter_SaveSelectedLearningObjectAsync_DoesNotThrowWhenSelectedObjectNull()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObject is null"));
    }

    [Test]
    public void LearningSpacePresenter_SaveSelectedLearningObject_WithElement_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var element = new LearningElementViewModel("f", "f", space, "f", "f", null,"f",
            "f", "f");
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting(presentationLogic);
        systemUnderTest.SetLearningSpace(space);
        systemUnderTest.SaveSelectedLearningObjectAsync();

        presentationLogic.Received().SaveLearningElementAsync(element);
    }

    [Test]
    public void LearningSpacePresenter_SaveSelectedLearningObject_WithUnknownObjectAsync_ThrowsNotImplemented()
    {
        var space = new LearningSpaceViewModel("foo", "foo", "foo", "foo", "foo");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        space.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting();
        systemUnderTest.SetLearningSpace(space);

        var ex = Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.SaveSelectedLearningObjectAsync());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #endregion


    private LearningSpacePresenter CreatePresenterForTesting(IPresentationLogic? presentationLogic = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningSpacePresenter(presentationLogic, learningElementPresenter, logger);
    }
}