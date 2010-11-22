copy MKIJIRO.prx MKIJIRO\MKIJIRO.prx
copy MKIJIRO_SC.prx MKIJIRO\MKIJIRO_SC.prx
del *.elf
del objects\*.o
move MKIJIRO_SC.prx g:\seplugins\mkultra.prx
move MKIJIRO.prx g:\seplugins\mkultra_psplink.prx