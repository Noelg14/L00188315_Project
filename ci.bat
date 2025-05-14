dotnet restore .
dotnet build --no-restore -c Release
dotnet publish --configuration Release --no-restore --no-build --property:PublishDir=./app
@rem cp L00188315_Project.Server/wwwroot/* L00188315_Project.Server/app/wwwroot
@rem xcopy L00188315_Project.Server\wwwroot\* L00188315_Project.Server\app\wwwroot
@rem dir /s L00188315_Project.Server\app

