cd SRC_nitePR_revJ
make -f sc
cd ..
del SRC_nitePR_revJ\*.elf
del SRC_nitePR_revJ\*.o
copy SRC_nitePR_revJ\nitePRmodSC.prx SRC_nitePR_revJ\nitePR\nitePRmodSC.prx
move SRC_nitePR_revJ\nitePRmodSC.prx g:\seplugins\nitePRmodSC.prx
move SRC_nitePR_revJ\nitePRmodSC.prx f:\seplugins\nitePRmodSC.prx
move SRC_nitePR_revJ\nitePRmodSC.prx e:\seplugins\nitePRmodSC.prx
del SRC_nitePR_revJ\*.prx