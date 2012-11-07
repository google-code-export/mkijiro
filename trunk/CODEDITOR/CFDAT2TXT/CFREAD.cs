using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CF
{
    class CODEFEAKReader : StreamReader
    {
        public CODEFEAKReader(Stream stream)
            : base(stream)
        {
        }


        public CODEFEAKReader(string fs,Encoding encode)
            : base(fs,encode)
        {
        }

        public string ReadCF()
        {
            int c;

            c = Read();
            if (c == -1)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();

            do
            {
                char ch = (char)c;
                if (ch == 'ਊ')//インド文字で区切り
                {
                    return sb.ToString();
                }
                else
                {
                    sb.Append(ch);
                }
            } while ((c = Read()) != -1);
            return sb.ToString();
        }
    }
}
