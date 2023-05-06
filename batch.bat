for /F "tokens=1-6 delims=	" %%A in (CardSheet.tsv) do (
	gimp-console-2.10.exe -i -b "(script-fu-compile-card \"%%B\" \"%%C\" \"%%D\" \"%%E\" \"%%F\" \"C:\\Users\\Drew\\Documents\\Gimp\\KriegEterna\\cropped\\%%A.xcf\" \"C:\\Users\\Drew\\Documents\\Gimp\\KriegEterna\\icons\\embossed\\%%E.xcf\" \"C:\\Users\\Drew\\Documents\\Gimp\\KriegEterna\\out\\%%A.png\")" -b "(gimp-quit 0)"
)