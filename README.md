community-templates
===================

Templates for F# projects, by the F# community, for use in Visual Studio, Xamarin Studio and other editors

The plan is that there will ne multiple directories, for Visual Studio, MonoDevelop etc.

The Visual Studio templates are shipped on Visual Studio Gallery.


## Example of how to edit/develop existing Visual Studio Templates 

Your contributions to this work are immensely welcome! If you contribute, we love you!

You need the Visual Studio 2012 SDK (when editing the templates with that version of Visual Studio)

1. *Edit the template* 

  * Extract ```VisualStudio\FsCsMvc4Template\template\FsMvc4VSIX\ProjectTemplates\ASPNET\FsMvc4.zip```

  * Edit the files under ```VisualStudio\FsCsMvc4Template\template\FsMvc4VSIX\ProjectTemplates\ASPNET\FsMvc4```

  * Rezip the files under ```VisualStudio\FsCsMvc4Template\template\FsMvc4VSIX\ProjectTemplates\ASPNET\FsMvc4``` into FsMvc4.zip and move up one directory

  * NOTE: when rezipping, make sure you didn't create an extra directory level called 'FsMvc4' in the zip.

2. *Bump the version number* - Edit in ```C:\GitHub\fsharp\community-templates\VisualStudio\FsCsMvc4Template\template\FsMvc4VSIX\source.extension.vsixmanifest```

3. *Build* - Open ```VisualStudio\FsCsMvc4Template\FsCsMvc4Template.sln```  and build

4. *Install* - Go to ```C:\GitHub\fsharp\community-templates\VisualStudio\FsCsMvc4Template\template\FsMvc4VSIX\bin\Debug``` and double-click on the VSIX to install it.

5. *Push* - Push your improvements to ```https://github.com/fsharp/community-templates/```. The maintainers will 

  * Check the adjustments 

  * Test in Windows 7, Windows 8 and multiple versions of Visual Studio

  * Push new templates to Visual Studio Gallery







