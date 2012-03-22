cd ..
cd SystemControl\libs\libpspkubridge
make
del *.o
cd ..\libpspsystemctrl_user
make
del *.o
cd ..\libpspsystemctrl_kernel
make
del *.o
cd ..
move libpspkubridge\libpspkubridge.a ..\..\libs\libpspkubridge.a
move libpspsystemctrl_user\libpspsystemctrl_user.a ..\..\libs\libpspsystemctrl_user.a
move libpspsystemctrl_kernel\libpspsystemctrl_kernel.a ..\..\libs\libpspsystemctrl_kernel.a