<?php
Yii::import('application.controllers.ApiController');
class UserController extends ApiController
{
 
    // List All Users
    // deprecated
    /*
    public function actionList()
    {
        $users = array();

        //get all users
        $result = User::model()->findAll();

        if(!empty($result)) {
            foreach($result as $item)
                $users[] = $item->attributes;
        }

        // Send the response
        $this->_sendResponse(200, CJSON::encode($users));
    }
    */

    // Get User
    public function actionGet($id)
    {
        $user = null;

        //get user
        $endpointUrl = "http://api.tripboard.me/usera?";
        $data = array(
            "id" => $id,
        );

        $auth = AuthHeader::getAuth();
        Yii::app()->curl->setOption(CURLOPT_HTTPAUTH, CURLAUTH_BASIC);
        Yii::app()->curl->setOption(CURLOPT_USERPWD, $auth);
        $user = Yii::app()->curl->get($endpointUrl, $data);
        $user = json_decode($user);

        // get from db
        $result = User::model()->findByPk($id);

        if(!empty($result)) {
            $attributes = $result->attributes;
            foreach ($attributes as $key => $value) {
                if($key !== 'id_user')
                    $user->$key = $value;
            }
        }

        // Send the response
        $this->_sendResponse(200, CJSON::encode($user));
    }

    public function actionUpdate($id)
    {
        $user = null;
        $ret = false;
        $message = "";
        $response = 400;
        $result = array();
        $param = $_POST;

        //get user
        $user = User::model()->findByPk($id);

        if(!empty($user)) {
            $user->attributes = $param;
            $ret = $user->save();
            if($ret){
                $message = "Saved successfully";
                $response = 200;
            }else{
                $message = "Failed in updating user";
            }
        }else{

            //$message = "User not found";

            //save new
            $user = new User();
            $user->attributes = $param;
            $user->id_user =$id;

            $ret = $user->save();
            if($ret){
                $message = "Saved successfully";
                $response = 200;
            }else{
                $message = "Failed in updating user";
            }
        }

        $result = array(
            "status" => $message,
        );

        // Send the response
        $this->_sendResponse($response, CJSON::encode($result));
    }
}
?>