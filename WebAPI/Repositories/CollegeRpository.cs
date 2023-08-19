using WebAPI.Models;

namespace WebAPI.Repositories
{
    public static class CollegeRpository
    {
        public static List<Student> Students { get; set; } = new List<Student>{
                new Student {
                Id=Guid.NewGuid(),
                Name="Ferid",
                Gender="Male",
                Age=52},
                new Student
                {
                    Id=Guid.NewGuid(),
                    Name="Mohamed Alaa",
                    Gender="Male",
                    Age=15
                }
            };
    }
}
