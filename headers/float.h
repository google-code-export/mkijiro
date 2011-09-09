/*
 * PSPLINK
 * -----------------------------------------------------------------------
 * Licensed under the BSD license, see LICENSE in PSPLINK root for details.
 *
 * util.c - util functions for psplink
 *
 * Copyright (c) 2005 James F <tyranid@gmail.com>
 * Copyright (c) 2005 Julian T <lovely@crm114.net>
 *
 * $HeadURL: svn://svn.ps2dev.org/psp/trunk/psplink/psplink/util.c $
 * $Id: util.c 1981 2006-07-24 21:51:41Z tyranid $
 */
 
#define MODE_GENERIC 0
#define MODE_EXP 1
#define MODE_FLOAT_ONLY 2

/*static int is_nan(unsigned int val)
{
  unsigned int conv;
  int sign;
  int exp;
  int mantissa;

  conv = *((unsigned int *) val);
  sign = (conv >> 31) & 1;

  exp = (conv >> 23) & 0xff;
  mantissa = conv & 0x7fffff;

  if(exp == 255)
  {
    return 1;
  }
  return 0;
}*/

static int is_inf(unsigned int val)
{
  unsigned int conv;
  int sign;
  int exp;
  int mantissa;

  conv = *((unsigned int *) val);
  sign = (conv >> 31) & 1;

  exp = (conv >> 23) & 0xff;
  mantissa = conv & 0x7fffff;

  if((exp == 255) && (mantissa == 0))
  {
    if(sign)
    {
      return -1;
    }
    else
    {
      return 1;
    }
  }
  if(exp == 255)
  {
    return 2;
  }
  if(exp > 180)
  {
    return 3;
  }

  return 0;
}

static char get_num(float *val, int *exp)
{
  int digit;
  float tmp;
  char ret = '0';

  if((*exp)++ < 16)
  {
    digit = (int) *val;
    if((digit >= 0) && (digit < 10))
    {
      ret = digit + '0';
      tmp = (float) digit;
      *val = (*val - digit)*10.0f;
    }
  }

  return ret;
}

void f_cvt(unsigned int addr, char *buf, int bufsize, int precision, int mode)
{
  /*if(addr % 4) 
  {
    strncpy(buf, "ERR", bufsize);
    buf[bufsize-1] = 0;
    return;
  }*/
  addr&=0xFFFFFFFC;
  char conv_buf[128];
  char *conv_p = conv_buf;
  float normval;
  int digits = 0;//Œ…”
  int exp = 0;//Žw”
  int exp_pos = 0;//Žw”‚ÌˆÊ’u
  int inf=0;//–³ŒÀ‘å
  int sign = 0;//•„†
  float round;
  int rndpos = 0;
  float val=*((float*)(addr));

  /* check for nan and +/- infinity */  
  inf = is_inf(addr);
  if(inf != 0)
  {
	if(inf < 0)
    {
     strncpy(buf,"-INF", bufsize);
    }
    if(inf==2)
    {
    strncpy(buf, "NAN", bufsize);
    }
    if(inf==3)
	{
    strncpy(buf, "HUGE", bufsize);
	}
    if(inf==1)
    {
    strncpy(buf,"INF", bufsize);
    }
    buf[bufsize-1] = 0;
    return;
  }

  if(val < 0.0f)
  {
    sign = 1;
    normval -= val;
  }
  else
  {
    sign = 0;
    normval = val;
  }

  if(precision < 0)
  {
    precision = 6;
  }

  /* normalise value */
  if(normval > 0.0f)
  {
    while((normval >= 1e8f) && (digits++ < 100))
    {
      normval *= 1e-8f;//0.00000001”{
      exp += 8;
    }
    while((normval >= 10.0f) && (digits++ < 100))
    {
      normval *= 0.1f;//0.1”{
      exp++;
    }
    while((normval < 1e-8f) && (digits++ < 100))
    {
      normval *= 1e8f;//1–œ”{
      exp -= 8;
    }
    while((normval < 1.0f) && (digits++ < 100))
    {
      normval *= 10.0f;//10”{
      exp--;
    }
  }

  for(rndpos = precision, round = 0.5f; rndpos > 0; rndpos--, round *= 0.1f);

  if(sign)
  {
    *conv_p++ = '-';
  }

  /*if((mode != MODE_EXP) && (mode != MODE_FLOAT_ONLY))
  {
    if((exp < -4) || (exp > precision))
    {
      mode = MODE_EXP;
    }
  }*/

  normval += round;

  /*if(mode == MODE_EXP)
  {
    *conv_p++ = get_num(&normval, &exp_pos);
    *conv_p++ = '.';

    while(precision > 0)
    {
      *conv_p++ = get_num(&normval, &exp_pos);
      precision--;
    }

    while((conv_p[-1] == '0') || (conv_p[-1] == '.'))
    {
      conv_p--;
    }

    *conv_p++ = 'e';
    if(exp < 0)
    {
      exp = -exp;
      *conv_p++ = '-';
    }
    else
    {
      *conv_p++ = '+';
    }

    if(exp / 100)
    {
      *conv_p++ = (exp / 100) + '0';
      exp %= 100;
    }

    if(exp / 10)
    {
      *conv_p++ = (exp / 10) + '0';
      exp %= 10;
    }
    *conv_p++ = exp + '0';
  }*/
  //else
  //{
    if(exp >= 0)
    {
      for(; (exp >= 0); exp--)
      {
        *conv_p++ = get_num(&normval, &exp_pos);
      }
    }
    else
    {
      *conv_p++ = '0';
    }

    exp++;
    if(precision > 0)
    {
      *conv_p++ = '.';
    }
    
    
    float  x=0.0f;
    float  y=1.0f;
    char   zero=0;
    for( ; (exp < 0) && (precision > 0); exp++, precision--)
    {
    
      y/=10.0f;
      x= 0.999999f*y;
		if(val > x && zero==0){
        *conv_p++ = '1';
        zero=1;
    	}
        else{
        *conv_p++ = '0';
        }
    }
	//int value = *(u32 *)(addr);
	//int mask = value&0x7FFFFF;
    while(precision > 0)
    {
    	if(zero){
      *conv_p++ = 0x30;
    	}
    	else{
      *conv_p++ = get_num(&normval, &exp_pos);
      }
      precision--;
    }
  //}

  *conv_p = 0;

  strncpy(buf, conv_buf, bufsize);
  buf[bufsize-1] = 0;

  return;
}
