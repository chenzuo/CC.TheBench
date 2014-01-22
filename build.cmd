@echo off

.\nuget.exe install src\packages.config -RequireConsent -o "lib" -ConfigFile src\nuget.config

if not exist out mkdir out

powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\lib\psake.4.3.1.0\tools\psake.ps1' %*; if ($psake.build_success -eq $false) { write-host "Build Failed!" -fore RED; exit 1 } else { exit 0 }"

pause