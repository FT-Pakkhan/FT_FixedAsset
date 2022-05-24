;#DropDB FT_FixedAssetEasyTest

#Application FT_FixedAssetWin
#Application FT_FixedAssetWeb

*FillForm
 User Name = Admin
*Action Log In

#IfDef FT_FixedAssetWin

*Action About...
 
*Action OK

#EndIf

#IfDef FT_FixedAssetWeb
*Action Navigation(My Details)
#EndIf
