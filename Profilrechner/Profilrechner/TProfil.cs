﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Profilrechner
{
    class TProfil
    {

        private double breiteUndHoehe;
        private double laenge;
        private double wandstaerke;
        private string profilmaterial;

        CatiaConnection cc = new CatiaConnection();

        public TProfil()
        {
            breiteUndHoehe = 0;
            wandstaerke = 0;
        }


        public void setBreiteUndHoehe(string zahl, string einheit)
        {
            breiteUndHoehe = eingabeMitEinheit.eingabeMitPruefung(zahl, einheit);
        }
        public void setWandstaerke(string zahl, string einheit)
        {
            wandstaerke = eingabeMitEinheit.eingabeMitPruefung(zahl, einheit);
        }
        public void setLaenge(string zahl, string einheit)
        {
            laenge = eingabeMitEinheit.eingabeMitPruefung(zahl, einheit);
        }
        public void setMaterial(string gewaeltesMaterial)
        {
            profilmaterial = gewaeltesMaterial;
        }

        public double getBreiteUndHoehe()
        {
            return breiteUndHoehe;
        }
        public double getWandstaerke()
        {
            return wandstaerke;
        }
        public double getLaenge()
        {
            return laenge;
        }
        public double getQflaeche()
        {
            return wandstaerke * (2 * breiteUndHoehe - wandstaerke);
        }
        public double getSchwerpunkt()
        {
            return wandstaerke * 0.5 * (breiteUndHoehe * wandstaerke + breiteUndHoehe * breiteUndHoehe - wandstaerke * wandstaerke) / getQflaeche();
        }
        public double getFlaechentraegheitsmomentX()
        {
            return breiteUndHoehe * wandstaerke * (wandstaerke * wandstaerke / 12 + Math.Pow(getSchwerpunkt() - wandstaerke / 2, 2)) + wandstaerke * (breiteUndHoehe - wandstaerke) * (Math.Pow(breiteUndHoehe - wandstaerke, 2) / 12 + Math.Pow((breiteUndHoehe + wandstaerke) / 2 - getSchwerpunkt(), 2));
        }
        public double getFlaechentraegheitsmomentY()
        {
            return (wandstaerke * Math.Pow(breiteUndHoehe, 3) + (breiteUndHoehe - wandstaerke) * Math.Pow(wandstaerke, 3)) / 12;
        }
        public double getVolumen()
        {
            return getQflaeche() * laenge;
        }
        public double getMasse()
        {
            return getVolumen() * Material.dichte(profilmaterial);
        }
        public double getPreis()
        {
            return getMasse() * Material.preis(profilmaterial);
        }

        public bool erzeugeCAD(bool? radienErzeugen)
        {
            try
            {
                
                //Finde Catia Prozess
                if (cc.CATIALaeuft() && breiteUndHoehe > 0 && wandstaerke > 0 && breiteUndHoehe > wandstaerke)
                {
                    //Öffne ein neues Part
                    cc.ErzeugePart();

                    // Erstelle eine Skizze
                    cc.ErstelleLeereSkizze();

                    // Generiere eine skizze vom rechteckprofil
                    cc.ErzeugeTProfilSkizze(breiteUndHoehe,wandstaerke,getSchwerpunkt(),radienErzeugen);

                    if (laenge > 0)
                    {
                        // Extrudiere Balken
                        cc.ErzeugeVolumenAusSkizze(laenge);
                        if(radienErzeugen == true)
                        {
                            cc.Screenshot("TProfil_" + Convert.ToString(breiteUndHoehe) + "mm_x_" + Convert.ToString(wandstaerke) + "mm_x_" + Convert.ToString(laenge) + "mm_Radius" + Convert.ToString(wandstaerke) + "mm");
                        }
                        else
                        {
                            cc.Screenshot("TProfil_" + Convert.ToString(breiteUndHoehe) + "mm_x_" + Convert.ToString(wandstaerke) + "mm_x_" + Convert.ToString(laenge) + "mm");
                        }
                        return true;
                    }
                    return false;
                }
                else if (cc.CATIALaeuft())
                {
                    //erstmal nix
                    return false;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Keine laufende Catia Application. " + Environment.NewLine + "Catia starten?", "Fehler",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Process catia = new Process();
                            try
                            {
                                catia.StartInfo.FileName = "C:\\Program Files\\Dassault Systemes\\B28\\win_b64\\code\\bin\\CATSTART.exe";
                                catia.Start();
                                Thread.Sleep(1000);
                                catia.Kill();
                                while (cc.CATIALaeuft() == false)
                                {
                                    Thread.Sleep(1000);
                                }
                                erzeugeCAD(radienErzeugen);
                                return true;
                            }
                            catch
                            {
                                break;
                            }

                        case MessageBoxResult.No:
                            break;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folgender Fehler ist aufgetreten:" + Environment.NewLine + ex, "Fehler",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Information);
                return false;
            }

        }

        public void speichern(bool? radienErzeugen)
        {
            if (radienErzeugen == true)
            {
                cc.Speichern("TProfil_" + Convert.ToString(breiteUndHoehe) + "mm_x_" + Convert.ToString(wandstaerke) + "mm_x_" + Convert.ToString(laenge) + "mm_Radius" + Convert.ToString(wandstaerke) + "mm");
            }
            else
            {
                cc.Speichern("TProfil_" + Convert.ToString(breiteUndHoehe) + "mm_x_" + Convert.ToString(wandstaerke) + "mm_x_" + Convert.ToString(laenge) + "mm");
            }

        }

    }
}
