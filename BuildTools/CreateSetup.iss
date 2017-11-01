; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Tales of Ascaria"
#define MyAppVersion "1.0"
#define MyAppPublisher "Silverplay Studio"
#define MyAppExeName "ProjetSynthese"
#define MyAppDataDirName MyAppExeName + "_Data"
#define MyAppExeFullName MyAppExeName + ".exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{8E2C992D-BE51-4249-964C-81669B94DFA4}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\LICENCE.md
OutputDir=..\Build\Setup
OutputBaseFilename={#MyAppExeName}Setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\Build\ProjetSynthese\{#MyAppExeFullName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Build\ProjetSynthese\{#MyAppDataDirName}\*"; DestDir: "{app}\{#MyAppDataDirName}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeFullName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeFullName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeFullName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

