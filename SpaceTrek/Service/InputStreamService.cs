using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using SpaceTrek.Helper;
using SpaceTrek.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SpaceTrek.Service
{
    /// <summary>
    /// Helper class to create sequence of image of space object
    /// </summary>
    public class InputStreamService
    {

        #region Property
        private SpaceObject CurrentSpaceObject;
        private SpaceChannel CurrentSpaceChannel;

        private PhotoCamera Camera;

        private Dictionary<string, object> ParametersRef;
        private string BoundaryString = "----ParisVanJava";
        private int SavedCounter = 0;
        public bool IsActive = false;
        
        private string CurrentFlashMode;
        private int CurrentRestIndex;
        private VideoBrush CurrentVideoBrush;
        #endregion

        public event EventHandler<EventArgs> OnChannelCreationCompleted;

        public void InitSpaceChannel(SpaceChannel spaceChannel)
        {
            this.CurrentSpaceChannel = spaceChannel;
        }

        #region Camera Class

        public void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            if (Camera != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    // LandscapeRight rotation when camera is on back of device.
                    int landscapeRightRotation = 180;

                    // Change LandscapeRight rotation for front-facing camera.
                    if (Camera.CameraType == CameraType.FrontFacing) landscapeRightRotation = -180;

                    // Rotate video brush from camera.
                    if (e.Orientation == PageOrientation.LandscapeRight)
                    {
                        // Rotate for LandscapeRight orientation.
                        CurrentVideoBrush.RelativeTransform =
                            new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = landscapeRightRotation };
                    }
                    else
                    {
                        // Rotate for standard landscape orientation.
                        CurrentVideoBrush.RelativeTransform =
                            new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 0 };
                    }
                });
            }

        }

        public void InitCamera(VideoBrush CurrentVideoBrush)
        {
           
            this.Camera = new PhotoCamera(CameraType.Primary);
          
            this.Camera.Initialized += Camera_Initialized;
            this.Camera.CaptureCompleted += Camera_CaptureCompleted;
            this.Camera.CaptureImageAvailable += Camera_CaptureImageAvailable;
            //Set the VideoBrush source to the camera.
            CurrentVideoBrush.SetSource(Camera);
        }


        public void InitCamera(VideoBrush currentVideoBrush,SpaceObject currentSpaceObject)
        {
            this.CurrentSpaceObject = currentSpaceObject;
            this.CurrentSpaceObject.OnChannelCreationCompleted += CurrentSpaceObject_OnChannelCreationCompleted;

            this.Camera = new PhotoCamera(CameraType.Primary);

            this.Camera.Initialized += Camera_Initialized;
            this.Camera.CaptureCompleted += Camera_CaptureCompleted;
            this.Camera.CaptureImageAvailable += Camera_CaptureImageAvailable;
            //Set the VideoBrush source to the camera.
            currentVideoBrush.SetSource(Camera);

            //create channel
            this.CurrentSpaceObject.CreateChannel();
        }

        void CurrentSpaceObject_OnChannelCreationCompleted(object sender, ChannelUserEventArgs e)
        {
            try
            {
                if (e.EventId != 0)
                {
                    CurrentSpaceChannel = new SpaceChannel() { id_channel = e.EventId };
                    if (OnChannelCreationCompleted != null)
                        OnChannelCreationCompleted(sender, e);
                }
            }
            catch { 
            
            }
        }

        void Camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            string fileName = SavedCounter + ".jpg";

            try
            { 
                //TODO : ADD INSERT STREAM TO SERVER
                SaveInputStream(e.ImageStream,SavedCounter);

                Debug.WriteLine("Captured image available, saving picture " + fileName);
            
      
                //e.ImageStream.Seek(0, SeekOrigin.Begin);

         
                //using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
                //{
                //    using (IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create, FileAccess.Write))
                //    {
                //        // Initialize the buffer for 4KB disk pages.
                //        byte[] readBuffer = new byte[4096];
                //        int bytesRead = -1;

                //        // Copy the image to isolated storage. 
                //        while ((bytesRead = e.ImageStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                //        {
                //            targetStream.Write(readBuffer, 0, bytesRead);
                //        }
                //    }
                //}

               
            }
            finally
            {
                // Close image stream
            //    e.ImageStream.Close();
            }

        }

        void Camera_CaptureCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            // Increments the savedCounter variable used for generating JPEG file names.
            SavedCounter++;
            Debug.WriteLine(SavedCounter.ToString());
            if (IsActive)
                Camera.CaptureImage();
        }

        public void DisposeCamera()
        {
            if (Camera != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                Camera.Dispose();

                // Release memory, ensure garbage collection.
                Camera.Initialized -= Camera_Initialized;
                Camera.CaptureCompleted -= Camera_CaptureCompleted;
                Camera.CaptureImageAvailable -= Camera_CaptureImageAvailable;
                //Camera.CaptureThumbnailAvailable -= cam_CaptureThumbnailAvailable;
                //Camera.AutoFocusCompleted -= cam_AutoFocusCompleted;
                //CameraButtons.ShutterKeyHalfPressed -= OnButtonHalfPress;
                //CameraButtons.ShutterKeyPressed -= OnButtonFullPress;
                //CameraButtons.ShutterKeyReleased -= OnButtonRelease;
            }
        }

        void Camera_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            try
            {
                if (e.Succeeded)
                {
                    this.Camera.FlashMode = FlashMode.Off;
                    var resx =  this.Camera.AvailableResolutions;

                    this.Camera.Resolution = resx.ElementAt(0);
                }
                else {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Something Error with your camera");
                    });
                }
            }
            catch (Exception ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {

                        MessageBox.Show("Something Error with your camera");
                    });
            }
        }

        /// <summary>
        /// Start capturing image
        /// </summary>
        public void StartCapture()
        {
            IsActive = true;
            this.Camera.CaptureImage();
        }

        /// <summary>
        /// Stop capturing image
        /// </summary>
        public void StopCapture()
        {
            IsActive = false;   
        }

        #endregion

        #region Data Service

                public void SaveInputStream(string userfile, System.IO.Stream attach_pic)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
           
                    if (!String.IsNullOrEmpty(userfile))
                        parameters.Add("userfile", userfile);
                    if (attach_pic != null)
                        parameters.Add("attach_pic", attach_pic);


                    ParametersRef = parameters;
                    ConstructRequestMultipart(new Uri(EndpointHelper.CHANNEL_UPLOAD, UriKind.Absolute));

                }

                public void SaveInputStream(System.IO.Stream attach_pic,int sequence)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

            
                    if (attach_pic != null)
                        parameters.Add("userfile", attach_pic);
                    parameters.Add("sequence", sequence);

                    ParametersRef = parameters;
                    ConstructRequestMultipart(new Uri(String.Format(EndpointHelper.CHANNEL_UPLOAD,CurrentSpaceChannel.id_channel), UriKind.Absolute));

                }

                public void SaveInputStream(PhotoResult attach_pic,int sequence)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();


                    if (attach_pic != null)
                        parameters.Add("userfile", attach_pic);
                    parameters.Add("sequence", sequence);

                    ParametersRef = parameters;
                    ConstructRequestMultipart(new Uri(String.Format(EndpointHelper.CHANNEL_UPLOAD, CurrentSpaceChannel.id_channel), UriKind.Absolute));

                }


                private void ConstructRequestMultipart(Uri uri)
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
                    myRequest.Method = "POST";
                    myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", BoundaryString);

                    myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
                }

                private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
                {
                    HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                    System.IO.Stream postStream = request.EndGetRequestStream(asynchronousResult);

                    StreamWriter postDataWriter = new StreamWriter(postStream);

                    foreach (var x in ParametersRef)
                    {
                        if (x.Value.GetType() == typeof(String))
                        {
                            if (!string.IsNullOrEmpty((String)x.Value))
                            {
                                postDataWriter.Write("\r\n--" + BoundaryString + "\r\n");
                                postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", x.Key, x.Value);
                            }
                        }
                        else if (x.Value.GetType() == typeof(int))
                        {
                            postDataWriter.Write("\r\n--" + BoundaryString + "\r\n");
                            postDataWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", x.Key, x.Value.ToString());
                        }
                        else
                        {
                            if ((Stream)x.Value != null)
                            {
                                postDataWriter.Write("\r\n--" + BoundaryString + "\r\n");
                                postDataWriter.Write("Content-Disposition: form-data;"
                                                        + "name=\"{0}\";"
                                                        + "filename=\"{1}\""
                                                        + "\r\nContent-Type: {2}\r\n\r\n",
                                                        x.Key,
                                                        ParametersRef["sequence"].ToString() + ".jpg",
                                                        "image/jpeg");
                                postDataWriter.Flush();
                                byte[] buffer = new byte[1024];
                                int bytesRead = 0;

                                while ((bytesRead = ((Stream)x.Value).Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    postStream.Write(buffer, 0, bytesRead);
                                }
                                ((Stream)x.Value).Close();
                            }
                        }
                    }

                    postDataWriter.Write("\r\n--" + BoundaryString + "--\r\n");
                    postDataWriter.Flush();
                    postStream.Close();

                    request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
                }

                private void GetResponseCallback(IAsyncResult asynchronousResult)
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                        HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                        System.IO.Stream streamResponse = response.GetResponseStream();
                        StreamReader streamRead = new StreamReader(streamResponse);

                        string result = streamRead.ReadToEnd();
                
                        streamResponse.Close();
                        streamRead.Close();
                        response.Close();


                    }
                    catch (Exception ex)
                    {
                    }

                }


        #endregion

    }
}
