@echo off

echo Exalt Account Manager Backup Tool by Maik8
echo: 
echo Disclaimer: NO WARRANTYS OR COVER FOR LOST DATA!
echo:
echo:
:SelectMode
echo Select mode:
echo 1: Create a backup
echo 2: Rollback to current backup
echo Select mode by typing 1 or 2 and press enter.

set /P mode=""

if %mode% equ 1 (
	goto CreateBackup
) else if %mode% equ 2 (
	goto Rollback
) else (
	cls
	echo #######################
	echo # Wrong input format! #
	echo #######################
	echo:
	goto SelectMode
)

:CreateBackup
if exist .\EAMBackup\ (
	rmdir /s/q .\EAMBackup
	echo Deleted old backup.
)

echo Starting the backup process...
xcopy "C:\Users\%username%\AppData\Local\ExaltAccountManager" ".\EAMBackup"  /S /E /C /Y /I > Nul
echo:
echo Backup DONE!
echo:
pause
exit

:Rollback
if exist .\EAMBackup\ (
	if exist C:\Users\%username%\AppData\Local\ExaltAccountManager\ (
		rmdir /s/q C:\Users\%username%\AppData\Local\ExaltAccountManager
		echo Deleted old files
	)
	echo Starting the rollback...
	xcopy ".\EAMBackup" "C:\Users\%username%\AppData\Local\ExaltAccountManager" /S /E /C /Y /I > Nul
	echo Rollback DONE!
	pause
	exit
) else ( 
	echo:
	echo No backup found!
	echo Aborting without changes!
	echo:
	pause
	exit
)
