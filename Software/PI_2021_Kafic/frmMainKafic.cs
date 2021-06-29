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
    public partial class frmMainKafic : Form
    {
        List<UCStol> listaStolova;
        Kafic kafic;
        private Moderator moderator;
        private Konobar konobar;

        public frmMainKafic()
        {
            InitializeComponent();
            listaStolova = new List<UCStol>();
            kafic = DohvatiKafic(1);
            this.Text = kafic.Naziv;
            RefreshStolovi();
            upravaljnjeStolovimaToolStripMenuItem.Visible = false;
            odjaviModeratoraToolStripMenuItem.Visible = false;
            odjaviKonobaraToolStripMenuItem.Visible = false;
            trenutniKorisnikToolStripMenuItem.Visible = false;
            

        }

        private Kafic DohvatiKafic(int id)
        {
            using (var context = new Entities())
            {
                var query = from k in context.Kafic
                            where k.ID_Kafic == id
                            select k;
                return query.Single();
            }
        }

        private void RefreshStolovi()
        {
            IzbrisiStolove();
            int point1 = 0;
            int point2 = 27;
            List<Stol> lista = new List<Stol>();
            using (var context = new Entities())
            {
                
                var query = from s in context.Stol
                            where s.Kafic_ID == kafic.ID_Kafic
                            select s;
                lista = query.ToList();
                foreach(Stol stol in lista)
               
                {
                    UCStol uCStol = new UCStol(kafic);
                    if (point1 + 193 < this.Size.Width)
                    {
                        uCStol.Location = new Point(point1, point2);
                        point1 += 193;

                    }
                    else
                    {
                        point1 = 0;
                        point2 += 211;
                        uCStol.Location = new Point(point1, point2);
                    }
                    
                    uCStol.ImeStola = stol.Oznaka_Stola;
                    
                    this.Controls.Add(uCStol);
                    
                }
            }
            
        }

        private void IzbrisiStolove()
        {
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(UCStol))
                {
                    this.Controls.Remove(control);
                }

            }
        }

        private void MainKafic_Load(object sender, EventArgs e)
        {
            PopuniStolove();
        }

        private void PromijeniImeStolova()
        {
            int brojStolova = 1;
            for (int i = listaStolova.Count-1; i >=0 ; i--)
            {
                listaStolova[i].ImeStola = "Stol" + brojStolova;
                brojStolova++;
            }
        }

        private void PopuniStolove()
        {
            listaStolova.Clear();
            foreach(Control control in this.Controls)
            {
                if (control.GetType() == typeof(UCStol))
                {
                    listaStolova.Add(control as UCStol);
                }

            }
        }

        private void upravaljnjeStolovimaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Upravljanje_Stolovima upravitelj = new Upravljanje_Stolovima(kafic);
            upravitelj.ShowDialog();
            RefreshStolovi();
        }

       

        private void MainKafic_ResizeEnd(object sender, EventArgs e)
        {
            RefreshStolovi();
        }

        private void upravljanjeNamirnicamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNamirniceMain frmNamirnice = new frmNamirniceMain(kafic);
            frmNamirnice.ShowDialog();
        }

       

        private void upravljanjeNormativimaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNormativMain frmNormativ = new frmNormativMain(kafic);
            frmNormativ.ShowDialog();
        }
        private void moderatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoginModerator frmLogin = new frmLoginModerator(kafic);
            frmLogin.ShowDialog();
            moderator = frmLogin.DohvatiModeratora();
            frmLogin.Close();
            if (moderator != null)
            {
                upravaljnjeStolovimaToolStripMenuItem.Visible = true;
                odjaviModeratoraToolStripMenuItem.Visible = true;
                moderatorToolStripMenuItem.Visible = false;


            }
        }

        private void odjaviModeratoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moderator = null;
            upravaljnjeStolovimaToolStripMenuItem.Visible = false;
            odjaviModeratoraToolStripMenuItem.Visible = false;
            moderatorToolStripMenuItem.Visible = true;
            MessageBox.Show("Moderator odjavljen!");
        }

        private void loginKonobarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoginKonobar frmLogin = new frmLoginKonobar(kafic);
            frmLogin.ShowDialog();
            if (frmLogin.DialogResult == DialogResult.OK)
            {
                konobar = frmLogin.nadeniKonobar;
                loginKonobarToolStripMenuItem.Visible = false;
                odjaviKonobaraToolStripMenuItem.Visible = true;
                frmLogin.Close();
                trenutniKorisnikToolStripMenuItem.Visible = true;
                trenutniKorisnikToolStripMenuItem.Text = "User: " + konobar.Ime + " " + konobar.Prezime;
                DodijeliStolovimaKonobara(konobar);
            }
            else frmLogin.Close();
        }

        private void DodijeliStolovimaKonobara(Konobar konobar)
        {
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(UCStol))
                {
                    UCStol stol = (UCStol)control;
                    stol.PostaviKonobara(konobar);
                }

            }
        }

        private void odjaviKonobaraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            konobar = null;
            loginKonobarToolStripMenuItem.Visible = true;
            odjaviKonobaraToolStripMenuItem.Visible = false;
            trenutniKorisnikToolStripMenuItem.Visible = false;
            MakniKonobaraSaStolova();
        }

        private void MakniKonobaraSaStolova()
        {
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(UCStol))
                {
                    UCStol stol = (UCStol)control;
                    stol.MakniKonobara();
                }

            }
        }
    }
}
