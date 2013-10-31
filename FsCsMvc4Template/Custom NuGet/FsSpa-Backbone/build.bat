c:\nuget\nuget.exe pack .\FsSpa-Backbone.nuspec
md c:\nuget\FsSpa-Backbone\
copy .\*.nupkg c:\nuget\FsSpa-Backbone\ /Y
c:\nuget\nuget.exe pack .\FsSpa-Backbone-Bundler.nuspec
md c:\nuget\FsSpa-Backbone-Bundler\
copy .\*.nupkg c:\nuget\FsSpa-Backbone-Bundler\ /Y
pause