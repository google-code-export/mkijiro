/* 
 * Copyright (C) 2006 aeolusc <soarchin@gmail.com>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of
 * the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 */

#ifndef _MEM_H
#define _MEM_H

#define MEM_VALUE(x) (*(unsigned char *)(x))
#define MEM_VALUE_SHORT(x) (*(unsigned short*)(x))
#define MEM_VALUE_LONG(x) (*(unsigned long*)(x))
#define MEM_VALUE_P(x, y) (*(((unsigned char *)(x)) + (y)))
#define MEM_SHORT(x) (unsigned short)MEM_VALUE(x) + (((unsigned short)MEM_VALUE_P(x, 1)) << 8)
#define MEM_LONG(x) (unsigned int)MEM_VALUE(x) + (((unsigned int)MEM_VALUE_P(x, 1)) << 8) + (((unsigned int)MEM_VALUE_P(x, 2)) << 16) + (((unsigned int)MEM_VALUE_P(x, 3)) << 24)
/* 
typedef struct{
	unsigned int addr;
	unsigned int value;
	int type;
	int lock;
	char name[12];
} __attribute__((packed)) t_mem_table, *p_mem_table; 
  */
typedef struct{
	unsigned int addr;
	unsigned int value;
	unsigned char type;
	unsigned char reserve1;
	unsigned short root;
	unsigned char lock;
	unsigned char reserve2;
	unsigned short reserve3;
	char name[32];
} __attribute__((packed)) t_mem_table, *p_mem_table;

//extern unsigned int v_offset;

extern void mem_dump(u32, u32);
extern int mem_search_byte(unsigned int value, unsigned int low, unsigned int high);
extern int mem_search_word(unsigned int value, unsigned int low, unsigned int high);
extern int mem_search_dword(unsigned int value, unsigned int low, unsigned int high);
extern int mem_search_value(unsigned int value, unsigned int low, unsigned int high);
extern int mem_search_fuzzy(int diff, unsigned int low, unsigned int high, int apx, unsigned int compareval, unsigned int fuzzy_diff);
extern int mem_search_get_result(unsigned int ** result);
extern void mem_search_finish();

extern void mem_set_lockspd(int spd);
extern void mem_table_index_init(const char * gname, int spd);

extern void mem_table_save(int idx);
extern int mem_table_load(int idx, int clear);
extern void mem_table_lock();
extern int mem_table_add(p_mem_table item);
extern void mem_table_delete(int index);
extern int mem_get_table(p_mem_table * table);
extern void mem_table_set(int idx, p_mem_table item);

extern void mem_table_clear();
//extern void mem_free();
extern void mem_table_insert(p_mem_table item, int index);
extern void mem_table_lockall();
#define MEMTABLE_INDEXMAX 100
extern char MODULE_DIR[];
extern u32 lastbutton;
extern void mem_table_savecw();
extern void mem_table_locksuit(int, int *, int *, int);
extern int mem_table_walkback(int index);
extern int mem_table_walkforward(int index);
extern void mem_table_enable(int index);
extern void mem_search_init();
//extern void mem_search_null();
extern void filter_filename(char *s);
extern void mem_table_deletesuit(int index);
extern void mem_table_suit(int index, int *start, int *end);
extern void mem_clear_instblock();
#endif
