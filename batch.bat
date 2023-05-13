SET WSPACE=%~dp0
SET "WSPACE=%WSPACE:\=\\%"
for /F "tokens=1-6 delims=	" %%A in (CardSheet.tsv) do (
	gimp-console-2.10.exe -i -b "(script-fu-compile-card \"%%B\" \"%%C\" \"%%D\" \"%%E\" \"%%F\" \"%WSPACE%cropped\\%%A.xcf\" \"%WSPACE%icons\\embossed\\%%E.xcf\" \"%WSPACE%out\\%%A.png\")" -b "(gimp-quit 0)"
)