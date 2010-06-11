MKV10

-----------------------------------------------------------------------
MESSAGE:
-----------------------------------------------------------------------
If you feel the need to rape and pillage the source...
Please dont bother me about helping you, i dont want to help...
...
and keep our credits in the source bitch. Every person who put time 
into this shit deserves their props. (just not in the gui, blow me. lulz) 
BSD lisence ftw!


-----------------------------------------------------------------------
READ ME ON JOKERING!:
-----------------------------------------------------------------------

The cheat system's been updated with new a new cheat jokering system, 
its not 100% quite yet but it works well enough to release.

#JKR FLASH ON
0xFF008000 0x0127E1E4
0x0055FA2C 0x3FACE000

Here is what you need to know to work them:

nitePR is told that this cheat is jokered by using the 0xFF flag taking up the 1st bite of the address, the last 3 bites are the value of your joker
for example:
-------------------------------
square = 8000
-------------------------------
flag   button   joker address
0xFF   008000   0x0127E1E4
-------------------------------


Or if you want to be more fancy, you can combine jokers like:
-------------------------------
square + Rtrig = 8200
-------------------------------
flag   button   joker address
0xFF   008200   0x0127E1E4
-------------------------------


Your new joker command:
-------------------------------
0xFF008200 0x0127E1E4
-------------------------------


To joker any code, add the joker line just under the cheat name like so:
-------------------------------
Name	#JKR FLASH ON <--cheat name
Joker   0xFF008000 0x0127E1E4
Code	0x0055FA2C 0x3FACE000
Code    you could have more code here
-------------------------------


incase the jokers dont like to shut off so here's how we create the
off code:
-------------------------------
No button pressed
-------------------------------
0xFF000000 0x0127E1E4
place "off" code here



-----------------------------------------------------------------------
How to edit ms0:/seplugins/nitePR/color/color0.txt:
-----------------------------------------------------------------------
0xFF0000FF ;color1 1st color (red) gui color 
0xFF000006 ;color1 fade amount subtracted from the color above each line
0xFFCCCCCC ;color2 2nd color (grey) gui line color
0xFF060606 ;color2 fade amount subtracted from the color above each line
0xFFFF0000 ;color3 (blue) on off toggle
0xFF00FF00 ;color4 (green) on off toggle


-----------------------------------------------------------------------
Enjoy cheating! D:
-----------------------------------------------------------------------
                  				/
			                  _/ \_  
			          -- Red  (>_<) (*)
                             / \ / \ |
                            /|  \  |\|
                            
                            SANiK if you ever see this:
                            	Socom Black Hackers FTW. 
                            		CocaCola > DrPepper 
                            			(but Dr Pepper is decent also)
                            			
-----------------------------------------------------------------------
;UCUS-98645.TXT
-----------------------------------------------------------------------

#+--------JOKERS---------+
0xFFFFFFFF 0xFFFFFFFF
;cheat hz must be 15 or higher.

#JKR FLASH ON
0xFF008000 0x0127E1E4
0x0055FA2C 0x3FACE000

#VERTICAL CLIPPING ON
0xFF008000 0x0127E1E4
0x00538B68 0x3F780000 

#TELEPORT UNDER MAP
;doesnt require off code
0xFF008000 0x0127E1E4
0xFFFFFFFF 0x00505858
0x0000001C 0x3F800000

#LOCK CAMERA ON
0xFF008000 0x0127E1E4
0x004B5428 0x00010001


-----------------------------------------------------------------------
ms0:/seplugins/nitePR/color/color0.txt (classic nitePR red)
-----------------------------------------------------------------------

0xFF0000FF
0xFF000006
0xFFCCCCCC
0xFF060606
0xFFFF0000
0xFF00FF00 

-----------------------------------------------------------------------
