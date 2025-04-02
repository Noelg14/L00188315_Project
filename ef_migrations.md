dotnet ef migrations add InitialCreate -p L00188315_Project.Infrastructure -c AppDbContext -s L00188315_Project.Server -o Data/Migrations
dotnet ef migrations add InitialCreateID -p L00188315_Project.Infrastructure -c AppIdentityDbContext -s L00188315_Project.Server -o Data/Migrations


dotnet ef database update -p L00188315_Project.Infrastructure -s L00188315_Project.Server -c AppDbContext
dotnet ef database update -p L00188315_Project.Infrastructure -s L00188315_Project.Server -c AppIdentityDbContext



dotnet ef migrations add AddUserId -p L00188315_Project.Infrastructure -c AppDbContext -s L00188315_Project.Server -o Data/Migrations
dotnet ef migrations add UpdateConsent -p L00188315_Project.Infrastructure -c AppDbContext -s L00188315_Project.Server -o Data/Migrations

dotnet ef database update -p L00188315_Project.Infrastructure -s L00188315_Project.Server -c AppDbContext



atu_project
!i%0kdAhRfL@?B4

Server=tcp:l00188315-project.database.windows.net,1433;Initial Catalog=L00188315-Project;Persist Security Info=False;User ID=atu_project;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;