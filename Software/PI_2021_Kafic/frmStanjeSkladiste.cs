﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PI_2021_Kafic
{
    public partial class frmStanjeSkladiste : Form
    {
        private Kafic kafic;
        private Namirnica odabranaNamirnica;
        public frmStanjeSkladiste(Kafic prosljedeniKafic)
        {
            InitializeComponent();
            kafic = prosljedeniKafic;
        }

        private void frmStanjeSkladiste_Load(object sender, EventArgs e)
        {
            nUDStanje.Minimum = 0;
            nUDStanje.Maximum = 100000;
            nUDStanje.DecimalPlaces = 2;
            nUDStanje.Increment = 0.01M ;
            nUDDodaj.Minimum = 0;
            nUDDodaj.Maximum = 100000;
            nUDDodaj.DecimalPlaces = 2;
            
            RefreshSkladiste();
            DohvatiNamirnice();


        }

        private void DohvatiNamirnice()
        {
            using (var context = new Entities())
            {
                var query = from n in context.Namirnica
                            where n.Kafic_ID == kafic.ID_Kafic
                            select n;
                cmbNamirnica.DataSource = query.ToList();
            }
        }

        private void RefreshSkladiste()
        {
            using (var context = new Entities())
            {
                var query = from n in context.Namirnica
                            where n.Kafic_ID == kafic.ID_Kafic
                            select n;
                dgvSkladiste.DataSource = query.ToList();
                dgvSkladiste.Columns["ID_Namirnica"].Visible = false;
                dgvSkladiste.Columns["Skladiste_ID"].Visible = false;
                dgvSkladiste.Columns["Kafic_ID"].Visible = false;
                dgvSkladiste.Columns["Skladiste"].Visible = false;
                dgvSkladiste.Columns["Stavka_normativa"].Visible = false;
                dgvSkladiste.Columns["Naziv_Namirnice"].HeaderText = "Namirnica";
                dgvSkladiste.Columns["Kolicina_na_skladistu"].HeaderText = "Dostupna količina na skladištu";

            }
        }

        private void dgvSkladiste_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSkladiste.CurrentRow.DataBoundItem != null)
            {
                odabranaNamirnica = dgvSkladiste.CurrentRow.DataBoundItem as Namirnica;
            }
            OsvjeziUpdate();
        }

        private void OsvjeziUpdate()
        {
            lblArtikl.Text = odabranaNamirnica.Naziv_Namirnice;
            if(odabranaNamirnica.Kolicina_na_skladistu!=null)
            nUDStanje.Value = (decimal)odabranaNamirnica.Kolicina_na_skladistu;
            
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            using (var context = new Entities())
            {
                context.Namirnica.Attach(odabranaNamirnica);
                odabranaNamirnica.Kolicina_na_skladistu = (double)nUDStanje.Value;
                context.SaveChanges();
            }
            RefreshSkladiste();
        }

        private void btnDodajKolicinu_Click(object sender, EventArgs e)
        {
            try
            {
                if (nUDDodaj.Value == 0) throw new KriviUnosException("Unesite vrijednost veću od 0");
                else
                {
                    Namirnica namirnica = cmbNamirnica.SelectedItem as Namirnica;
                    using (var context = new Entities())
                    {
                        context.Namirnica.Attach(namirnica);
                        namirnica.Kolicina_na_skladistu += (double)nUDDodaj.Value;
                        context.SaveChanges();

                    }
                    RefreshSkladiste();
                }
            }
            catch (KriviUnosException ex)
            {
                MessageBox.Show(ex.DodatnaPoruka);
                throw;
            }
        }

        private void frmStanjeSkladiste_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            PomocneFunkcije.PomocneFunkcije.HelpStanjeSkladista();
        }
    }
}
