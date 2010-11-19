#!/bin/bash
rm *.elf
rm objects/*.o
cp MKIJIRO.prx MKIJIRO/mkultra_psplink.prx
cp MKIJIRO.prx /media/disk/seplugins/mkultra_psplink.prx
cp MKIJIRO_SC.prx MKIJIRO/mkultra.prx
cp MKIJIRO_SC.prx /media/disk/seplugins/mkultra.prx
rm *.prx
