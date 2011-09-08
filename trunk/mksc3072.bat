make -f sc3072 clean
make -f sc3072
SET BUID=3072
copy MKIJIRO_SC_%BUID%.prx MKIJIRO\MKIJIRO_SC_%BUID%.prx
copy MKIJIRO_SC_%BUID%.prx patches\MKIJIRO_SC_%BUID%.prx
move MKIJIRO_SC_%BUID%.prx g:\seplugins\MKIJIRO_SC_%BUID%.prx
move MKIJIRO_SC_%BUID%.prx f:\seplugins\MKIJIRO_SC_%BUID%.prx
move MKIJIRO_SC_%BUID%.prx e:\seplugins\MKIJIRO_SC_%BUID%.prx