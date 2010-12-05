TARGET = MKIJIRO
OBJS = objects/crt0_prx.o objects/exports.o objects/module.o objects/pspdebugkb.o objects/snprintf.o objects/usb.o objects/psid.o objects/blit.o
#objects/server.o

# Define to build this as a prx (instead of a static elf)
BUILD_PRX=1
# Define the name of our custom exports (minus the .exp extension)
PRX_EXPORTS=objects/exports.exp

USE_KERNEL_LIBS = 1
USE_KERNEL_LIBC = 1

INCDIR = 

# popstation build flags
# CFLAGS = -O2 -G0 -w -msingle-float -g -D_POPSMODE_

# umd build flags
CFLAGS = -O2 -G0 -w -msingle-float -g -D_UMDMODE_ -D_JOKER_ -D_PSID_ -D_FONT_misaki -D_IJIRO150_

#-D_FONT_debug
#-D_FONT_acorn //MKULTRA Defalut font
#-D_SERVER_ //unfinished socket mode for pc debugging
#-D_FONT_acorn //font
#-D_USB_ //usb mode
#-D_POPSMODE_  //pops mode cannot be enabled with umd mode (not working atm)
#-D_UMDMODE_ //umd mode cannot be enabled with pops mode
#-D_SOCOM_  //socom shit
#-D_PSID_  //psid corruptor shit
#-D_SCREENSHOT_ //works fine
#-D_JUNK_ //remove shit from cheats

CXXFLAGS = $(CFLAGS) -fno-exceptions -fno-rtti
ASFLAGS = $(CFLAGS)

LIBDIR =

# popstation
# LIBS = -lpspchnnlsv -lpsputility -lpspdebug -lpspge_driver -lpspwlan

# umd
LIBS =  -lpspchnnlsv -lpsputility -lpspdebug -lpspge_driver -lpspwlan -lpsppower -lpspumd -lpspusb -lpspusbstor 
#-lpspsdk -lpspnet -lpspnet_apctl -lpspnet_inet pc debugging shit leave it alone
#look maw no nand driver!! wowies! (what a nice guy huh? yeah who gives a fuck what you think n00b stfu get b00ted)

LDFLAGS = -nostdlib  -nodefaultlibs -g

PSPSDK=$(shell psp-config --pspsdk-path)
include $(PSPSDK)/lib/build.mak

