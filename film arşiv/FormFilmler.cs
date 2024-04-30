using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace film_arşiv
{
    public partial class FormFilmler : Form
    {
        string baglanti = "Server=localhost;Database=film_arsiv;Uid=root;Pwd='';";
        public FormFilmler()
        {
            InitializeComponent();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgwFilmler.SelectedRows[0];

            int satirId = Convert.ToInt32(dr.Cells[0].Value);

            DialogResult cevap = MessageBox.Show("Filmi silmek istediğinizden emin misiniz?",
                                                 "Filmi sil", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Error);

            if (cevap == DialogResult.Yes)
            {

                using (MySqlConnection baglan = new MySqlConnection(baglanti))
                {
                    int film_id = Convert.ToInt32(dgwFilmler.SelectedRows[0].Cells["film_id"].Value);
                    baglan.Open();
                    string sorgu = "DELETE FROM filmler WHERE film_id=@film_id;";
                    MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                    cmd.Parameters.AddWithValue("@film_id", film_id);
                    cmd.ExecuteNonQuery();

                    DgwDoldur();
                }
            }
        }
        void DgwDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT * FROM filmler;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dgwFilmler.DataSource = dt;

            }
        }

        private void FormFilmler_Load(object sender, EventArgs e)
        {
            DgwDoldur();
            CmbDoldur();
        }
        void CmbDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT DISTINCT tur FROM filmler;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                cmbTur.DataSource = dt;

                cmbTur.DisplayMember = "tur";
                cmbTur.ValueMember = "tur";

            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "UPDATE filmler SET film_ad=@film_ad, yonetmen = @yonetmen, yil = @yil, tur=@tur, sure= @sure, poster = @poster, imdb_puan = @imdb_puan, film_odul = @film_odul WHERE film_id = @film_id";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@film_ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@yonetmen", txtYonetmen.Text);
                cmd.Parameters.AddWithValue("@yil", txtTarih.Text);
                cmd.Parameters.AddWithValue("@tur", cmbTur.SelectedValue);
                cmd.Parameters.AddWithValue("@sure", txtSure.Text);
                cmd.Parameters.AddWithValue("@poster", txtPoster.Text);
                cmd.Parameters.AddWithValue("@imdb_puan", txtPuan.Text);
                cmd.Parameters.AddWithValue("@film_odul", cbOdul.Checked);

                int film_id = Convert.ToInt32(dgwFilmler.SelectedRows[0].Cells["film_id"].Value);
                cmd.Parameters.AddWithValue("@film_id", film_id);

                cmd.ExecuteNonQuery();

                DgwDoldur();

            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string sorgu = " SELECT * FROM filmler WHERE film_ad LIKE @aranan;";


            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@aranan", "%" + txtAra.Text + "%");
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgwFilmler.DataSource = dt;
            }
        }

        private void dgwFilmler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwFilmler.SelectedRows.Count > 0)
            {
                txtAd.Text = dgwFilmler.SelectedRows[0].Cells["film_ad"].Value.ToString();
                txtYonetmen.Text = dgwFilmler.SelectedRows[0].Cells["yonetmen"].Value.ToString();
                txtTarih.Text = dgwFilmler.SelectedRows[0].Cells["yil"].Value.ToString();
                cmbTur.SelectedValue = dgwFilmler.SelectedRows[0].Cells["tur"].Value.ToString();
                txtSure.Text = dgwFilmler.SelectedRows[0].Cells["sure"].Value.ToString();
                txtPoster.Text = dgwFilmler.SelectedRows[0].Cells["poster"].Value.ToString();
                txtPuan.Text = dgwFilmler.SelectedRows[0].Cells["imdb_puan"].Value.ToString();             
                cbOdul.Checked = Convert.ToBoolean(dgwFilmler.SelectedRows[0].Cells["film_odul"].Value);


            }
        }
    }
}
