/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package common;

/**
 *
 * @author aqsath
 */
public class Tanggal {

    public static String convertNamaToAngka(String pNamaBulan)
    {
        String tReturn = "";
        if(pNamaBulan.equalsIgnoreCase("Jan")){tReturn = "01";}
        else if(pNamaBulan.equalsIgnoreCase("Feb")){tReturn = "02";}
        else if(pNamaBulan.equalsIgnoreCase("Mar")){tReturn = "03";}
        else if(pNamaBulan.equalsIgnoreCase("Apr")){tReturn = "04";}
        else if(pNamaBulan.equalsIgnoreCase("May")){tReturn = "05";}
        else if(pNamaBulan.equalsIgnoreCase("Jun")){tReturn = "06";}
        else if(pNamaBulan.equalsIgnoreCase("Jul")){tReturn = "07";}
        else if(pNamaBulan.equalsIgnoreCase("Aug")){tReturn = "08";}
        else if(pNamaBulan.equalsIgnoreCase("Sep")){tReturn = "09";}
        else if(pNamaBulan.equalsIgnoreCase("Oct")){tReturn = "10";}
        else if(pNamaBulan.equalsIgnoreCase("Nov")){tReturn = "11";}
        else if(pNamaBulan.equalsIgnoreCase("Dec")){tReturn = "12";}
        
        return tReturn;
    }
}
