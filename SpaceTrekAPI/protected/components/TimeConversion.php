<?php

/**
 * TimeConversion is used for converting time to date format
 */
class TimeConversion
{

	public function convertRisetimeToDate($risetime)
	{
		$date = date("Y-m-d H:i:s.u", $risetime);
		return $date;
	}

	public function timeDifference($risetime)
	{
		$now = time();
		$diff = ((int)$risetime - $now);

		return $diff;
	}

}

?>