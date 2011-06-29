cd SRC_nitePR_revJ
make -f sc_slim_test
del *.elf
del *.o
copy nitePRmodSC_SLIM_TEST.prx nitePR\nitePRmodSC_SLIM_TEST.prx
move nitePRmodSC_SLIM_TEST.prx g:\seplugins\nitePRmodSC_SLIM_TEST.prx
move nitePRmodSC_SLIM_TEST.prx f:\seplugins\nitePRmodSC_SLIM_TEST.prx
move nitePRmodSC_SLIM_TEST.prx e:\seplugins\nitePRmodSC_SLIM_TEST.prx
del *.prx