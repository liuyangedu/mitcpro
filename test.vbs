
Rem ����ΪVbScript�ű�
Set WshShell = WScript.CreateObject("WScript.Shell")
strDesktop = WshShell.SpecialFolders("Desktop") :'�����ļ��С����桱

Dim folderName1
folderName1 = ""

Dim fso1
Set fso1 = CreateObject("Scripting.FileSystemObject")

Dim fullpath
fullpath = fso1.GetAbsolutePathName(folderName1)


set oShellLink = WshShell.CreateShortcut(strDesktop & "\�Ķ�����ERPϵͳ.lnk")
oShellLink.TargetPath = fullpath  + "\mySystem\mySystem\bin\Release\mySystem.exe" : 'Ŀ��
oShellLink.WindowStyle = 3 :'����1Ĭ�ϴ��ڼ������3��󻯼������7��С��
oShellLink.IconLocation = fullpath  + "\mySystem\mySystem\pic\logo32.png"
oShellLink.WorkingDirectory = fullpath  + "\mySystem\mySystem\bin\Release" '��ʼλ��
oShellLink.Save : '���������ݷ�ʽ
