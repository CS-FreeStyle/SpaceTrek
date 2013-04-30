/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package spacetrek;

import java.util.Timer;

/**
 *
 * @author aqsath
 */
public class Handler {

    public static void runHandler()
    {
        //new Timer().schedule(new StationChecker(), 0, 1000 * 60 * 15);
        new Timer().schedule(new SatelliteChecker(), 0, 1000 * 60 * 15);
    }
}
