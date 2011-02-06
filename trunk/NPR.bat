cd SRC_nitePR_revJ
make
cd ..
del SRC_nitePR_revJ\*.elf
del SRC_nitePR_revJ\*.o
copy SRC_nitePR_revJ\nitePR.prx SRC_nitePR_revJ\nitePR\nitePR.prx
copy SRC_nitePR_revJ\nitePR.prx patches\nitePR.prx
move SRC_nitePR_revJ\nitePR.prx g:\seplugins\nitePR.prx
move SRC_nitePR_revJ\nitePR.prx e:\seplugins\nitePR.prx