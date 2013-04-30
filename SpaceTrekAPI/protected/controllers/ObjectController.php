<?php
Yii::import('application.controllers.ApiController');
class ObjectController extends ApiController
{
 
    // List All Objects
    public function actionList()
    {
        $objects = array();
        $param = $_GET;

        $lat = (isset($param["lat"]))? $param["lat"] : -6.7063;
        $lon = (isset($param["lon"]))? $param["lon"] : 108.5570;
        $alt = (isset($param["alt"]))? $param["alt"] : 100;
        $n = (isset($param["n"]))? $param["n"] : 1;
        $limit = 5;

        //get all space objects
        $result = SpaceObject::model()->findAll();

        if(!empty($result)) {
            foreach($result as $item){
                $temp = $item->attributes;
                $temp["occurences"] = ObjectOccurence::getOccurences($item->type, $item->id_object, $limit, $lat, $lon, $alt, $n);

                $objects[] = $temp;
            }
        }

        usort($objects, "sort_by_shortest_time");

        // Send the response
        $this->_sendResponse(200, CJSON::encode($objects));
    }

    // Get Object
    public function actionGet($id)
    {
        $object = null;
        $lat = (isset($param["lat"]))? $param["lat"] : -6.7063;
        $lon = (isset($param["lon"]))? $param["lon"] : 108.5570;
        $alt = (isset($param["alt"]))? $param["alt"] : 100;
        $n = (isset($param["n"]))? $param["n"] : 1;
        $limit = 5;

        //get space object
        $result = SpaceObject::model()->findByPk($id);

        if(!empty($result)) {
            $temp = $result->attributes;
            $temp["occurences"] = ObjectOccurence::getOccurences($result->type, $result->id_object, $limit, $lat, $lon, $alt, $n);
            $temp["channels"] = array();

            //get all channels
            $criteria = new CDbCriteria(array(
                "condition" => "id_object = ".$result->id_object,
                "order" => "stream_number ASC",
            ));

            $channels = Channel::model()->findAll($criteria);
            if(count($channels) > 0){
                $tempChannel = array();
                foreach ($channels as $channel) {

                    //get user
                    $endpointUrl = "http://api.tripboard.me/usera?";
                    $data = array(
                        "id" => $channel->id_user,
                    );

                    $auth = AuthHeader::getAuth();
                    Yii::app()->curl->setOption(CURLOPT_HTTPAUTH, CURLAUTH_BASIC);
                    Yii::app()->curl->setOption(CURLOPT_USERPWD, $auth);
                    $user = Yii::app()->curl->get($endpointUrl, $data);
                    $user = json_decode($user);

                    if(isset($user->username)){
                        $tempChannel["user"] = $user->username;
                    }else{
                        $tempChannel["user"] = "Anonymous";
                    }

                    $tempChannel["id_channel"] = $channel->id_channel;
                    $tempChannel["stream_number"] = $channel->stream_number;
                    $tempChannel["date"] = $channel->date;

                    $temp["channels"][] = $tempChannel;        
                }
            }

            $object = $temp;
        }


        // Send the response
        $this->_sendResponse(200, CJSON::encode($object));
    }

    // Get Closest Object
    public function actionClosest()
    {
        $objects = array();
        $lat = (isset($param["lat"]))? $param["lat"] : -6.7063;
        $lon = (isset($param["lon"]))? $param["lon"] : 108.5570;
        $alt = (isset($param["alt"]))? $param["alt"] : 100;
        $n = (isset($param["n"]))? $param["n"] : 1;
        $limit = 1;

        //get all space objects
        $result = SpaceObject::model()->findAll();

        if(!empty($result)) {
            foreach($result as $item){
                $temp = $item->attributes;
                $temp["occurences"] = ObjectOccurence::getOccurences($item->type, $item->id_object, $limit, $lat, $lon, $alt, $n);
                $temp["channels"] = array();

                //get all channels
                $criteria = new CDbCriteria(array(
                    "condition" => "id_object = ".$item->id_object,
                    "order" => "stream_number ASC",
                ));

                $channels = Channel::model()->findAll($criteria);
                if(count($channels) > 0){
                    $tempChannel = array();
                    foreach ($channels as $channel) {
                         //get user
                        $endpointUrl = "http://api.tripboard.me/usera?";
                        $data = array(
                            "id" => $channel->id_user,
                        );

                        $auth = AuthHeader::getAuth();
                        Yii::app()->curl->setOption(CURLOPT_HTTPAUTH, CURLAUTH_BASIC);
                        Yii::app()->curl->setOption(CURLOPT_USERPWD, $auth);
                        $user = Yii::app()->curl->get($endpointUrl, $data);
                        $user = json_decode($user);

                        if(isset($user->username)){
                            $tempChannel["user"] = $user->username;
                        }else{
                            $tempChannel["user"] = "Anonymous";
                        }

                        $tempChannel["id_channel"] = $channel->id_channel;
                        $tempChannel["stream_number"] = $channel->stream_number;
                        $tempChannel["date"] = $channel->date;

                        $temp["channels"][] = $tempChannel;        
                    }
                }

                $objects[] = $temp;
            }
        }


        usort($objects, "sort_by_shortest_time");
        $object = $objects[0];

        // Send the response
        $this->_sendResponse(200, CJSON::encode($object));
    }

    public function actionAdd(){

        $ret = false;
        $message = "";
        $response = 400;
        $result = array();
        $param = $_POST;
        $lastId = null;
        
        //add space object
        if(!isset($param["name"])){

            $message = "Name can not be empty";
        }else{
            $object = SpaceObject::model()->findByAttributes(array("name"=>$param["name"]));

            if(empty($object)) {
                
                $object = new SpaceObject();

                $object->type= (isset($param["type"]))? $param["type"] : "other";
                $object->name= $param["name"];
                $object->description= $param["name"];
                $object->image= "";

                $ret = $object->save();

                if($ret){
                    $lastId = Yii::app()->db->getLastInsertId();

                    $objectPos = new ObjectPosition();
                    $objectPos->id_object = $lastId;
                    $objectPos->date = $param["date"];
                    $objectPos->lat = (isset($param["lat"]))? $param["lat"] : "0";
                    $objectPos->lon = (isset($param["lon"]))? $param["lon"] : "0";

                    $ret = $objectPos->save();
                    if($ret){

                        $message = "Object created successfully";
                        $response = 200;

                    }else{

                        $message = "Object created successfully, but failed in saving occurence";

                    }
                }else{
                    $message = "Failed in saving object";
                }

            }else{

                $lastId = $object->id_object;

                //add occurence
                $objectPos = new ObjectPosition();
                $objectPos->id_object = $lastId;
                $objectPos->date = $param["date"];
                $objectPos->lat = (isset($param["lat"]))? $param["lat"] : "0";
                $objectPos->lon = (isset($param["lon"]))? $param["lon"] : "0";

                $ret = $objectPos->save();
                if($ret){

                    $message = "Occurence saved successfully";
                    $response = 200;

                }else{

                    $message = "Failed in saving occurence";

                }
            }
        }

        $result = array(
            "status" => $message,
            "id" => $lastId,
        );

        // Send the response
        $this->_sendResponse($response, CJSON::encode($result));

    }

}

function sort_by_shortest_time($a, $b) {
    if((isset($a["occurences"][0]))&&(!isset($b["occurences"][0]))){
        return -1;
    }else if((!isset($a["occurences"][0]))&&(isset($b["occurences"][0]))){
        return 1;
    }else if((!isset($a["occurences"][0]))&&(!isset($b["occurences"][0]))){
        return 1;
    }else if($a["occurences"][0]["timeleft"] < $b["occurences"][0]["timeleft"]){
        return -1;
    }else{
        return 1;
    }
}
?>