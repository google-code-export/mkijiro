make -f swap
copy MKIJIROSWAP.prx MKIJIRO\MKIJIROSWAP.prx
del *.elf
del objects\*.o
move MKIJIROSWAP.prx g:\seplugins\MKIJIROSWAP.prx
move MKIJIROSWAP.prx f:\seplugins\MKIJIROSWAP.prx
move MKIJIROSWAP.prx e:\seplugins\MKIJIROSWAP.prx
del *.prx