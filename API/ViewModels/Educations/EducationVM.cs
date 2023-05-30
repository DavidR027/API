using API.Models;
using API.ViewModels.Universities;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Educations;

public class EducationVM
{
    public Guid? Guid { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    [Range(0, 4, ErrorMessage = "GPA must be between 0 to 4")]
    public float GPA { get; set; }
    public Guid UniversityGuid { get; set; }

/*    public static EducationVM ToVM(Education education)
    {
        return new EducationVM
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            GPA = education.GPA,
            UniversityGuid = education.UniversityGuid
        };
    }

    public static Education ToModel(EducationVM educationVM)
    {
        return new Education()
        {
            Guid = System.Guid.NewGuid(),
            Major = educationVM.Major,
            Degree = educationVM.Degree,
            GPA = educationVM.GPA,
            UniversityGuid = educationVM.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static Education ToModelU(EducationVM educationVM)
    {
        return new Education()
        {
            Guid = (Guid)educationVM.Guid,
            Major = educationVM.Major,
            Degree = educationVM.Degree,
            GPA = educationVM.GPA,
            UniversityGuid = educationVM.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }*/
}