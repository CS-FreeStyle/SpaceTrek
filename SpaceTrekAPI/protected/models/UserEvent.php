<?php

/**
 * This is the model class for table "user_event".
 *
 * The followings are the available columns in table 'user_event':
 * @property integer $id_event
 * @property integer $id_user
 * @property integer $id_object
 * @property string $fb_event_id
 *
 * The followings are the available model relations:
 * @property User $idUser
 * @property SpaceObject $idObject
 */
class UserEvent extends CActiveRecord
{
	/**
	 * Returns the static model of the specified AR class.
	 * @return UserEvent the static model class
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
		return 'user_event';
	}

	/**
	 * @return array validation rules for model attributes.
	 */
	public function rules()
	{
		// NOTE: you should only define rules for those attributes that
		// will receive user inputs.
		return array(
			array('id_user, id_object, fb_event_id', 'required'),
			array('id_user, id_object', 'numerical', 'integerOnly'=>true),
			array('fb_event_id', 'length', 'max'=>200),
			// The following rule is used by search().
			// Please remove those attributes that should not be searched.
			array('id_event, id_user, id_object, fb_event_id', 'safe', 'on'=>'search'),
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
			'id_event' => 'Id Event',
			'id_user' => 'Id User',
			'id_object' => 'Id Object',
			'fb_event_id' => 'Fb Event',
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

		$criteria->compare('id_event',$this->id_event);
		$criteria->compare('id_user',$this->id_user);
		$criteria->compare('id_object',$this->id_object);
		$criteria->compare('fb_event_id',$this->fb_event_id,true);

		return new CActiveDataProvider($this, array(
			'criteria'=>$criteria,
		));
	}
}