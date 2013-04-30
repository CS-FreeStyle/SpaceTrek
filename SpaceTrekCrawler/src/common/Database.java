/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package common;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;
import spacetrek.Config;

/**
 *
 * @author aqsath
 */
public class Database {
    
    public static void addTravelObject(String pName, String pDesc, String pCity, String pTempImage, String pLat, String pLng, String pTag)
    {
        Connection tConnection = null;
        Statement tStatement = null;
        
        try
        {
            Class.forName("com.mysql.jdbc.Driver").newInstance();
            String tConnectionString = "jdbc:mysql://" + Config.DATABASE_SERVER + ":" + Config.DATABASE_PORT + "/" + Config.DATABASE_NAME;
            tConnection = DriverManager.getConnection(tConnectionString, Config.DATABASE_USERNAME, Config.DATABASE_PASSWORD);
            tStatement = tConnection.createStatement();
            
            String tQuery = "INSERT INTO travel_object(name, description, city, temp_img, lat, lng) VALUES(?, ?, ?, ?, ?, ?)";
            tStatement = tConnection.prepareStatement(tQuery, Statement.RETURN_GENERATED_KEYS);
            ((PreparedStatement)tStatement).setString(1, pName);
            ((PreparedStatement)tStatement).setString(2, pDesc);
            ((PreparedStatement)tStatement).setString(3, pCity);
            ((PreparedStatement)tStatement).setString(4, pTempImage);
            ((PreparedStatement)tStatement).setFloat(5, Float.parseFloat(pLat));
            ((PreparedStatement)tStatement).setFloat(6, Float.parseFloat(pLng));

            ((PreparedStatement)tStatement).executeUpdate();
            
            if(!pTag.isEmpty())
            {
                ResultSet tRS = ((PreparedStatement)tStatement).getGeneratedKeys();
                if(tRS.next())
                {
                    int tTravelObjectID = tRS.getInt(1);
                    System.out.println(tTravelObjectID);
                    tQuery = "INSERT INTO tag(id_object, keyword) VALUES(?, ?)";
                    tStatement = tConnection.prepareStatement(tQuery, Statement.RETURN_GENERATED_KEYS);
                    ((PreparedStatement)tStatement).setInt(1, tTravelObjectID);
                    ((PreparedStatement)tStatement).setString(2, pTag);

                    ((PreparedStatement)tStatement).executeUpdate();
                }
            }
        }
        catch(Exception ex)
        {
            ex.printStackTrace();
        }
        finally
        {
            if(tConnection != null)
            {
                try
                {
                    tConnection.close();
                }
                catch(Exception ex)
                {
                    ex.printStackTrace();
                }
            }
            
            if(tStatement != null)
            {
                try
                {
                    tStatement.close();
                }
                catch(Exception ex)
                {
                    ex.printStackTrace();
                }
            }
        }
    }
    
    public static Map<Integer, String> getTravelObjectLatLong()
    {
        Map<Integer, String> tMap = new HashMap<Integer, String>();
        Connection tConnection = null;
        Statement tStatement = null;
        
        try
        {
            Class.forName("com.mysql.jdbc.Driver").newInstance();
            String tConnectionString = "jdbc:mysql://" + Config.DATABASE_SERVER + ":" + Config.DATABASE_PORT + "/" + Config.DATABASE_NAME;
            tConnection = DriverManager.getConnection(tConnectionString, Config.DATABASE_USERNAME, Config.DATABASE_PASSWORD);
            tStatement = tConnection.createStatement();
            
            String tQuery = "SELECT * FROM travel_object";
            ResultSet tRS = tStatement.executeQuery(tQuery);
            while(tRS.next())
            {
                int tID = tRS.getInt("id_object");
                String tLatLong = tRS.getString("lat") + "," + tRS.getString("lng");
                
                tMap.put(tID, tLatLong);
            }
        }
        catch(Exception ex)
        {
            ex.printStackTrace();
        }
        finally
        {
            if(tConnection != null)
            {
                try
                {
                    tConnection.close();
                }
                catch(Exception ex)
                {
                    ex.printStackTrace();
                }
            }
            
            if(tStatement != null)
            {
                try
                {
                    tStatement.close();
                }
                catch(Exception ex)
                {
                    ex.printStackTrace();
                }
            }
        }
        
        return tMap;
    }
}
