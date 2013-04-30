<?php

/**
 * ISSPrediction is used for predicting when ISS station will travel above given spesific location
 */
class ISSPrediction
{

	public function predict($lat=0, $lon=0, $alt=0, $num_passes=1){

		$endpointUrl  = "http://api.open-notify.org/iss/?";

		$data = array(
			"lat"=>$lat,
			"lon"=>$lon,
			"alt"=>$alt,
			"n"=>$num_passes,
		);

		$result = Yii::app()->curl->get($endpointUrl, $data);

		return $result;
        
	}

}

?>