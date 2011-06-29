cd SRC_nitePR_revJ
make -f sc
del *.elf
del *.o
copy nitePRmodSC.prx nitePR\nitePRmodSC.prx
move nitePRmodSC.prx g:\seplugins\nitePRmodSC.prx
move nitePRmodSC.prx f:\seplugins\nitePRmodSC.prx
move nitePRmodSC.prx e:\seplugins\nitePRmodSC.prx
del *.prx