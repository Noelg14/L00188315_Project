dotnet ef migrations add SQLite -p L00188315_Project.Infrastructure -c AppDbContext -s L00188315_Project.Server -o Data/Migrations
dotnet ef migrations add SQLiteID -p L00188315_Project.Infrastructure -c AppIdentityDbContext -s L00188315_Project.Server -o Data/Migrations

dotnet ef database update -p L00188315_Project.Infrastructure -s L00188315_Project.Server -c AppDbContext
dotnet ef database update -p L00188315_Project.Infrastructure -s L00188315_Project.Server -c AppIdentityDbContext