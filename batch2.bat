for /F "tokens=1-6 delims=	" %%A in (CardSheet.tsv) do (
	gimp-console-2.10.exe -i -b "(script-fu-compile-card \"%%B\" "%%C\" \"%%D\" \"%%E\" %%F \"C:\\Users\\Drew\\Documents\\Gimp\\cropped\\%%A.xcf\" \"C:\\Users\\Drew\\Documents\\Gimp\\icons\\%%E.xcf\" \"C:\\Users\\Drew\\Documents\\Gimp\\out\\%%A.png\")" -b "(gimp-quit 0)"
)