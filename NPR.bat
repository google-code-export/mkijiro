cd SRC_nitePR_revJ
make
cd ..
del SRC_nitePR_revJ\*.elf
del SRC_nitePR_revJ\*.o
copy SRC_nitePR_revJ\nitePRmod.prx SRC_nitePR_revJ\nitePR\nitePRmod.prx
copy SRC_nitePR_revJ\nitePRmod.prx patches\nitePRmod.prx
move SRC_nitePR_revJ\nitePRmod.prx g:\seplugins\nitePRmod.prx
move SRC_nitePR_revJ\nitePRmod.prx e:\seplugins\nitePRmod.prx
del RC_nitePR_revJ\*.prx