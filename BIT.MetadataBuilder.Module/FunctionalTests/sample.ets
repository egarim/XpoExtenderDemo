;#DropDB BIT.MetadataBuilderEasyTest

#Application BIT.MetadataBuilder.Win
#Application BIT.MetadataBuilder.Web

*FillForm
 User Name = Admin
*Action Log In

#IfDef BIT.MetadataBuilder.Win

*Action About...
 
*Action OK

#EndIf

#IfDef BIT.MetadataBuilder.Web
*Action Navigation(My Details)
#EndIf
