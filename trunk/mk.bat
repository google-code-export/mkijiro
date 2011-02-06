make
copy MKIJIRO.prx MKIJIRO\MKIJIRO.prx
copy MKIJIRO.prx patches\MKIJIRO.prx
del *.elf
del objects\*.o
move MKIJIRO.prx g:\seplugins\MKIJIRO.prx
move MKIJIRO.prx e:\seplugins\MKIJIRO.prx
del *.prx