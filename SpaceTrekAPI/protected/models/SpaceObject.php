<?php

/**
 * This is the model class for table "space_object".
 *
 * The followings are the available columns in table 'space_object':
 * @property integer $id_object
 * @property string $type
 * @property string $name
 * @property string $description
 *
 * The followings are the available model relations:
 * @property Calendar[] $calendars
 * @property Channel[] $channels
 * @property ObjectPosition[] $objectPositions
 * @property PerspectiveObjectPosition[] $perspectiveObjectPositions
 * @property UserEvent[] $userEvents
 */
class SpaceObject extends CActiveRecord
{
	/**
	 * Returns the static model of the specified AR class.
	 * @return SpaceObject the static model class
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
		return 'space_object';
	}

	/**
	 * @return array validation rules for model attributes.
	 */
	public function rules()
	{
		// NOTE: you should only define rules for those attributes that
		// will receive user inputs.
		return array(
			array('type, name, description', 'required'),
			array('type', 'length', 'max'=>9),
			array('name', 'length', 'max'=>100),
			// The following rule is used by search().
			// Please remove those attributes that should not be searched.
			array('id_object, type, name, description', 'safe', 'on'=>'search'),
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
			'calendars' => array(self::HAS_MANY, 'Calendar', 'id_object'),
			'channels' => array(self::HAS_MANY, 'Channel', 'id_object'),
			'objectPositions' => array(self::HAS_MANY, 'ObjectPosition', 'id_object'),
			'perspectiveObjectPositions' => array(self::HAS_MANY, 'PerspectiveObjectPosition', 'id_object'),
			'userEvents' => array(self::HAS_MANY, 'UserEvent', 'id_object'),
		);
	}

	/**
	 * @return array customized attribute labels (name=>label)
	 */
	public function attributeLabels()
	{
		return array(
			'id_object' => 'Id Object',
			'type' => 'Type',
			'name' => 'Name',
			'description' => 'Description',
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

		$criteria->compare('id_object',$this->id_object);
		$criteria->compare('type',$this->type,true);
		$criteria->compare('name',$this->name,true);
		$criteria->compare('description',$this->description,true);

		return new CActiveDataProvider($this, array(
			'criteria'=>$criteria,
		));
	}
}