@echo off

.\src\.nuget\nuget.exe install src\.nuget\packages.config -source "https://nuget.org/api/v2/" -RequireConsent -o "lib"

powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\lib\psake.4.3.1.0\tools\psake.ps1' ServeSite %*; if ($psake.build_success -eq $false) { write-host "Serve Failed!" -fore RED; exit 1 } else { exit 0 }"
