SET WSPACE=%~dp0
SET "WSPACE=%WSPACE:\=\\%"
SET TYPE="Siege"
SET TEXT="Pass"
setlocal EnableDelayedExpansion


if %TYPE%=="Ranged" (
	SET outlinedark=TRUE
) else if %TYPE%=="Melee" (
	SET outlinedark=TRUE
) else if %TYPE%=="Siege" (
	SET outlinedark=TRUE
) else if %TYPE%=="Power" (
	SET outlinedark=FALSE
) else if %TYPE%=="King" (
	SET outlinedark=MID
) else if %TYPE%=="Decoy" (
	SET outlinedark=TRUE
) else if %TYPE%=="Spy" (
	SET outlinedark=FALSE
) else if %TYPE%=="Weather" (
	SET outlinedark=FALSE
)
gimp-console-2.10.exe -i -b "(script-fu-gen-text \"%TEXT%\"  \"%TYPE%-Title\"  \"%WSPACE%text\" \"%TEXT%%TYPE%\" \"!outlinedark!\")" -b "(gimp-quit 0)"
