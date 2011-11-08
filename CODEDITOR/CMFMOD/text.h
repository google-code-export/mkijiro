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

#ifndef _TEXT_H
#define _TEXT_H
#define MAX_TEXTROW_COUNT 7000
typedef struct {
	const char * start;
	int count;
} t_textrow, * p_textrow;

typedef struct {
	int size;
	char * buf;
	int row_count;
	t_textrow rows[MAX_TEXTROW_COUNT];
} t_text, * p_text;

typedef struct {
p_text txt;
}t_txtpack, *p_txtpack;

extern int text_open(const char * filename, int rowbytes, p_txtpack txtpack);
extern int text_rows(p_txtpack txtpack);
extern p_textrow text_read(int row, p_txtpack txtpack);
extern void text_close(p_txtpack txtpack);

#endif
