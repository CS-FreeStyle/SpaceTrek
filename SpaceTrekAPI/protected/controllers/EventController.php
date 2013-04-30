<?php
Yii::import('application.controllers.ApiController');
class EventController extends ApiController
{
    public function actionInvite($id)
    {
    	$tUserEvent = UserEvent::model()->findByPk($id);
    	$tFBEventID = $tUserEvent->fb_event_id;
    	
        $tUserEvent = null;
        $ret = false;
        $message = "";
        $result = array();
        $param = $_POST;
        
        $tUserIDS = $param["id_users"];
	$tToken=  $param["token"];
        
        //$tURL = "https://graph.facebook.com/" . $tFBEventID . "/invited?access_token=BAACEdEose0cBAMTmEZATFGlFlwl9TNpaf617woe0iU6AyrNNu85knqLOG1KhmEdZACV6vZCIbe1w06t50LR6HZAj2ie0MhfioLe1Nh7Dk0OzA0ZBUysZAGAN2DYfqKzmDRa6OmeP993TLwxSfm8ZARwRdVFQWvUIyZC6tplSZCkaDz1xZBgZA64SZApjslrU5momJurE488FHjMp4AZDZD";
	$tURL = "https://graph.facebook.com/" . $tFBEventID . "/invited?access_token=" . $tToken;
        
        $tData["users"] = $tUserIDS;
        
		$tOutput = Yii::app()->curl->post($tURL, $tData);

		$this->_sendResponse(200, "success");
    }

    public function actionCreate()
    {
        $tUserEvent = null;
        $ret = false;
        $message = "";
        $response = 400;
        $result = array();
        $param = $_POST;
        
        $tUserID = $param["id_user"];
        $tObjectID = $param["id_object"];
	$tToken = $param["token"];

	//$tURL = "https://graph.facebook.com/me/events?access_token=BAACEdEose0cBAMTmEZATFGlFlwl9TNpaf617woe0iU6AyrNNu85knqLOG1KhmEdZACV6vZCIbe1w06t50LR6HZAj2ie0MhfioLe1Nh7Dk0OzA0ZBUysZAGAN2DYfqKzmDRa6OmeP993TLwxSfm8ZARwRdVFQWvUIyZC6tplSZCkaDz1xZBgZA64SZApjslrU5momJurE488FHjMp4AZDZD";
	$tURL = "https://graph.facebook.com/me/events?access_token=" . $tToken;
        
        $tObject = SpaceObject::model()->findByPk($tObjectID);
        $tOccurences = ObjectOccurence::getOccurences($tObject->type, $tObject->id_object, 1);
        if(count($tOccurences) > 0)
        {
        	$tRisetime = $tOccurences[0]["risetime"];
//        	$tDate = "2013-04-30T06:26:45+0700";
//        	$tDate = "2013-04-21T06:45:49+0700";
	        $tDate = date("Y-m-d\TH:i:sO", $tRisetime);
//			$tDate = date("Y-m-d H:i:s", $tRisetime);
	
			$tData["name"] = $tObject->name . " Sighting";
			$tData["start_time"] = $tDate;
			$tData["description"] = $tObject->description;
			$tOutput = Yii::app()->curl->post($tURL, $tData);
			
			$tObjectOutput = json_decode($tOutput);
//			echo $tOutput;
			$tFBEventID = $tObjectOutput->id;
	
			$tUserEvent = new UserEvent();
			$tUserEvent->id_user = $tUserID;
			$tUserEvent->id_object = $tObjectID;
			$tUserEvent->fb_event_id = $tFBEventID;
			
			$ret = $tUserEvent->save();
			$tEventID = 0;
			if($ret)
			{
				$message = "Saved successfully";
				$response = 200;
				$tEventID = Yii::app()->db->getLastInsertId();
			}
			else
			{
				$message = "Failed to create event";
			}
        }

        $result = array(
            "status" => $message,
            "event_id" => $tEventID
        );

//         Send the response
        $this->_sendResponse($response, CJSON::encode($result));
    }
}
?>