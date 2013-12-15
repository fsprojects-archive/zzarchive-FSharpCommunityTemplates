@echo off
for /D %%k in (FsWebAddRegistryEntries*) do (
  nuget\nuget.exe pack %%k\%%k.nuspec -BasePath %%k -o %%k
  copy %%k\*.nupkg nuget
)

pause