c:\nuget\nuget.exe pack .\FsSpa-Knockout.nuspec
md c:\nuget\FsSpa-Knockout\
copy .\*.nupkg c:\nuget\FsSpa-Knockout\ /Y
c:\nuget\nuget.exe pack .\FsSpa-Knockout-Bundler.nuspec
md c:\nuget\FsSpa-Knockout-Bundler\
copy .\*.nupkg c:\nuget\FsSpa-Knockout-Bundler\ /Y
pause