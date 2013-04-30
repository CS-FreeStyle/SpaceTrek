/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package spacetrek;

import common.Formula;
import common.Network;
import java.util.ArrayList;
import java.util.List;
import java.util.TimerTask;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONArray;
import org.json.JSONObject;

/**
 *
 * @author aqsath
 */
public class SatelliteChecker extends TimerTask{

    @Override
    public void run()
    {
        String tAsal = "1364662800";
        String tAkhir = "1368291600";
        
        try
        {
            String tOutput = Network.getSingleton().getAllSatellite(tAsal, tAkhir);
            JSONArray tArrayOutput = new JSONArray(tOutput);
            for(int i = 0; i < tArrayOutput.length(); i++)
            {
                JSONObject tObject = tArrayOutput.getJSONObject(i);
                String tID = tObject.getString("id");
                String tStart = tObject.getString("start");
                String tEnd = tObject.getString("end");
                String tTitle = tObject.getString("title");
                String tClassName = tObject.getString("className");
                double tLatitude = 0.0;
                double tLongitude = 0.0;
                
                /*
                System.out.println("ID: " + tID);
                System.out.println("start: " + tStart);
                System.out.println("end: " + tEnd);
                //System.out.println("title: " + tTitle);
                System.out.println("name: " + tClassName.replaceAll("Satellite_", "") + " - " + tTitle);
                System.out.println("=====================");
                */
                
                String tURL = "http://integral.esa.int/mySpaceCal/index/ajax?func=info&id=" + tID;
                try
                {
                    String tOutputSatellite = Network.getSingleton().sendGet(tURL, null);
                    String[] tStringSplit = tOutputSatellite.split("<br>");
                    String tRadiant = tStringSplit[5].replaceAll("Ra: ", "");
                    String tDeclination = tStringSplit[6].replaceAll("Dec: ", "");
                    
                        String[] tSplitWaktuRadiant = tRadiant.split(":");
                        int tJamRadiant = Integer.parseInt(tSplitWaktuRadiant[0]);
                        int tMenitRadiant = Integer.parseInt(tSplitWaktuRadiant[1]);
                        String[] tSplitDetikRadiant = tSplitWaktuRadiant[2].split("\\.");
                        int tDetikRadiant = Integer.parseInt(tSplitDetikRadiant[0]);
                        int tMilidetikRadiant = Integer.parseInt(tSplitDetikRadiant[1]);
                        tLatitude = Formula.getRAFromHourMinute(tJamRadiant, tMenitRadiant, tDetikRadiant, tMilidetikRadiant);
                        
                        String[] tSplitWaktuDeclination = tDeclination.split(":");
                        String tStrJamDeclination = tSplitWaktuDeclination[0];
                        boolean tMinus = false;
                        if(tStrJamDeclination.charAt(0) == '-')
                        {
                            tMinus = true;
                        }
                        
                        int tJamDeclination = Integer.parseInt(tSplitWaktuDeclination[0].substring(1));
                        int tMenitDeclination = Integer.parseInt(tSplitWaktuDeclination[1]);
                        String[] tSplitDetikDeclination = tSplitWaktuDeclination[2].split("\\.");
                        int tDetikDeclination = Integer.parseInt(tSplitDetikDeclination[0]);
                        int tMilidetikDeclination = Integer.parseInt(tSplitDetikDeclination[1]);
                        tLongitude = Formula.convertDeclinationToLongitude(tJamDeclination, tMenitDeclination, tDetikDeclination, tMilidetikDeclination, tMinus);
                }
                catch(Exception ex)
                {
                    ex.printStackTrace();
                }
                
                try
                {
                    String tURLAdd = "http://119.81.24.210/parisvanjava/spacetrek/object/add/";
                    List<BasicNameValuePair> tListParameter = new ArrayList<BasicNameValuePair>();
                    tListParameter.add(new BasicNameValuePair("name", tClassName.replaceAll("Satellite_", "") + " - " + tTitle));
                    tListParameter.add(new BasicNameValuePair("type", "satellite"));
                    tListParameter.add(new BasicNameValuePair("date", tStart));
                    tListParameter.add(new BasicNameValuePair("lat", tLatitude + ""));
                    tListParameter.add(new BasicNameValuePair("lon", tLongitude + ""));
                    
                    String tReturn = Network.getSingleton().sendPost(tURLAdd, tListParameter);
                    System.out.println(tReturn);
                    
                }
                catch(Exception ex)
                {
                    
                }
            }
        }
        catch(Exception ex)
        {
            ex.printStackTrace();
        }
    }
}
