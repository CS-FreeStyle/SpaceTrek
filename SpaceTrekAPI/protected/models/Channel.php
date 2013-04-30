<?php

/**
 * This is the model class for table "channel".
 *
 * The followings are the available columns in table 'channel':
 * @property integer $id_channel
 * @property integer $id_object
 * @property integer $id_user
 * @property integer $stream_number
 * @property string $date
 *
 * The followings are the available model relations:
 * @property User $idUser
 * @property SpaceObject $idObject
 * @property Stream[] $streams
 */
class Channel extends CActiveRecord
{
	/**
	 * Returns the static model of the specified AR class.
	 * @return Channel the static model class
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
		return 'channel';
	}

	/**
	 * @return array validation rules for model attributes.
	 */
	public function rules()
	{
		// NOTE: you should only define rules for those attributes that
		// will receive user inputs.
		return array(
			array('id_object, id_user, date', 'required'),
			array('id_object, id_user, stream_number', 'numerical', 'integerOnly'=>true),
			// The following rule is used by search().
			// Please remove those attributes that should not be searched.
			array('id_channel, id_object, id_user, stream_number, date', 'safe', 'on'=>'search'),
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
			'streams' => array(self::HAS_MANY, 'Stream', 'id_channel'),
		);
	}

	/**
	 * @return array customized attribute labels (name=>label)
	 */
	public function attributeLabels()
	{
		return array(
			'id_channel' => 'Id Channel',
			'id_object' => 'Id Object',
			'id_user' => 'Id User',
			'stream_number' => 'Stream Number',
			'date' => 'Date',
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

		$criteria->compare('id_channel',$this->id_channel);
		$criteria->compare('id_object',$this->id_object);
		$criteria->compare('id_user',$this->id_user);
		$criteria->compare('stream_number',$this->stream_number);
		$criteria->compare('date',$this->date,true);

		return new CActiveDataProvider($this, array(
			'criteria'=>$criteria,
		));
	}
}