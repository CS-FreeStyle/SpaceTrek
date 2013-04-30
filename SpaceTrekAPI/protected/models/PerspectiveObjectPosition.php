<?php

/**
 * This is the model class for table "perspective_object_position".
 *
 * The followings are the available columns in table 'perspective_object_position':
 * @property integer $id_pos
 * @property integer $id_object
 * @property integer $id_user
 * @property string $date
 * @property double $lat
 * @property double $long
 *
 * The followings are the available model relations:
 * @property User $idUser
 * @property SpaceObject $idObject
 */
class PerspectiveObjectPosition extends CActiveRecord
{
	/**
	 * Returns the static model of the specified AR class.
	 * @return PerspectiveObjectPosition the static model class
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
		return 'perspective_object_position';
	}

	/**
	 * @return array validation rules for model attributes.
	 */
	public function rules()
	{
		// NOTE: you should only define rules for those attributes that
		// will receive user inputs.
		return array(
			array('id_object, id_user, date, lat, long', 'required'),
			array('id_object, id_user', 'numerical', 'integerOnly'=>true),
			array('lat, long', 'numerical'),
			// The following rule is used by search().
			// Please remove those attributes that should not be searched.
			array('id_pos, id_object, id_user, date, lat, long', 'safe', 'on'=>'search'),
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
			'idUser' => array(self::BELONGS_TO, 'User', 'id_user'),
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
			'id_user' => 'Id User',
			'date' => 'Date',
			'lat' => 'Lat',
			'long' => 'Long',
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
		$criteria->compare('id_user',$this->id_user);
		$criteria->compare('date',$this->date,true);
		$criteria->compare('lat',$this->lat);
		$criteria->compare('long',$this->long);

		return new CActiveDataProvider($this, array(
			'criteria'=>$criteria,
		));
	}
}