<?php

/**
 * This is the model class for table "object_position".
 *
 * The followings are the available columns in table 'object_position':
 * @property integer $id_pos
 * @property integer $id_object
 * @property string $date
 * @property double $lat
 * @property double $lon
 *
 * The followings are the available model relations:
 * @property SpaceObject $idObject
 */
class ObjectPosition extends CActiveRecord
{
	/**
	 * Returns the static model of the specified AR class.
	 * @return ObjectPosition the static model class
	 */
	public static function model($className=__CLASS__)
	{
		return parent::model($className);
	}

	/**
	 * @return string the associated database table name
	 */
	public function tableName()
	{
		return 'object_position';
	}

	/**
	 * @return array validation rules for model attributes.
	 */
	public function rules()
	{
		// NOTE: you should only define rules for those attributes that
		// will receive user inputs.
		return array(
			array('id_object, date, lat, lon', 'required'),
			array('id_object', 'numerical', 'integerOnly'=>true),
			array('lat, lon', 'numerical'),
			// The following rule is used by search().
			// Please remove those attributes that should not be searched.
			array('id_pos, id_object, date, lat, lon', 'safe', 'on'=>'search'),
		);
	}

	/**
	 * @return array relational rules.
	 */
	public function relations()
	{
		// NOTE: you may need to adjust the relation name and the related
		// class name for the relations automatically generated below.
		return array(
			'idObject' => array(self::BELONGS_TO, 'SpaceObject', 'id_object'),
		);
	}

	/**
	 * @return array customized attribute labels (name=>label)
	 */
	public function attributeLabels()
	{
		return array(
			'id_pos' => 'Id Pos',
			'id_object' => 'Id Object',
			'date' => 'Date',
			'lat' => 'Lat',
			'lon' => 'Lon',
		);
	}

	/**
	 * Retrieves a list of models based on the current search/filter conditions.
	 * @return CActiveDataProvider the data provider that can return the models based on the search/filter conditions.
	 */
	public function search()
	{
		// Warning: Please modify the following code to remove attributes that
		// should not be searched.

		$criteria=new CDbCriteria;

		$criteria->compare('id_pos',$this->id_pos);
		$criteria->compare('id_object',$this->id_object);
		$criteria->compare('date',$this->date,true);
		$criteria->compare('lat',$this->lat);
		$criteria->compare('lon',$this->lon);

		return new CActiveDataProvider($this, array(
			'criteria'=>$criteria,
		));
	}
}