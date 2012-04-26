using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Cfg;
using NHibernate;
using WindowsFormsApplication1.Domain;

namespace WindowsFormsApplication1
{
    public partial class LibraryForm : Form
    {
        public User _user;
        public LibraryForm()
        {
            InitializeComponent();
        }
        public LibraryForm(User user)
        {
            this._user = user;
            InitializeComponent();
        }

        private void LibraryForm_Load(object sender, EventArgs e)
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.Book).Assembly);
            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();
            IQuery q = sess.CreateQuery("FROM Book");
            var list = q.List<Domain.Book>();
            IQuery q_order = sess.CreateQuery("FROM Order o where o.User_id=:user_id").SetParameter("user_id",this._user.Id);
            var list_order = q_order.List<Domain.Order>();
            for (int i = 0; i < list_order.Count; i++) 
            {
                IQuery q_book = sess.CreateQuery("FROM Book a where a.Id = :book_id").SetParameter("book_id", list_order[i].Book_id);
                var list_book = q_book.List<Domain.Book>();
                listBox1.Items.Add(list_book[0].Name);
            }

                for (int i = 0; i < list.Count; i++)
                {
                    int a_id = list[i].Author_id;
                    IQuery q2 = sess.CreateQuery("FROM Author a where a.Id = :a_id").SetParameter("a_id", a_id);
                    var list2 = q2.List<Domain.Author>();
                    if (list2.Count > 0)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Text = list[i].Name + " " + list2[0].Surname + " " + list2[0].Name + " " + list2[0].Patronymic;
                        item.Value = list[i].Id;
                        comboBox1.Items.Add(item);

                    }
                }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var book_id = (comboBox1.SelectedItem as ComboBoxItem).Value;
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.Book).Assembly);
            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();
            IQuery q = sess.CreateQuery("FROM Book b where b.Id=:book_id").SetParameter("book_id",book_id);
            var list = q.List<Domain.Book>();
            if (list.Count > 0 && list[0].Count > 0)
            {
                button1.Visible = true;
                label2.Text = "";
            }
            else 
            {
                button1.Visible = false;
                label2.Text = "Нет в наличии данной книги";
            }
               
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._user.Login="2";
            var user_id = this._user.Id;
            var book_id = (comboBox1.SelectedItem as ComboBoxItem).Value;
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.Order).Assembly);
            var sessions = cfg.BuildSessionFactory();
            var sess = sessions.OpenSession();
            IQuery q_order = sess.CreateQuery("FROM Order o where o.User_id=:user_id and o.Book_id=:book_id").SetParameter("user_id", user_id).SetParameter("book_id",book_id);
            var list_order = q_order.List<Domain.Order>();
            IQuery q_book = sess.CreateQuery("FROM Book b where b.Id=:book_id").SetParameter("book_id", book_id);
            var list_book = q_book.List<Domain.Book>();
            if (list_order.Count > 0)
            {
                label2.Text = "Вы уже взяли эту книгу";
            }
            else 
            {
                var order = new Domain.Order
                {
                    Book_id = book_id.ToString(),
                    User_id = user_id.ToString(),
                    Expiration_date = "90"
                };
                sess.Save(order);
                sess.Flush();
                button1.Visible = false;
                IQuery q_book_add = sess.CreateQuery("FROM Book b where b.Id=:book_id").SetParameter("book_id", order.Book_id);
                var book = q_book_add.List<Domain.Book>();
                listBox1.Items.Add(book[0].Name);
            }
        }
    }
}
