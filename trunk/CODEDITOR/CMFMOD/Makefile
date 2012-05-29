PSP_SLIM = 
BIG5_TXT = 

#release: all 
#	psp-build-exports -k exports.exp 
#	psp-build-exports -s -k -v exports.exp

ifdef PSP_SLIM
PSP_FW_VERSION = 371
else
PSP_FW_VERSION = 150
endif



TARGET = CMFMOD

ifdef PSP_SLIM
OBJS = conf.o main.o ctrl.o encode.o font.o layout.o mem.o\
	screenshot.o text.o ui.o dict.o lang_zh.o allocmem.o convert_cw.o\
	blend.o rgb_color.o minifloat.o usb.o common.o\
	ctrl_driver.o display_driver.o power_driver.o ge_driver.o syscon_driver.o\
	MySUB.o MyIMG.o MyJPG.o SNAPJPG.o SNAPPNG.o FAT.o CMF_STATE.o\
	smsutils.o
else
OBJS = conf.o main.o ctrl.o encode.o font.o layout.o mem.o\
	screenshot.o text.o ui.o dict.o lang_zh.o allocmem.o convert_cw.o\
	blend.o rgb_color.o minifloat.o usb.o common.o\
	MySUB.o MyIMG.o MyJPG.o SNAPJPG.o SNAPPNG.o FAT.o CMF_STATE.o\
	smsutils.o
endif	

BUILD_PRX=1
USE_KERNEL_LIBC=1
USE_KERNEL_LIBS=1

PRX_EXPORTS=exports.exp



ifdef PSP_SLIM
INCDIR = sdk/include
else
INCDIR =
endif


CFLAGS = -Os -G0 -Wall -fno-strict-aliasing -fno-builtin-printf -DENABLE_CLANG
ifdef PSP_SLIM
CFLAGS += -DGAME3XX
else
CFLAGS +=
endif

ifdef ENGLISH_UI
CFLAGS += -DENGLISH_UI
endif

ifdef JAPANESE_UI
CFLAGS += -DJAPANESE_UI
CFFLAG += -DGBK
endif

ifdef BIG5_TXT
CFLAGS += -DBIG5_ENCODE_TEXT
endif

CXXFLAGS = $(CFLAGS) -fno-exceptions -fno-rtti
ASFLAGS = $(CFLAGS)


ifdef PSP_SLIM
LIBDIR = sdk/lib
else
LIBDIR += sdk/lib
endif

LDFLAGS = -nostdlib -nodefaultlibs


ifdef PSP_SLIM
LIBS += -lpspsystemctrl_kernel -lpsprtc_driver
else
LIBS += -lpsppower_driver -lpspsystemctrl_kernel -lpspge_driver -lpsprtc_driver
endif


PSPSDK=$(shell psp-config --pspsdk-path)
include $(PSPSDK)/lib/build.mak
