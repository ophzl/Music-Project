using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicProjectWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Music> elementsValidated = new List<Music>();
        List<Music> elementsNotValidated = new List<Music>();

        static async Task<string> GetURI(Uri u)
        {
            var response = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

        static async Task<string> SendURI(Uri u, HttpContent c)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = u,
                    Content = c
                };

                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    response = result.StatusCode.ToString();
                }
            }
            return response;
        }

            public void GetData()
        {
            var t = Task.Run(() => GetURI(new Uri("https://localhost:44368/api/Musics")));
            t.Wait();
            JArray j = JArray.Parse(t.Result);
            DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(j.ToString(), (typeof(DataTable)));
            dataGridView1.DataSource = dataTable;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Visible = true;
        }

        public void SetData()
        {
            var t = Task.Run(() => GetURI(new Uri("https://localhost:44368/api/Musics")));
            t.Wait();
            JArray j = JArray.Parse(t.Result);
            foreach (var elem in j)
            {
                Boolean isValidated = (Boolean)elem["isValidated"];
                if (isValidated)
                {
                    elementsValidated.Add(new Music((int)elem["id"],(string)elem["title"],(DateTime)elem["releaseDate"],(decimal)elem["price"],(string)elem["genre"],(Boolean)elem["isValidated"]));
                }
                else
                {
                    elementsNotValidated.Add(new Music((int)elem["id"], (string)elem["title"], (DateTime)elem["releaseDate"], (decimal)elem["price"], (string)elem["genre"], (Boolean)elem["isValidated"]));
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Hide();
            GetData();
            button3.Visible = true;
            button2.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetData();
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.Columns.Add("Title");
            listView1.Columns.Add("Release Date");
            listView1.Columns.Add("Genre");
            listView1.Columns.Add("Price");
            foreach (var elem in elementsValidated)
            {
                listView1.Items.Add(new ListViewItem(new string[] { elem.Title, elem.ReleaseDate.ToString(), elem.Genre, elem.Price.ToString() }));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Hide();
            SetData();
            listView1.Visible = true;
            button2.Hide();
            button3.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Boolean isValidated = (Boolean)dataGridView1.CurrentRow.Cells[5].Value;
            string elemId = dataGridView1.CurrentRow.Cells[0].Value + "";
            var payload = elementsNotValidated.FirstOrDefault(f => f.Id.ToString() == elemId);
            payload.IsValidated = isValidated;
            HttpContent c = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var t = Task.Run(() => SendURI(new Uri("https://localhost:44368/api/Musics/" + elemId), c));
        }
    }
}
