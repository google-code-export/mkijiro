cd SRC_nitePR_revJ
make -f nohb
cd ..
del SRC_nitePR_revJ\*.elf
del SRC_nitePR_revJ\*.o
copy SRC_nitePR_revJ\nitePRmodNOHB.prx SRC_nitePR_revJ\nitePR\nitePRmodNOHB.prx
move SRC_nitePR_revJ\nitePRmodNOHB.prx g:\seplugins\nitePRmodNOHB.prx
move SRC_nitePR_revJ\nitePRmodNOHB.prx e:\seplugins\nitePRmodNOHB.prx
del SRC_nitePR_revJ\*.prx