using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Cfg;

namespace WindowsFormsApplication1
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.User).Assembly);

            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();
            var new_user = new Domain.User

            {
                Name = textBox1.Text,
                Surname = textBox2.Text,
                Patronymic = textBox3.Text,
                Role_id = 1,
                Login = textBox4.Text,
                Pass = textBox5.Text
            };
            if (new_user.Name.Length > 0 && new_user.Surname.Length > 0 && new_user.Patronymic.Length > 0 && new_user.Login.Length > 0 && new_user.Pass.Length > 0)
            {
                sess.Save(new_user);
                sess.Flush();
                this.Hide();
            }
            else 
            {
                label6.Text = "Не все поля заполнены!";
            }
            
        }
    }
}
