@echo off
setlocal enableDelayedExpansion

:: Display the output of each process if the /O option is used
:: else ignore the output of each process
if /i "%~1" equ "/O" (
  set "lockHandle=1"
  set "showOutput=1"
) else (
  set "lockHandle=1^>nul 9"
  set "showOutput="
)

:: Define the maximum number of parallel processes to run.
set "maxProc=5"

:: Get a unique base lock name for this particular instantiation.
:: Incorporate a timestamp from WMIC if possible, but don't fail if
:: WMIC not available. Also incorporate a random number.
  set "lock="
  for /f "skip=1 delims=-+ " %%T in ('2^>nul wmic os get localdatetime') do (
    set "lock=%%T"
    goto :break
  )
  :break
  set "lock=%temp%\lock%lock%_%random%_"

:: Initialize the counters
  set /a "startCount=0, endCount=0"

:: Clear any existing end flags
  for /l %%N in (1 1 %maxProc%) do set "endProc%%N="

:: Launch the commands in a loop
  set launch=1
  SET WSPACE=%~dp0
  SET "WSPACE=%WSPACE:\=\\%"
  for /F "tokens=1-6 delims=	" %%A in (CardSheet.tsv) do (
    if !startCount! lss %maxProc% (
      set /a "startCount+=1, nextProc=startCount"
    ) else (
      call :wait
    )
    set cmd!nextProc!=%%A
    if defined showOutput echo -------------------------------------------------------------------------------
    echo !time! - proc!nextProc!: starting %%A
    2>nul del %lock%!nextProc!
    %= Redirect the lock handle to the lock file. The CMD process will     =%
    %= maintain an exclusive lock on the lock file until the process ends. =%
    start /b "" cmd /c %lockHandle%^>"%lock%!nextProc!" 2^>^&1 gimp-console-2.10.exe -i -b "(script-fu-compile-card \"%%B\" \"%%C\" \"%%D\" \"%%E\" \"%%E-Title\" \"%%F\" \"%WSPACE%cropped\\%%A.xcf\" \"%WSPACE%icons\\embossed\\%%E.xcf\" \"%WSPACE%out\\%%A\")" -b "(gimp-quit 0)""
  )
  set "launch="

:wait
:: Wait for procs to finish in a loop
:: If still launching then return as soon as a proc ends
:: else wait for all procs to finish
  :: redirect stderr to null to suppress any error message if redirection
  :: within the loop fails.
  for /l %%N in (1 1 %startCount%) do 2>nul (
    %= Redirect an unused file handle to the lock file. If the process is    =%
    %= still running then redirection will fail and the IF body will not run =%
    if not defined endProc%%N if exist "%lock%%%N" 9>>"%lock%%%N" (
      %= Made it inside the IF body so the process must have finished =%
      if defined showOutput echo ===============================================================================
      echo !time! - proc%%N: finished !cmd%%N!
      if defined showOutput type "%lock%%%N"
      if defined launch (
        set nextProc=%%N
        exit /b
      )
      set /a "endCount+=1, endProc%%N=1"
    )
  )
  if %endCount% lss %startCount% (
    1>nul 2>nul ping /n 2 ::1
    goto :wait
  )

2>nul del %lock%*
if defined showOutput echo ===============================================================================
echo Done