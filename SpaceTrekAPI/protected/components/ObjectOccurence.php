<?php

/**
 *OBjectOccurence is used for predicting when ISS station will travel above given spesific location
 */
class ObjectOccurence
{

	public function getOccurences($type, $id_object, $limit=5, $lat=null, $lon=null, $alt=null, $num_passes=1){

		$result = array();

        $lat = (isset($param["lat"]))? $param["lat"] : -6.7063;
        $lon = (isset($param["lon"]))? $param["lon"] : 108.5570;
        $alt = (isset($param["alt"]))? $param["alt"] : 100;

		if($type == "station"){

            //get from third party api                    
            $objOccurences = json_decode(ISSPrediction::predict($lat, $lon, $alt, $num_passes));

            if(isset($objOccurences->response)){
                foreach ($objOccurences->response as $occ) {

                    if($occ->risetime > time()){

                        $tempOcc = array();
                        $tempOcc["risetime"] = $occ->risetime;
                        $tempOcc["date"] = TimeConversion::convertRisetimeToDate($occ->risetime);
                        $tempOcc["timeleft"] = TimeConversion::timeDifference($occ->risetime);

                        $result[] = $tempOcc;       

                    } 
                    
                }
            }


        }else if(($type == "satellite")||($type == "other")){

            //get from calendar
            $criteria = new CDbCriteria(array(
                "condition" => "id_object = ".$id_object,
                "order" => "date ASC",
                "limit" => $limit,
            ));
            $objOccurences = Calendar::model()->findAll($criteria);

            if(isset($objOccurences)){

                foreach ($objOccurences as $occ) {

                    $date = new DateTime($occ->date);
                    $timestamp = $date->getTimestamp();

                    if($timestamp > time()){

                        $tempOcc = array();
                        $tempOcc["risetime"] = $timestamp;
                        $tempOcc["date"] = $date->format('Y-m-d H:i:s');
                        $tempOcc["timeleft"] = TimeConversion::timeDifference($timestamp);

                        $result[] = $tempOcc;        

                    }
                    
                }
            }

        }else{  //meteor
            $criteria = new CDbCriteria(array(
                "condition" => "id_object = ".$id_object,
                "order" => "date ASC",
                "limit" => 10,
            ));

            $objOccurences = ObjectPosition::model()->findAll($criteria);

            if(isset($objOccurences)){

                foreach ($objOccurences as $occ) {

                    $date = new DateTime($occ->date);
                    $timestamp = $date->getTimestamp();

                    if($timestamp > time()){

                        $tempOcc = array();
                        $tempOcc["risetime"] = $timestamp;
                        $tempOcc["date"] = $date->format('Y-m-d H:i:s');
                        $tempOcc["timeleft"] = TimeConversion::timeDifference($timestamp);

                        $result[] = $tempOcc;        

                    }
                }
            }
        }

		return $result;
        
	}

}

?>