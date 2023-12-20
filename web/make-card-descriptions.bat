@echo off

set "string1=%1"
set "string2=%2"
set "string3=%3"
set "string4=%4"

setlocal EnableDelayedExpansion

set "htmlFile=%5"
set "headerFile=%6"
set "footerFile=%7"

set "paragraphTag="<p>"
set "closingParagraphTag="</p>"

set "headerContent="
for /F "tokens=* delims=:" %%a in ('type !headerFile!') do (
    set "headerContent=%%a"
)

set "footerContent="
for /F "tokens=* delims=:" %%a in ('type !footerFile!') do (
    set "footerContent=%%a"
)

findstr /c: "</header>" < !htmlFile! > NUL
if !errorlevel! neq 0 (
    echo "Could not find header in HTML file."
    exit /b
)

findstr /c: "</footer>" < !htmlFile! > NUL
if !errorlevel! neq 0 (
    echo "Could not find footer in HTML file."
    exit /b
)

set "headerLine="
for /F "tokens=* delims=:" %%a in ('type !htmlFile!') do (
    set "headerLine=%%a"
    if "%%a" == "</header>" (
        echo !headerContent!
        echo !paragraphTag!!string1!!closingParagraphTag!
        echo !paragraphTag!!string2!!closingParagraphTag!
        echo !paragraphTag!!string3!!closingParagraphTag!
        echo !paragraphTag!!string4!!closingParagraphTag!
        echo !footerContent!
        echo %%a
    ) else (
        echo %%a
    )
) > !htmlFile!