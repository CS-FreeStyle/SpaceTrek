/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package custom;

import common.Formula;
import common.Network;
import common.Tanggal;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 *
 * @author aqsath
 */
public class MeteorCrawler {
    
    public static void main(String[] args)
    {
        String tURL = "http://www.amsmeteors.org/meteor-showers/meteor-shower-calendar/?y=";
        
        for(int tTahun = 2009; tTahun <= 2023; tTahun++)
        {
            tURL = tURL + tTahun;
            try
            {
                String tOutput1 = Network.getSingleton().sendGet(tURL, null);
                Pattern tPattern1 = Pattern.compile("<div class=\"shower clearfix\">.*?</p>.*?</div>", Pattern.DOTALL);
                Matcher tMatcher1 = tPattern1.matcher(tOutput1);
                while(tMatcher1.find())
                {
                    String tOutput2 = tMatcher1.group();
                    
                    String tName = "";
                    String tTanggalMeteor = "";
                    String tRadiant = "";
                    
                    Pattern tPatternName = Pattern.compile("<h3 id=\".*?\">", Pattern.DOTALL);
                    Matcher tMatcherName = tPatternName.matcher(tOutput2);
                    
                    while(tMatcherName.find())
                    {
                        tName = tMatcherName.group();
                        tName = tName.replaceAll("<h3 id=\"", "");
                        tName = tName.replaceAll("\">", "");
                        tName = tName.replaceAll("\\+", " ");
                    }
                    
                    Pattern tPatterDate = Pattern.compile("Peak night.*?<strong>.*?</strong>", Pattern.DOTALL);
                    Matcher tMatcherDate = tPatterDate.matcher(tOutput2);
                    
                    while(tMatcherDate.find())
                    {
                        String tTempDate = tMatcherDate.group();
                        tTempDate = tTempDate.replaceAll("Peak night", "");
                        tTempDate = tTempDate.replaceAll("<.*?>", "");
                        tTempDate = tTempDate.trim();
                        String[] tDateSplit = tTempDate.split("\\s+");
                        if(tDateSplit.length == 2)
                        {
                            String tBulan = Tanggal.convertNamaToAngka(tDateSplit[0]);
                            String[] tTanggalSplit = tDateSplit[1].split("-");
                            if(tTanggalSplit.length == 2)
                            {
                                tTanggalMeteor = tTahun + "-" + tBulan + "-" + tTanggalSplit[1];
                            }
                            else
                            {
                                tTanggalMeteor = tTahun + "-" + tBulan + "-" + tDateSplit[1];
                            }
                        }
                    }
                    
                    Pattern tPatternRadiant = Pattern.compile("Radiant.*?<strong>", Pattern.DOTALL);
                    Matcher tMatcherRadiant = tPatternRadiant.matcher(tOutput2);
                    
                    int tJam = 0;
                    int tMenit = 0;
                    double tLatitude = 0.0;
                    
                    while(tMatcherRadiant.find())
                    {
                        tRadiant = tMatcherRadiant.group();
                        tRadiant = tRadiant.replaceAll("Radiant</a>:</strong> ", "");
                        tRadiant = tRadiant.replaceAll("&deg; -", "");
                        tRadiant = tRadiant.replaceAll("<strong>", "");
                        tRadiant = tRadiant.trim();
                        
                        String[] tRadiantSplit = tRadiant.split(" ");
                        if(tRadiantSplit.length == 2)
                        {
                            String[] tWaktuSplit = tRadiantSplit[0].split(":");
                            if(tWaktuSplit.length == 2)
                            {
                                tJam = Integer.parseInt(tWaktuSplit[0]);;
                                tMenit = Integer.parseInt(tWaktuSplit[1]);
                            }
                            
                            tLatitude = Double.parseDouble(tRadiantSplit[1]);
                        }
                    }
                    
                    double tLongitude = Formula.getRAFromHourMinute(tJam, tMenit);
                    System.out.println(tName);
                    System.out.println(tTanggalMeteor);
                    System.out.println(tLatitude + ", " + tLongitude);
                    
                    System.out.println("=====");
                }
                
            }
            catch(Exception ex)
            {
                ex.printStackTrace();
            }
        }
    }
}
