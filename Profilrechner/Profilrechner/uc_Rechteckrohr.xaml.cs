﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Profilrechner
{
    /// <summary>
    /// Interaktionslogik für uc_Rechteckrohr.xaml
    /// </summary>
    public partial class uc_Rechteckrohr : UserControl
    {
        public uc_Rechteckrohr()
        {
            InitializeComponent();
        }



        private void fehlerprüfungMitFarbe(double pruefzahl, TextBox eingabebox)
        {
            if (pruefzahl == 0)
            {
                if (eingabeMitEinheit.Fehlerpruefung(eingabebox.Text))
                {
                    FocusManager.SetFocusedElement(this, eingabebox);
                    eingabebox.SelectAll();
                    eingabebox.Background = Brushes.IndianRed;
                }

            }
            else
            {
                eingabebox.Background = Brushes.Transparent;
            }
        }

        private void berechnen()
        {
            int ausgabe = 0; // ausgabe erfolgt nur wenn ausgabe 0 bleibt

            Rechteckrohr meinRechteckrohr = new Rechteckrohr();

            meinRechteckrohr.setHoehe(tb_Hoehe.Text, cb_einheitHoehe.Text);
            meinRechteckrohr.setBreite(tb_Breite.Text, cb_einheitBreite.Text);
            meinRechteckrohr.setLaenge(tb_Laenge.Text, cb_einheitLaenge.Text);
            meinRechteckrohr.setWandstaerke(tb_Wandstärke.Text, cb_einheitWandstärke.Text);
            meinRechteckrohr.setMaterial(cb_Material.Text);

            if (meinRechteckrohr.getQflaeche() == 0)
            {
                meinRechteckrohr.berechneUnbekannte(tb_flaechentraegheitsmomentX.Text, tb_flaechentraegheitsmomentY.Text);
                if (meinRechteckrohr.getQflaeche() > 0)
                {
                    tb_Breite.Text = Convert.ToString(Math.Round(eingabeMitEinheit.Einheitenrueckrechner(meinRechteckrohr.getBreite(), cb_einheitBreite.Text), 3));
                    tb_Hoehe.Text = Convert.ToString(Math.Round(eingabeMitEinheit.Einheitenrueckrechner(meinRechteckrohr.getHoehe(), cb_einheitHoehe.Text), 3));
                    tb_Wandstärke.Text = Convert.ToString(Math.Round(eingabeMitEinheit.Einheitenrueckrechner(meinRechteckrohr.getWandstärke(), cb_einheitWandstärke.Text), 3));

                }
                else
                {
                    fehlerprüfungMitFarbe(eingabeMitEinheit.eingabeMitPruefung(tb_flaechentraegheitsmomentX.Text, "mm"), tb_flaechentraegheitsmomentX);
                    fehlerprüfungMitFarbe(eingabeMitEinheit.eingabeMitPruefung(tb_flaechentraegheitsmomentY.Text, "mm"), tb_flaechentraegheitsmomentY);
                    ausgabe = 1;
                }

            }

            fehlerprüfungMitFarbe(meinRechteckrohr.getLaenge(), tb_Laenge);
            fehlerprüfungMitFarbe(meinRechteckrohr.getBreite(), tb_Breite);
            fehlerprüfungMitFarbe(meinRechteckrohr.getHoehe(), tb_Hoehe);
            fehlerprüfungMitFarbe(meinRechteckrohr.getWandstärke(), tb_Wandstärke);

            if (ausgabe == 0)
            {
                tb_flaechentraegheitsmomentX.Background = Brushes.Transparent;
                tb_flaechentraegheitsmomentY.Background = Brushes.Transparent;

                lbl_qflaeche.Content = Math.Round(meinRechteckrohr.getQflaeche(), 3) + " mm²";
                tb_flaechentraegheitsmomentX.Text = Math.Round(meinRechteckrohr.getFlaechenträgheitsmomentX(), 3) + " mm⁴";
                tb_flaechentraegheitsmomentY.Text = Math.Round(meinRechteckrohr.getFlaechenträgheitsmomentY(), 3) + " mm⁴";

                if (meinRechteckrohr.getVolumen() > 0)
                {
                    lbl_volumen.Content = Math.Round(meinRechteckrohr.getVolumen(), 3) + " mm³";
                    lbl_masse.Content = Math.Round(meinRechteckrohr.getMasse(), 3) + " kg";
                    lbl_preis.Content = Math.Round(meinRechteckrohr.getPreis(), 3) + " €";
                }

            }


            if (meinRechteckrohr.getWandstärke() > meinRechteckrohr.getBreite())
            {
                tb_Breite.Background = Brushes.IndianRed;
                tb_Wandstärke.Background = Brushes.IndianRed;
                ausgabe = 1;
            }

            if (meinRechteckrohr.getWandstärke() > meinRechteckrohr.getHoehe())
            {
                tb_Hoehe.Background = Brushes.IndianRed;
                tb_Wandstärke.Background = Brushes.IndianRed;
                ausgabe = 1;
            }

        }

        private void aendernTextBox(object sender, KeyEventArgs e)
        {
            berechnen();
        }

        private void aendernComboBox(object sender, EventArgs e)
        {
            berechnen();
        }
    }
}
