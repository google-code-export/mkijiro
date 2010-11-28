#!/bin/bash
rm *.elf
rm objects/*.o
cp MKIJIRO.prx MKIJIRO/MKIJIRO.prx
cp MKIJIRO.prx /media/disk/seplugins/MKIJIRO.prx
cp MKIJIRO_SC.prx MKIJIRO/MKIJIRO_SC.prx
cp MKIJIRO_SC.prx /media/disk/seplugins/MKIJIRO_SC.prx
cp MKIJIRO_POPS.prx MKIJIRO/MKIJIRO_POPS.prx
cp MKIJIRO_POPS.prx /media/disk/seplugins/MKIJIRO_POPS.prx
rm *.prx
