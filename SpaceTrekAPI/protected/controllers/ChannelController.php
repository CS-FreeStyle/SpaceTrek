<?php
Yii::import('application.controllers.ApiController');
class ChannelController extends ApiController
{
 
    // Create Channel
    public function actionAdd($id)
    {
        $ret = false;
        $message = "";
        $response = 400;
        $result = array();
        $param = $_POST;
        $lastId = null;

        //get space object
        $object = SpaceObject::model()->findByPk($id);

        if(!empty($object)) {
            
            $number = 0;
            $lastChanel = Channel::model()->findBySql("SELECT MAX(stream_number) AS stream_number FROM channel WHERE id_object=".$id);
            if(isset($lastChanel)){
                $number = $lastChanel->stream_number;
            }
            $number++;

            $channel = new Channel();
            $channel->attributes = $param;
            $channel->id_object = $id;
            $channel->stream_number = $number;
            $channel->date = date('Y-m-d H:i:s');

            $ret = $channel->save();
            if($ret){
                $lastId = Yii::app()->db->getLastInsertId();
                $message = "Channel created successfully";
                $response = 200;
            }else{
                $message = "Failed in creating channel";
            }

        }else{

            $message = "Invalid id_object. Space Object was not found";

        }

        $result = array(
            "status" => $message,
            "id" => $lastId,
        );

        // Send the response
        $this->_sendResponse($response, CJSON::encode($result));
    }

    // Upload Channel Stream
    public function actionUpload($id)
    {
        $ret = false;
        $message = "";
        $response = 400;
        $result = array();
        $param = $_POST;
        $files = $_FILES["userfile"];
        $isError = false;
        $lastId = null;
        $link = null;

        //get channel
        $channel = Channel::model()->findByPk($id);

        if(!empty($channel)) {
            
            if(!isset($files)){
                $isError = true;
                $message = "File to be uploaded was not found";
            }

            if(!$isError){
                $dir = Yii::app()->basePath.'/../stream/'.$id."/";

                if(!is_dir($dir))
                    mkdir($dir,0777, true);

                $info = pathinfo($files['name']);
                $ext = strtolower($info['extension']);
                $uploadFile = $dir . $param["sequence"].'.'.$ext;
                $link = "http://".$_SERVER["HTTP_HOST"].Yii::app()->baseUrl."/stream/".$id."/".$param["sequence"].".".$ext;

                $isUploaded = move_uploaded_file($files['tmp_name'], $uploadFile);

                if(!$isUploaded){
                    $isError = true;
                    $message = "Can not upload file.";
                }

                if(!$isError){
                    $stream = new Stream();
                    $stream->sequence = $param['sequence'];
                    $stream->id_channel = $id;
                    $stream->file = $link;

                    $ret = $stream->save();
                    if($ret){
                        $lastId = Yii::app()->db->getLastInsertId();
                        $message = "Stream uploaded successfully";
                        $response = 200;
                    }else{
                        $message = "Failed in saving stream";
                    }
                }
            }

        }else{

            $message = "Invalid id channel. Channel was not found";

        }

        $result = array(
            "status" => $message,
            "id" => $lastId,
            "link" => $link,
        );

        // Send the response
        $this->_sendResponse($response, CJSON::encode($result));
    }

    // Get Channel Stream
    public function actionGet($id)
    {
        $sequence = isset($_GET["sequence"])? $_GET["sequence"] : 0;
        $result = array();
        $channelName="";
        $channelUser="";
        $channelDate="";
        $stream="";

        $response = 200;

        //get channel
        $channel = Channel::model()->findByPk($id);

        if(!empty($channel)) {

            $criteria = new CDbCriteria(array(
                "condition" => "id_channel = ".$id." AND sequence > ".$sequence,
                "order" => "sequence ASC",
            ));

            $streams = Stream::model()->findAll($criteria);
            if(count($streams) > 0){
                foreach ($streams as $key => $value) {
                    $temp["sequence"] = $value["sequence"];
                    $temp["file"] = $value["file"];
                    $result[] = $temp;
                }
            }

            $channelName = $channel->idObject->name." #".$channel->stream_number;
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
                $channelUser = $user->username;
            }else{
                $channelUser = "Anonymous";
            }

            $channelDate = $channel->date;
            $stream = $result;
        }

        $ret = array(
            "name"=> $channelName,
            "user"=> $channelUser,
            "date"=> $channelDate,
            "stream"=>$result,
        );

        // Send the response
        $this->_sendResponse($response, CJSON::encode($ret));
    }
}
?>