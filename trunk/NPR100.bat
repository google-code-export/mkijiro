cd SRC_nitePR_revJ
make -f 100
cd ..
del SRC_nitePR_revJ\*.elf
del SRC_nitePR_revJ\*.o
copy SRC_nitePR_revJ\nitePRmod100.prx SRC_nitePR_revJ\nitePR\nitePRmod100.prx
move SRC_nitePR_revJ\nitePRmod100.prx g:\seplugins\nitePRmod100.prx
move SRC_nitePR_revJ\nitePRmod100.prx e:\seplugins\nitePRmod100.prx
del SRC_nitePR_revJ\*.prx