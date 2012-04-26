using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1.Domain
{
   public class User
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Patronymic { get; set; }
        public virtual int Role_id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Pass { get; set; }    
    }
}
