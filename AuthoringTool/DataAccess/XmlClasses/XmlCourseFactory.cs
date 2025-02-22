﻿using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities.course;

namespace AuthoringTool.DataAccess.XmlClasses;

/// <summary>
/// Sets the paramters of course/course.xml, course/enrolments.xml, course/inforef.xml, course/roles.xml,
/// course/completiondefaults.xml and creates the files
/// </summary>
public class XmlCourseFactory
{
    private string currentTime;
    private LearningWorldJson? learningWorld;
    internal ICourseCourseXmlCategory CourseCourseXmlCategory { get; }
    internal ICourseCourseXmlCourse CourseCourseXmlCourse { get; }
    internal ICourseEnrolmentsXmlEnrol CourseEnrolmentsXmlEnrolManual { get; }
    internal ICourseEnrolmentsXmlEnrol CourseEnrolmentsXmlEnrolGuest { get; }
    internal ICourseEnrolmentsXmlEnrol CourseEnrolmentsXmlEnrolSelf { get; }
    internal ICourseEnrolmentsXmlEnrolments CourseEnrolmentsXmlEnrolments { get; }
    internal ICourseEnrolmentsXmlEnrols CourseEnrolmentsXmlEnrols { get; }
    internal ICourseInforefXmlInforef CourseInforefXmlInforef { get; }
    internal ICourseInforefXmlRole CourseInforefXmlRole { get; }
    internal ICourseInforefXmlRoleref CourseInforefXmlRoleref { get; }
    internal ICourseRolesXmlRoles CourseRolesXmlRoles { get; }
    internal ICourseCompletiondefaultXmlCourseCompletionDefaults CourseCompletiondefaultXmlCourseCompletionDefaults { get; }
    internal IReadDSL? ReadDsl { get; }

   
    public XmlCourseFactory(IReadDSL readDsl, ICourseCourseXmlCategory? courseCourseXmlCategory=null, ICourseCourseXmlCourse? courseCourseXmlCourse=null,
        ICourseEnrolmentsXmlEnrol? courseEnrolmentsXmlEnrol=null, ICourseEnrolmentsXmlEnrols? courseEnrolmentsXmlEnrols=null, 
        ICourseEnrolmentsXmlEnrolments? courseEnrolmentsXmlEnrolments=null, ICourseInforefXmlRole? courseInforefXmlRole=null, 
        ICourseInforefXmlRoleref? courseInforefXmlRoleref=null, ICourseInforefXmlInforef? courseInforefXmlInforef=null, 
        ICourseRolesXmlRoles? courseRolesXmlRoles=null, ICourseCompletiondefaultXmlCourseCompletionDefaults? courseCourseXmlCompletiondefault=null)
    {
        CourseCourseXmlCategory = courseCourseXmlCategory?? new CourseCourseXmlCategory();
        CourseCourseXmlCourse = courseCourseXmlCourse?? new CourseCourseXmlCourse();

        CourseEnrolmentsXmlEnrolManual = courseEnrolmentsXmlEnrol?? new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolGuest = courseEnrolmentsXmlEnrol?? new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrolSelf = courseEnrolmentsXmlEnrol?? new CourseEnrolmentsXmlEnrol();
        CourseEnrolmentsXmlEnrols = courseEnrolmentsXmlEnrols?? new CourseEnrolmentsXmlEnrols();
        CourseEnrolmentsXmlEnrolments = courseEnrolmentsXmlEnrolments?? new CourseEnrolmentsXmlEnrolments();

        CourseInforefXmlRole = courseInforefXmlRole?? new CourseInforefXmlRole();
        CourseInforefXmlRoleref = courseInforefXmlRoleref?? new CourseInforefXmlRoleref();
        CourseInforefXmlInforef = courseInforefXmlInforef?? new CourseInforefXmlInforef();

        CourseRolesXmlRoles = courseRolesXmlRoles?? new CourseRolesXmlRoles();
        
        CourseCompletiondefaultXmlCourseCompletionDefaults = courseCourseXmlCompletiondefault?? new CourseCompletiondefaultXmlCourseCompletionDefaults();
        
        ReadDsl = readDsl;
        learningWorld = readDsl.GetLearningWorld();
        currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
    }
    
    /// <summary>
    /// Use all the methods of the current class
    /// </summary>
    public void CreateXmlCourseFactory()
    {
        //Sets the parameter and creates Course/course.xml
        CreateCourseCourseXml();
        
        //Sets the parameter and creates Course/enrolments.xml
        CreateCourseEnrolmentsXml();
        
        //Sets the parameter and creates Course/inforef.xml
        CreateCourseInforefXml();
        
        //Sets the parameter and creates Course/roles.xml
        CreateCourseRolesXml();
        
        //Sets the parameter and creates Course/completiondefault.xml
        CreateCourseCompletiondefault();
    }
    
    
    public void CreateCourseCourseXml()
    {
        //set parameters of the course/course.xml file
        if (learningWorld != null)
            if (learningWorld.identifier != null)
            {
                CourseCourseXmlCourse.Shortname = learningWorld.identifier.value;
                CourseCourseXmlCourse.Fullname = learningWorld.identifier.value;
                CourseCourseXmlCourse.Format = "tiles";
                CourseCourseXmlCourse.BaseColour = "#009681";
                CourseCourseXmlCourse.CourseUseSubtiles = "1";
                CourseCourseXmlCourse.CourseShowTileProgress = "2";
                CourseCourseXmlCourse.ShowGrades = "1";
                CourseCourseXmlCourse.Visible = "1";
                CourseCourseXmlCourse.Theme = "boost";
                CourseCourseXmlCourse.ShowCompletionConditions = "1";
                CourseCourseXmlCourse.EnableCompletion = "1";
                CourseCourseXmlCourse.Category = (CourseCourseXmlCategory) CourseCourseXmlCategory;

                //create course/course.xml file
                CourseCourseXmlCourse.Serialize();
            }
    }

    public void CreateCourseEnrolmentsXml()
    {
        //set parameters of the course/enrolments.xml file
        //the enrolments.xml file is identical in every moodle backup file
        // the property "status" shows which enrolment Method is selected: 0=selected; 1=not selected
        CourseEnrolmentsXmlEnrolManual.Id = "1";
        CourseEnrolmentsXmlEnrolManual.RoleId = "5";
        CourseEnrolmentsXmlEnrolManual.EnrolMethod = "manual";
        CourseEnrolmentsXmlEnrolManual.Status = "1";
        
        CourseEnrolmentsXmlEnrolGuest.Id = "2";
        CourseEnrolmentsXmlEnrolGuest.RoleId = "0";
        CourseEnrolmentsXmlEnrolGuest.EnrolMethod = "guest";
        CourseEnrolmentsXmlEnrolGuest.Status = "0";

        CourseEnrolmentsXmlEnrolSelf.Id = "3";
        CourseEnrolmentsXmlEnrolSelf.RoleId = "5";
        CourseEnrolmentsXmlEnrolSelf.EnrolMethod = "self";
        CourseEnrolmentsXmlEnrolSelf.Status = "1";
        CourseEnrolmentsXmlEnrolSelf.CustomInt1 = "0";
        CourseEnrolmentsXmlEnrolSelf.CustomInt2 = "0";
        CourseEnrolmentsXmlEnrolSelf.CustomInt3 = "0";
        CourseEnrolmentsXmlEnrolSelf.CustomInt4 = "1";
        CourseEnrolmentsXmlEnrolSelf.CustomInt5 = "0";
        CourseEnrolmentsXmlEnrolSelf.CustomInt6 = "1";

        CourseEnrolmentsXmlEnrols.Enrol.Add(CourseEnrolmentsXmlEnrolManual as CourseEnrolmentsXmlEnrol);
        CourseEnrolmentsXmlEnrols.Enrol.Add(CourseEnrolmentsXmlEnrolGuest as CourseEnrolmentsXmlEnrol);
        CourseEnrolmentsXmlEnrols.Enrol.Add(CourseEnrolmentsXmlEnrolSelf as CourseEnrolmentsXmlEnrol);

        CourseEnrolmentsXmlEnrolments.Enrols = (CourseEnrolmentsXmlEnrols) CourseEnrolmentsXmlEnrols;
        
        //create course/enrolments.xml file
        CourseEnrolmentsXmlEnrolments.Serialize();
    }

    public void CreateCourseInforefXml()
    {
        //set parameters of the course/inforef.xml file
        CourseInforefXmlRoleref.Role = (CourseInforefXmlRole) CourseInforefXmlRole;
        CourseInforefXmlInforef.Roleref = (CourseInforefXmlRoleref) CourseInforefXmlRoleref;
        
        //create course/inforef.xml file
        CourseInforefXmlInforef.Serialize();
    }

    public void CreateCourseRolesXml()
    {
        //create course/roles.xml file
        CourseRolesXmlRoles.Serialize();
    }

    public void CreateCourseCompletiondefault()
    {
        //create course/completiondefaults.xml
        //The file is empty, therefore no parameters need to be set yet.
        CourseCompletiondefaultXmlCourseCompletionDefaults.Serialize();
    }

}