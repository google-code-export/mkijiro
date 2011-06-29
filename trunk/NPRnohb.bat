cd SRC_nitePR_revJ
make -f nohb
del *.elf
del *.o
copy nitePRmodNOHB.prx nitePR\nitePRmodNOHB.prx
move nitePRmodNOHB.prx g:\seplugins\nitePRmodNOHB.prx
move nitePRmodNOHB.prx f:\seplugins\nitePRmodNOHB.prx
move nitePRmodNOHB.prx e:\seplugins\nitePRmodNOHB.prx
del *.prx