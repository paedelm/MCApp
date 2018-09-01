@echo off
if not %1. == . goto git
echo *******************
echo %~nx0 "commit message"
echo *******************
goto :EOF
:git
git status
git add .
git commit -m "%~1"
git push azurebob master