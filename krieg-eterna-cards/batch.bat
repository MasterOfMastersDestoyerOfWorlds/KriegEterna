SET WSPACE=%~dp0
SET "WSPACE=%WSPACE:\=\\%"
setlocal EnableDelayedExpansion


for /F "tokens=1-8 delims=	" %%A in (CardSheet.tsv) do (
	if "%%E"=="Ranged" (
		SET outlinedark=TRUE
	) else if "%%E"=="Melee" (
		SET outlinedark=TRUE
	) else if "%%E"=="Siege" (
		SET outlinedark=TRUE
	) else if "%%E"=="Power" (
		SET outlinedark=FALSE
	) else if "%%E"=="King" (
		SET outlinedark=MID
	) else if "%%E"=="Decoy" (
		SET outlinedark=TRUE
	) else if "%%E"=="Spy" (
		SET outlinedark=FALSE
	) else if "%%E"=="Weather" (
		SET outlinedark=FALSE
	)
	gimp-console-2.10.exe -i -b "(script-fu-compile-card \"%%B\" \"%%C\" \"%%D\" %%H \"%%E\" \"%%E-Title\" \"%%F\" \"%WSPACE%cropped\\%%A.xcf\" \"%WSPACE%icons\\embossed\\%%E.xcf\" \"%WSPACE%out\" \"%%A\" \"!outlinedark!\")" -b "(gimp-quit 0)"
)