make -f pops
copy MKIJIRO_POPS.prx MKIJIRO\MKIJIRO_POPS.prx
copy MKIJIRO_POPS.prx patches\MKIJIRO_POPS.prx
del *.elf
del objects\*.o
move MKIJIRO_POPS.prx g:\seplugins\MKIJIRO_POPS.prx
move MKIJIRO_POPS.prx f:\seplugins\MKIJIRO_POPS.prx
move MKIJIRO_POPS.prx e:\seplugins\MKIJIRO_POPS.prx
del *.prx