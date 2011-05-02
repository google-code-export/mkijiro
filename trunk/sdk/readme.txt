SDK for 3.71 M33

Some changes have been added in this sdk: sctrlHENSetOnApplyPspRelSectionEven has been replaced
by sctrlHENSetStartModuleHandler.

Sony has changed a lot of kernel nids: ctrl, power, syscon, sysreg, clockgen, umd modules, etc
If your prx doesn't load, dump your nids using prxtool, and compare them to the ones of 3.71.

Sony has also deleted the power speed functions from kernel usage. You can use the new function
sctrlHENSetSpeed to fix this issue.

Included is a sample that uses the 32 MB of extra ram of the psp slim.