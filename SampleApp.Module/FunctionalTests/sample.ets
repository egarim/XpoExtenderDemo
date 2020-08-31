;#DropDB SampleAppEasyTest

#Application SampleApp.Win
#Application SampleApp.Web
#IfDef SampleApp.Win

*Action About...
 
*Action OK

#EndIf

#IfDef SampleApp.Web
!Action Navigation(My Details)
#EndIf
