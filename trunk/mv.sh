#!/bin/bash
rm *.elf
rm objects/*.o
cp MKIJIRO.prx MKIJIRO/MKIJIRO.prx
cp MKIJIRO.prx /media/disk/seplugins/MKIJIRO.prx
cp MKIJIRONOHB.prx MKIJIRO/MKIJIRONOHB.prx
cp MKIJIRONOHB.prx /media/disk/seplugins/MKIJIRONOHB.prx
cp MKIJIRO_SC.prx MKIJIRO/MKIJIRO_SC.prx
cp MKIJIRO_SC.prx /media/disk/seplugins/MKIJIRO_SC.prx
cp MKIJIRO_POPS.prx MKIJIRO/MKIJIRO_POPS.prx
cp MKIJIRO_POPS.prx /media/disk/seplugins/MKIJIRO_POPS.prx
rm *.prx
cd SRC_nitePR_revJ
rm *.elf
rm objects/*.o
cp nitePRmod.prx nitePR/nitePRmod.prx
cp nitePRmodNOHB.prx nitePR/nitePRmodNOHB.prx
cp nitePRmod.prx /media/disk/seplugins/nitePRmod.prx
cp nitePRmodNOHB.prx /media/disk/seplugins/nitePRmodNOHB.prx
rm *.prx
