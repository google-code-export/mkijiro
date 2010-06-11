#!/bin/bash
make clean || { echo "Clean source as root"; exit 1; }
make || { echo "Error while building source"; exit 1; }
psp-packer nitePR.prx || { echo "Error while packing"; exit 1; }
mv nitePR.prx ../OTHER/ || { mkdir ../OTHER && move nitePR.prx ../OTHER/nitePR.prx; exit 1; }

make clean || { echo "Clean source as root"; exit 1; }
make || { echo "Error while building source"; exit 1; }
psp-fixup-imports -m patches/map.txt nitePR.prx  || { echo "Error while patching, perhaps you've packed the prx before patching it?"; exit 1; }
psp-packer nitePR.prx || { echo "Error while packing"; exit 1; }
mv nitePR.prx ../371/ || { mkdir ../371 && move nitePR.prx ../371/nitePR.prx; exit 1; }
