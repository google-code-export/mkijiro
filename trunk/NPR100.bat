cd SRC_nitePR_revJ
make -f 100
del *.elf
del *.o
copy nitePRmod100.prx nitePR\nitePRmod100.prx
move nitePRmod100.prx g:\seplugins\nitePRmod100.prx
move nitePRmod100.prx f:\seplugins\nitePRmod100.prx
move nitePRmod100.prx e:\seplugins\nitePRmod100.prx
del *.prx