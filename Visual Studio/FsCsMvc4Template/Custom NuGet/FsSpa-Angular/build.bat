c:\nuget\nuget.exe pack .\FsSpa-Angular.nuspec
md c:\nuget\FsSpa-Angular\
copy .\*.nupkg c:\nuget\FsSpa-Angular\ /Y
c:\nuget\nuget.exe pack .\FsSpa-Angular-Bundler.nuspec
md c:\nuget\FsSpa-Angular-Bundler\
copy .\*.nupkg c:\nuget\FsSpa-Angular-Bundler\ /Y
pause