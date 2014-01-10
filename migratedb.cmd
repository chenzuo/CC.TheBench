@echo off

.\src\.nuget\nuget.exe install src\.nuget\packages.config -source "https://nuget.org/api/v2/" -RequireConsent -o "lib"

if not exist out mkdir out

powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\lib\psake.4.3.1.0\tools\psake.ps1' MigrateDatabase %*; if ($psake.build_success -eq $false) { write-host "Migrate Failed!" -fore RED; exit 1 } else { exit 0 }"

pause