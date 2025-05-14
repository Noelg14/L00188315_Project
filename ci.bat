dotnet restore .
dotnet publish --configuration Release --no-restore --property:PublishDir=./app
@rem cp L00188315_Project.Server/wwwroot/* L00188315_Project.Server/app/wwwroot
@rem xcopy L00188315_Project.Server\wwwroot\* L00188315_Project.Server\app\wwwroot
dir /s L00188315_Project.Server\app

