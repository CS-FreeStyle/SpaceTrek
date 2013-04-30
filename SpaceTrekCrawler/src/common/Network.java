/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package common;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.SocketException;
import java.net.URLEncoder;
import java.util.List;
import javax.net.ssl.SSLHandshakeException;
import org.apache.http.Header;
import org.apache.http.HeaderElement;
import org.apache.http.HeaderElementIterator;
import org.apache.http.HttpEntityEnclosingRequest;
import org.apache.http.HttpHost;
import org.apache.http.HttpRequest;
import org.apache.http.HttpResponse;
import org.apache.http.HttpVersion;
import org.apache.http.NoHttpResponseException;
import org.apache.http.auth.AuthScope;
import org.apache.http.auth.AuthState;
import org.apache.http.auth.Credentials;
import org.apache.http.auth.UsernamePasswordCredentials;
import org.apache.http.client.CookieStore;
import org.apache.http.client.HttpClient;
import org.apache.http.client.HttpRequestRetryHandler;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.params.ClientPNames;
import org.apache.http.client.params.CookiePolicy;
import org.apache.http.client.protocol.ClientContext;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.ConnectionKeepAliveStrategy;
import org.apache.http.conn.params.ConnRoutePNames;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.cookie.Cookie;
import org.apache.http.impl.client.BasicCookieStore;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.message.BasicHeaderElementIterator;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpParams;
import org.apache.http.params.HttpProtocolParams;
import org.apache.http.protocol.BasicHttpContext;
import org.apache.http.protocol.ExecutionContext;
import org.apache.http.protocol.HTTP;
import org.apache.http.protocol.HttpContext;
import org.apache.http.util.EntityUtils;

/**
 *
 * @author aqsath
 */
public class Network {
    
    private static HttpClient mHttpClient;
    private static HttpParams mParameter = null;
    private static SchemeRegistry mRegistry;
    
    //Proxy
    private static boolean mProxyEnabled = false;
    private static String mProxyHost;
    private static int mProxyPort;
    private static String mProxyUsername;
    private static String mProxyPassword;
    
    private static Network mInstance = new Network();
    
    public static Network getSingleton()
    {
        return mInstance;
    }
    
    private Network()
    {
        setup();
        mHttpClient = createHttpClient();
    }
    
    public void setHttpClient(HttpClient pHttpClient)
    {
        mHttpClient = pHttpClient;
    }
    
    private final void setup()
    {
        mRegistry = new SchemeRegistry();
        mRegistry.register(new Scheme("http", 80, PlainSocketFactory.getSocketFactory()));
        mRegistry.register(new Scheme("https", 443, SSLSocketFactory.getSocketFactory()));

        HttpParams tParameter = new BasicHttpParams();
        HttpProtocolParams.setVersion(tParameter, HttpVersion.HTTP_1_1);
        HttpProtocolParams.setContentCharset(tParameter, "UTF-8");
        HttpProtocolParams.setUseExpectContinue(tParameter, true);
        HttpProtocolParams.setHttpElementCharset(tParameter, "UTF-8");
        HttpProtocolParams.setUserAgent(tParameter, "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_3) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.65 Safari/537.31");

        mParameter = tParameter;
    }
    
    public void setProxyParameter(String pProxyHost, int pProxyPort, String pProxyUsername, String pProxyPassword)
    {
        mProxyHost = pProxyHost;
        mProxyPort = pProxyPort;
        mProxyUsername = pProxyUsername;
        mProxyPassword = pProxyPassword;
        mProxyEnabled = true;
        mHttpClient = createHttpClient();
    }

    private HttpClient createHttpClient()
    {
        //PoolingClientConnectionManager tManager = new PoolingClientConnectionManager(mRegistry);
        ClientConnectionManager tManager = new ThreadSafeClientConnManager(mRegistry);
        //tManager.setDefaultMaxPerRoute(100);
        //tManager.setMaxTotal(500);
        final DefaultHttpClient tClient = new DefaultHttpClient(tManager, mParameter);
        tClient.getParams().setParameter(ClientPNames.COOKIE_POLICY, CookiePolicy.BROWSER_COMPATIBILITY);
        tClient.getParams().setBooleanParameter("http.protocol.expect-continue", false);

        if(mProxyEnabled)
        {
            setupProxy(tClient);
        }
        
        //Retry Handler
        HttpRequestRetryHandler tHandler = new HttpRequestRetryHandler() {

            @Override
            public boolean retryRequest(IOException ioe, int i, HttpContext hc) {
                if(i >= 5)
                {
                    return false;
                }
                
                if(ioe instanceof NoHttpResponseException)
                {
                    return true;
                }
                
                if(ioe instanceof SSLHandshakeException)
                {
                    return false;
                }
                
                HttpRequest tRequest = (HttpRequest)hc.getAttribute(ExecutionContext.HTTP_REQUEST);
                boolean tIdempotent = !(tRequest instanceof HttpEntityEnclosingRequest);
                if(tIdempotent)
                {
                    return true;
                }
                
                return false;
            }
        };
        tClient.setHttpRequestRetryHandler(tHandler);
        
        //Keep Alive Strategy
        ConnectionKeepAliveStrategy tStrategy = new ConnectionKeepAliveStrategy() {

            @Override
            public long getKeepAliveDuration(HttpResponse hr, HttpContext hc) {
                HeaderElementIterator tIterator = new BasicHeaderElementIterator(hr.headerIterator(HTTP.CONN_KEEP_ALIVE));
                while(tIterator.hasNext())
                {
                    HeaderElement tElement = tIterator.nextElement();
                    String tParam = tElement.getName();
                    String tValue = tElement.getValue();
                    if(tValue != null && tParam.equalsIgnoreCase("timeout"))
                    {
                        try
                        {
                            return Long.parseLong(tValue) * 1000;
                        }
                        catch(NumberFormatException ignore)
                        {
                            
                        }
                    }
                }
                
                HttpHost tTarget = (HttpHost) hc.getAttribute(ExecutionContext.HTTP_TARGET_HOST);
                if("http://integral.esa.int".equalsIgnoreCase(tTarget.getHostName()))
                {
                    return 5 * 1000;
                }
                else
                {
                    return 30 * 1000;
                }
            }
        };
        tClient.setKeepAliveStrategy(tStrategy);

        return tClient;
    }

    private void setupProxy(DefaultHttpClient pClient)
    {
        HttpHost tProxy = new HttpHost(mProxyHost, mProxyPort, "http");
        pClient.getParams().setParameter(ConnRoutePNames.DEFAULT_PROXY, tProxy);
        AuthState tAuthState = new AuthState();
        tAuthState.setAuthScope(new AuthScope(tProxy.getHostName(), tProxy.getPort()));

        AuthScope tScope = tAuthState.getAuthScope();
        Credentials tCredential = new UsernamePasswordCredentials(mProxyUsername, mProxyPassword);
        pClient.getCredentialsProvider().setCredentials(tScope, tCredential);
    }
    
    public String sendGet(String pUrl, List<BasicNameValuePair> pList) throws java.io.IOException
    {
        String tReturn = "";
        
        if(pList != null)
        {
            for(int i = 0; i < pList.size(); i++)
            {
                if(i == 0)
                {
                    pUrl += "?";
                }
                else
                {
                    pUrl += "&";
                }

                pUrl += pList.get(i).getName() + "=" + URLEncoder.encode(pList.get(i).getValue(), "UTF-8");
            }
        }
        
        HttpGet tGet = new HttpGet(pUrl);
        HttpResponse tResponse = mHttpClient.execute(tGet); //throws java.io.IOException
        tReturn = EntityUtils.toString(tResponse.getEntity());
        EntityUtils.consume(tResponse.getEntity());
        
        return tReturn;
    }
    
    public String sendPost(String pUrl, List<BasicNameValuePair> pList) throws java.io.IOException
    {
        String tReturn = "";
        
        HttpPost tPost = new HttpPost(pUrl);
        tPost.setEntity(new UrlEncodedFormEntity(pList, HTTP.UTF_8));
        HttpResponse tResponse = mHttpClient.execute(tPost); //throws java.io.IOException
        tReturn = EntityUtils.toString(tResponse.getEntity());
        EntityUtils.consume(tResponse.getEntity());
        
        return tReturn;
    }
    
    public String getAllSatellite(String pAwal, String pAkhir)
    {
        String tReturn = "";
        
        try
        {
            CookieStore tCookieStore = new BasicCookieStore();
            HttpContext tLocalContext = new BasicHttpContext();
            tLocalContext.setAttribute(ClientContext.COOKIE_STORE, tCookieStore);
            
            String tURLAwalBanget = "http://integral.esa.int/mySpaceCal/";
            HttpGet tGetAwalBanget = new HttpGet(tURLAwalBanget);
            HttpResponse tResponseAwalBanget = mHttpClient.execute(tGetAwalBanget, tLocalContext);
            EntityUtils.consume(tResponseAwalBanget.getEntity());
            
            String mURLAwal = "http://integral.esa.int/mySpaceCal/index/ajax?func=data&start=" + pAwal + "&end=" + pAkhir;
            HttpGet tGetAwal = new HttpGet(mURLAwal);
            HttpResponse tResponseAwal = mHttpClient.execute(tGetAwal, tLocalContext);
            
            //EntityUtils.consume(tResponseAwal.getEntity());
            
            tReturn = EntityUtils.toString(tResponseAwal.getEntity());
            return tReturn;
        }
        catch(Exception ex)
        {
            ex.printStackTrace();
        }
        
        return tReturn;
    }
}
