if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

EnsurePsbuildInstalled

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revision = "{0:D4}" -f [convert]::ToInt32($revision, 10)

& dotnet restore Cocotte\Cocotte.csproj
& dotnet pack Cocotte\Cocotte.csproj -c Release -o ..\artifacts --version-suffix $revision-prerelease 