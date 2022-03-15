using System;
using System.Linq;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspacePresenterUt
{
    [Test]
    public void AuthoringToolWorkspacePresenter_StandardConstructor_AllPropertiesInitialized()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        Assert.AreEqual(false, systemUnderTest.CreateLearningSpaceDialogueOpen);
        Assert.AreEqual(false, systemUnderTest.CreateLearningWorldDialogOpen);
        Assert.AreEqual(false, systemUnderTest.EditLearningSpaceDialogOpen);
        Assert.AreEqual(false, systemUnderTest.EditLearningWorldDialogOpen);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldCreated_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var callbackCalled = false;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);

        systemUnderTest.OnLearningWorldCreate += (_, world) =>
        {
            callbackCalled = true;
            Assert.IsNotNull(world);
            Assert.AreEqual("foo", world!.Name);
            Assert.AreEqual("f", world.Shortname);
            Assert.AreEqual("bar", world.Authors);
            Assert.AreEqual("de", world.Language);
            Assert.AreEqual("A test", world.Description);
            Assert.AreEqual("testing", world.Goals);
        };
        systemUnderTest.CreateNewLearningWorld("foo", "f", "bar", "de", "A test", "testing");
        Assert.AreEqual(true, callbackCalled);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldCreated_CallsLearningWorldPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo",
                "Foo", "Foo"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        
        worldPresenter.Received()
            .CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldCreated_AddsWorldToWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        Assert.AreEqual(0, workspaceVm.LearningWorlds.Count);
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        Assert.AreEqual(1, workspaceVm.LearningWorlds.Count);
        var learningWorldViewModel = workspaceVm.LearningWorlds.First();
        Assert.AreEqual("Foo", learningWorldViewModel.Name);
        Assert.AreEqual("Foo", learningWorldViewModel.Shortname);
        Assert.AreEqual("Foo", learningWorldViewModel.Authors);
        Assert.AreEqual("Foo", learningWorldViewModel.Language);
        Assert.AreEqual("Foo", learningWorldViewModel.Description);
        Assert.AreEqual("Foo", learningWorldViewModel.Goals);
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldSelected_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var firstCallbackCalled = false;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);

        EventHandler<LearningWorldViewModel?> firstCallback = (_, world) =>
        {
            firstCallbackCalled = true;
            Assert.IsNotNull(world);
            Assert.AreEqual("tetete", world!.Name);
            Assert.AreEqual("f", world.Shortname);
            Assert.AreEqual("bar", world.Authors);
            Assert.AreEqual("de", world.Language);
            Assert.AreEqual("A test", world.Description);
            Assert.AreEqual("testing", world.Goals);
        };
        
        systemUnderTest.OnLearningWorldSelect += firstCallback;
        
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        systemUnderTest.CreateNewLearningWorld("tetete", "f", "bar", "de", "A test",
            "testing");
        systemUnderTest.ChangeSelectedLearningWorld("tetete");
        
        Assert.AreEqual(true, firstCallbackCalled);
        firstCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect -= firstCallback;
        
        var secondCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect += (_, world) =>
        {
            secondCallbackCalled = true;
            Assert.IsNotNull(world);
            Assert.AreEqual("Foo", world!.Name);
            Assert.AreEqual("Foo", world.Shortname);
            Assert.AreEqual("Foo", world.Authors);
            Assert.AreEqual("Foo", world.Language);
            Assert.AreEqual("Foo", world.Description);
            Assert.AreEqual("Foo", world.Goals);
        };
        
        systemUnderTest.ChangeSelectedLearningWorld("Foo");
        Assert.AreEqual(true, secondCallbackCalled);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldSelected_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        Assert.AreEqual(null, workspaceVm.SelectedLearningWorld);
        
        systemUnderTest.ChangeSelectedLearningWorld("tetete");
        Assert.AreEqual(world2, workspaceVm.SelectedLearningWorld);
        
        systemUnderTest.ChangeSelectedLearningWorld("Foo");
        Assert.AreEqual(world1, workspaceVm.SelectedLearningWorld);
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldSelected_ThrowsIfNoLearningWorldWithName() 
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        Assert.IsEmpty(workspaceVm.LearningWorlds);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.ChangeSelectedLearningWorld("foo"));
        Assert.AreEqual("no world with that name in viewmodel", ex.Message);
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldDeleted_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        EventHandler<LearningWorldViewModel?> firstCallback = (_, world) =>
        {
            Assert.Fail("first callback with null as selected world should not have been called");
        };
        var secondCallbackCalled = false;
        EventHandler<LearningWorldViewModel?> secondCallback = (_, world) =>
        {
            secondCallbackCalled = true;
            Assert.AreEqual(world1, world);
        };
        var thirdCallbackCalled = false;
        EventHandler<LearningWorldViewModel?> thirdCallback = (_, world) =>
        {
            thirdCallbackCalled = true;
            Assert.AreEqual(world2, world);
        };
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        systemUnderTest.OnLearningWorldDelete += firstCallback;
         
        systemUnderTest.DeleteSelectedLearningWorld();

        systemUnderTest.OnLearningWorldDelete -= firstCallback;
        systemUnderTest.OnLearningWorldDelete += secondCallback;
        
        systemUnderTest.ChangeSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.AreEqual(true, secondCallbackCalled);
        systemUnderTest.OnLearningWorldDelete -= secondCallback;
        systemUnderTest.OnLearningWorldDelete += thirdCallback;
        
        systemUnderTest.ChangeSelectedLearningWorld(world2.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.AreEqual(true, thirdCallbackCalled);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldDeleted_DeletesWorldFromViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        
        Assert.AreEqual(2, workspaceVm.LearningWorlds.Count);
        Assert.Contains(world1, workspaceVm.LearningWorlds);
        Assert.Contains(world2, workspaceVm.LearningWorlds);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.AreEqual(2, workspaceVm.LearningWorlds.Count);
        Assert.Contains(world1, workspaceVm.LearningWorlds);
        Assert.Contains(world2, workspaceVm.LearningWorlds);
        
        systemUnderTest.ChangeSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.AreEqual(1, workspaceVm.LearningWorlds.Count);
        Assert.Contains(world2, workspaceVm.LearningWorlds);
        
        systemUnderTest.ChangeSelectedLearningWorld(world2.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.AreEqual(0, workspaceVm.LearningWorlds.Count);
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldDeleted_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        systemUnderTest.ChangeSelectedLearningWorld(world2.Name);
        Assert.AreEqual(world2, workspaceVm.SelectedLearningWorld);
        
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.AreEqual(world1, workspaceVm.SelectedLearningWorld);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldEdited_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm.LearningWorlds.Add(world1);
        var callbackCalled = false;
        EventHandler<LearningWorldViewModel?> callback = (_, world) =>
        {
            callbackCalled = true;
            Assert.IsNotNull(world);
            Assert.AreEqual(world1, world);
        };
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        systemUnderTest.OnLearningWorldEdit += callback;
        
        systemUnderTest.ChangeSelectedLearningWorld(world1.Name);
        
        systemUnderTest.EditCurrentLearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        
        Assert.AreEqual(true, callbackCalled);
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldEdited_SelectedWorldChanged()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        
        systemUnderTest.ChangeSelectedLearningWorld(world1.Name);
        
        systemUnderTest.EditCurrentLearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        
        Assert.AreEqual("Name", world1.Name);
        Assert.AreEqual("Shortname", world1.Shortname);
        Assert.AreEqual("Authors", world1.Authors);
        Assert.AreEqual("Language", world1.Language);
        Assert.AreEqual("Description", world1.Description);
        Assert.AreEqual("Goals", world1.Goals);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_LearningWorldEdited_ThrowsNullWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, worldPresenter);
        Assert.IsNull(workspaceVm.SelectedLearningWorld);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditCurrentLearningWorld("foo",
            "bar", "this", "does", "not", "matter"));
        Assert.AreEqual("SelectedLearningWorld is null", ex!.Message);
    }

    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null,
        ILearningWorldPresenter? learningWorldPresenter = null, ILearningSpacePresenter? learningSpacePresenter = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, learningWorldPresenter,
            learningSpacePresenter);
    }
}