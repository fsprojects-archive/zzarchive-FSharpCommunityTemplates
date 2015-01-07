param($rootPath, $toolsPath, $package, $project)

Function Add-Registry-Keys ($key) {  
    New-ItemProperty -Path $key -Name '{F2A71F9B-5D33-465A-A702-920D77279786}' -PropertyType String -Value '{F2A71F9B-5D33-465A-A702-920D77279786}' -ErrorAction SilentlyContinue
}  

Add-Registry-Keys 'HKCU:\Software\Microsoft\VWDExpress\11.0_Config\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKCU:\Software\Microsoft\VSWinDesktopExpress\12.0_Config\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKCU:\Software\Microsoft\VisualStudio\12.0_Config\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKCU:\Software\Microsoft\VisualStudio\14.0_Config\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'

Add-Registry-Keys 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\VWDExpress\11.0\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\11.0\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\VSWinDesktopExpress\12.0\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\12.0\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
Add-Registry-Keys 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\14.0\Projects\{349C5851-65DF-11DA-9384-00065B846F21}\LanguageTemplates'
