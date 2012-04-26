using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1.Domain
{
    class Book
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Author_id { get; set; }
        public virtual int Count { get; set; }
        public virtual int Period { get; set; }
    }
}
