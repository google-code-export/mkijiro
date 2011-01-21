copy MKIJIRO.prx MKIJIRO\MKIJIRO.prx
copy MKIJIRO_SC.prx MKIJIRO\MKIJIRO_SC.prx
copy MKIJIRO_POPS.prx MKIJIRO\MKIJIRO_POPS.prx
copy MKIJIROHEN620.prx MKIJIROHEN635.prx
psp-fixup-imports -m patch\map620.txt MKIJIROHEN620.prx
psp-fixup-imports -m patch\map635.txt MKIJIROHEN635.prx
copy MKIJIROHEN620.prx MKIJIRO\MKIJIROHEN620.prx
copy MKIJIROHEN635.prx MKIJIRO\MKIJIROHEN635.prx
del *.elf
del objects\*.o
move MKIJIRO_POPS.prx g:\seplugins\MKIJIRO_POPS.prx
move MKIJIRO_SC.prx g:\seplugins\MKIJIRO_SC.prx
move MKIJIRO.prx f:\seplugins\MKIJIRO.prx
move MKIJIROHEN620.prx f:\seplugins\HEN620TA\MKIJIRO.prx
move MKIJIROHEN635.prx g:\plugins\MKIJIRO.prx