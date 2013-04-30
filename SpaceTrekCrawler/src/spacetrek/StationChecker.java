/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package spacetrek;

import common.Network;
import java.util.TimerTask;
import org.json.JSONObject;

/**
 *
 * @author aqsath
 */
public class StationChecker extends TimerTask{

    @Override
    public void run()
    {
        String tURL = "http://api.open-notify.org/iss-now/v1/";
        
        try
        {
            String tOutput = Network.getSingleton().sendGet(tURL, null);
            JSONObject tObjectOutput = new JSONObject(tOutput);
            long tTimestamp = tObjectOutput.getLong("timestamp");
            JSONObject tObjectISSPosition = tObjectOutput.getJSONObject("iss_position");
            double tLatitude = tObjectISSPosition.getDouble("latitude");
            double tLongitude = tObjectISSPosition.getDouble("longitude");
            
            System.out.println(tTimestamp + ": " + tLatitude + ", " + tLongitude);
        }
        catch(Exception ex)
        {
            ex.printStackTrace();
        }
    }
}
