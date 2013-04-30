<html>
	<head>

	</head>
	<body>
		<form method="post" action="upload.php" enctype="multipart/form-data">
			<input type="file" name="userfile" />
			<button type='submit' name="submit" value="upload">Submit</button>
		</form>
		<form method="post" action="http://119.81.24.210/parisvanjava/spacetrek/channel/upload/1" enctype="multipart/form-data">
			<input type="file" name="userfile" />
			<input type="hidden" name="id_channel" value="1" />
			<input type="hidden" name="sequence" value="1" />
			<button type='submit' name="submit" value="upload">Submit</button>
		</form>
	</body>
</html>