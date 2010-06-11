#!/bin/bash
# You will want to edit this file to point to the correct mount path.
# /mnt/loop /mnt/sda1 /mnt/hda1 /mnt/sr0 figure it out n00b.

make clean || { echo "Clean source as root"; exit 1; }
make || { echo "Error while building source"; exit 1; }
psp-packer nitePR.prx || { echo "Error while packing"; exit 1; } 
mount /dev/sda1  || { echo "Error while mounting psp"; exit 1; }
mv nitePR.prx /mnt/loop/seplugins/nitePR.prx  || { echo "Directory does not exist, try mounting your psp first."; exit 1; } 
umount /dev/sda1 || { echo "Error while unmounting psp"; exit 1; } 
