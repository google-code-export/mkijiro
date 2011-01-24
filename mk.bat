copy MKIJIRO.prx MKIJIRO\MKIJIRO.prx
copy MKIJIRO_POPS.prx MKIJIRO\MKIJIRO_POPS.prx
copy MKIJIRO.prx patches\MKIJIRO.prx
copy MKIJIRO_POPS.prx patches\MKIJIRO_POPS.prx
del *.elf
del objects\*.o
move MKIJIRO_POPS.prx e:\seplugins\MKIJIRO_POPS.prx
move MKIJIRO.prx e:\seplugins\MKIJIRO.prx
del *.prx