create a database initialy (this example has MS Sql Server 2019) and create a table in it . use those credentials to form the connection string which will be written appconfig.json.
if the application throws the exception such as " the connection was successful but the there was proble in login " then go to SSMS 19 omn dialog box "Connect to server " click on options(bleow right hand side)
click on the check box trust server certificate to true.. that will resolve the error.
