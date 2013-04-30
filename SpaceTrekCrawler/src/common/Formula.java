/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package common;

/**
 *
 * @author aqsath
 */
public class Formula {

    public static double getRAFromHourMinute(int pHour, int pMinute)
    {
        int tJumlahMenit = (pHour * 60) + pMinute;
        double tRA = tJumlahMenit * 360.0 / (24.0 * 60.0);
        tRA = tRA + 0.5;
        
        if(tRA > 180)
        {
            tRA = 0 - (tRA - 180);
        }
        
        return tRA;
    }
    
    public static double getRAFromHourMinute(int pHour, int pMinute, int pSecond, int pMilisecond)
    {
        double tJumlahDetik = (pHour * 60 * 60) + (pMinute * 60) + pSecond + (pMilisecond / 100.0);
        double tRA = tJumlahDetik * 360.0 / (24.0 * 60.0 * 60.0);
        
        if(tRA > 180)
        {
            tRA = 0 - (tRA - 180);
        }
        
        return tRA;
    }
    
    public static double convertDeclinationToLongitude(int tJam, int pMinute, int pSecond, int pMilisecond, boolean tMinus)
    {
        double tJumlahDetik = (pMinute * 60) + pSecond + (pMilisecond / 100.0);
        double tDegree = tJumlahDetik / (60.0 * 60.0);
        
        double tLongitude = tJam + tDegree;
        if(tMinus)
        {
            tLongitude = 0 - tLongitude;
        }
        
        return tLongitude;
    }
}
