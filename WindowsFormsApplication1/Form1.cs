using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Cfg;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

                private void button4_Click(object sender, EventArgs e)
        {
           RegistrationForm reg_form = new RegistrationForm();
           reg_form.ShowDialog(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.User).Assembly);
            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();
            var login = textBox1.Text;
            IQuery q = sess.CreateQuery("FROM User u where u.Login=:login").SetParameter("login",login);
            var list = q.List<Domain.User>();
            if (list.Count > 0 && list[0].Pass == textBox2.Text)
            {
                var role_id = list[0].Role_id;
                IQuery q_role = sess.CreateQuery("FROM Role u where u.Id=:role_id").SetParameter("role_id", role_id);
                var list_role = q_role.List<Domain.Role>();
                if (list_role[0].Name.Equals("reader"))
                {
                    LibraryForm lib_form = new LibraryForm(list[0]);
                    lib_form.ShowDialog();
                }
                else 
                {
                    AdminForm admin_form = new AdminForm();
                    admin_form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            
        }

        
    }
}
