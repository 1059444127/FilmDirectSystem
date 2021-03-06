; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "胶片直投"
#define MyAppVersion "1.10.170626"
#define MyAppPublisher "上海实和智能电子科技有限公司"
#define MyAppURL "http://www.i-seehealth.com/"
#define MyAppExeName "FilmDirectSystem.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{E20029CD-37B3-4E7F-9B72-BAE978277152}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\FilmDirectSystem
DisableProgramGroupPage=yes
OutputDir=F:\软件
OutputBaseFilename=FilmDirectSystem_Setup_V1.10.170626
SetupIconFile=D:\workspace\胶片直投系统V2.0\FilmDirectSystem\logo.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\workspace\胶片直投系统V2.0\FilmDirectSystem\bin\Debug\FilmDirectSystem.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\workspace\胶片直投系统V2.0\FilmDirectSystem\bin\Debug\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

